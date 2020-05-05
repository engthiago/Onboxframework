using Onbox.Core.V6.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Onbox.Store.V6
{
    public class StateHistory<T> where T : class, new()
    {
        public DateTimeOffset UpdatedAt { get; set; }
        public T State { get; set; }
        public string Action { get; set; }
    }

    public interface IStoreAction<T> where T : new()
    {
        string GetAction();
    }

    public class StorageSubscription
    {
        private readonly List<Delegate> callbacks;
        private Delegate target;

        internal StorageSubscription(List<Delegate> callbacks, Delegate target)
        {
            this.callbacks = callbacks;
            this.target = target;
        }

        public void Unsubscribe()
        {
            Console.WriteLine("Unsubscribing...");
            callbacks.Remove(target);
            target = null;
        }
    }

    public class Store<T> where T : class, INotifyPropertyChanged, new()
    {
        private T store = new T();
        private readonly IMapper mapper;

        private readonly List<StateHistory<T>> stateHistory = new List<StateHistory<T>>();

        public List<Delegate> maincallbacks = new List<Delegate>();
        public Dictionary<string, List<Delegate>> callbacks = new Dictionary<string, List<Delegate>>();

        public Store(IMapper mapper)
        {
            this.stateHistory.Add(new StateHistory<T> { Action = null, State = null, UpdatedAt = DateTimeOffset.UtcNow });
            this.mapper = mapper;
        }

        public T State => this.store;

        public StorageSubscription Subscribe<TState>(IStoreAction<TState> action, Action<TState> callback) where TState : new()
        {
            var actionString = action.GetAction();

            if (actionString == null)
            {
                return new StorageSubscription(maincallbacks, callback);
            }

            EnsureActionType(action, actionString);

            if (callbacks.ContainsKey(actionString))
            {
                var list = callbacks[actionString];
                list.Add(callback);
                return new StorageSubscription(list, callback);
            }
            else
            {
                var list = new List<Delegate>();
                list.Add(callback);
                callbacks.Add(actionString, list);
                return new StorageSubscription(list, callback);
            }
        }

        private void EnsureActionType<TState>(IStoreAction<TState> action, string actionString) where TState : new()
        {
            if (actionString != null)
            {
                var propertyType = GetPropertyType(typeof(T), actionString);
                if (propertyType == null)
                {
                    throw new ArgumentException($"Action {action.GetType().FullName} has an invalid path: {actionString}");
                }

                var tpe = typeof(TState);
                if (tpe != propertyType)
                {
                    throw new ArgumentException($"Action {action.GetType().FullName} has a property type mismatch, it should be {tpe.FullName} instead of {propertyType.FullName}");
                }
            }
        }

        public Type GetPropertyType(Type type, string path)
        {
            var currentType = type;
            var pathArr = path?.Split('.');

            foreach (var pathItem in pathArr)
            {
                if (currentType == null)
                {
                    return null;
                }
                var prop = currentType.GetProperty(pathItem);
                if (prop == null)
                {
                    return null;
                }

                currentType = prop.PropertyType;
            }

            return currentType;
        }

        public void SetState<TState>(TState state, IStoreAction<TState> action) where TState : new()
        {
            var actionString = action?.GetAction();
            EnsureActionType(action, actionString);

            var newStore = this.mapper.Map<T>(store);
            var pathArr = actionString?.Split('.');

            object currentObject = newStore;

            if (pathArr != null)
            {
                foreach (var pathItem in pathArr)
                {
                    var prop = currentObject.GetType().GetProperty(pathItem);
                    var newObject = prop.GetValue(currentObject);

                    if (newObject == null)
                    {
                        newObject = Activator.CreateInstance(prop.PropertyType);
                        prop.SetValue(currentObject, newObject);
                    }

                    currentObject = newObject;
                }
            }

            mapper.Map(state, currentObject);

            var stateEntry = new StateHistory<T>
            {
                Action = actionString,
                State = newStore,
                UpdatedAt = DateTimeOffset.Now
            };
            stateHistory.Add(stateEntry);

            store = newStore;

            if (actionString != null && callbacks.ContainsKey(actionString))
            {
                foreach (var callback in callbacks[actionString])
                {
                    callback.DynamicInvoke(currentObject);
                }
            }
            if (actionString == null)
            {
                foreach (var callback in maincallbacks)
                {
                    callback.DynamicInvoke(currentObject);
                }
            }
        }

    }
}
