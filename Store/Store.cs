using Onbox.Core.V6.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

    public interface IStoreAction
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

        private readonly List<StateHistory<T>> storeHistory = new List<StateHistory<T>>();

        public List<Delegate> maincallbacks = new List<Delegate>();
        public Dictionary<string, List<Delegate>> callbacks = new Dictionary<string, List<Delegate>>();

        public Store(IMapper mapper)
        {
            this.storeHistory.Add(new StateHistory<T> { Action = null, State = null, UpdatedAt = DateTimeOffset.UtcNow });
            this.mapper = mapper;
        }

        public T State => this.store;

        public StorageSubscription Subscribe<TState>(string action, Action<TState> callback)
        {
            if (action == null)
            {
                return new StorageSubscription(maincallbacks, callback);
            }

            if (callbacks.ContainsKey(action))
            {
                var list = callbacks[action];
                list.Add(callback);
                return new StorageSubscription(list, callback);
            }
            else
            {
                var list = new List<Delegate>();
                list.Add(callback);
                callbacks.Add(action, list);
                return new StorageSubscription(list, callback);
            }
        }

        public void SetState<TState>(TState state, string action) where TState : new()
        {
            var newStore = this.mapper.Map<T>(store);
            var pathArr = action?.Split('.');

            object currentObject = newStore;

            if (pathArr!= null)
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
                Action = action,
                State = newStore,
                UpdatedAt = DateTimeOffset.Now
            };
            storeHistory.Add(stateEntry);

            store = newStore;

            if (action != null && callbacks.ContainsKey(action))
            {
                foreach (var callback in callbacks[action])
                {
                    callback.DynamicInvoke(currentObject);
                }
            }
            if (action == null)
            {
                foreach (var callback in maincallbacks)
                {
                    callback.DynamicInvoke(currentObject);
                }
            }
        }

    }
}
