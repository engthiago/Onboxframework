using Onbox.Core.V6.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Onbox.Store.V6
{
    public class StateHistory<T> where T : class, new()
    {
        public DateTimeOffset UpdatedAt { get; set; }
        public T OldState { get; set; }
        public T NewState { get; set; }
        public string Action { get; set; }
    }

    public interface IStorageSubscription
    {
        void Unsubscribe();
    }

    public class StorageSubscription : IStorageSubscription
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

    public interface IStoreAction<T> where T : new()
    {
        string GetAction();
    }

    public interface IStore<T>: INotifyPropertyChanged where T : class, new()
    {
        T State { get; }
        Type GetPropertyType(Type type, string path);
        void SetState<TState>(TState state, IStoreAction<TState> action) where TState : new();
        IStorageSubscription Subscribe<TState>(IStoreAction<TState> action, Action<TState> callback) where TState : new();
    }

    public class Store<T> : IStore<T> where T : class, new()
    {
        private T store = new T();
        public T State => this.store;

        private readonly IMapper mapper;

        private readonly List<StateHistory<T>> stateHistory = new List<StateHistory<T>>();

        public List<Delegate> maincallbacks = new List<Delegate>();
        public Dictionary<string, List<Delegate>> callbacks = new Dictionary<string, List<Delegate>>();

        public event PropertyChangedEventHandler PropertyChanged;

        public Store(IMapper mapper)
        {
            this.stateHistory.Add(new StateHistory<T> { Action = null, NewState = null, UpdatedAt = DateTimeOffset.UtcNow });
            this.mapper = mapper;
        }

        public IStorageSubscription Subscribe<TState>(IStoreAction<TState> action, Action<TState> callback) where TState : new()
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
            // Ensures that the action has the correct type
            var actionString = action?.GetAction();
            EnsureActionType(action, actionString);

            // Copies the new state of the store
            var newStore = this.mapper.Map<T>(this.store);
            var pathArr = actionString?.Split('.');

            // Gets the Object that will be changed
            object currentObject = GetStoreObjectByPath(newStore, pathArr);

            // Maps the new state to the object that will be changed
            mapper.Map(state, currentObject);

            // Adds the state history
            var stateEntry = new StateHistory<T>
            {
                Action = actionString,
                OldState = this.store,
                NewState = newStore,
                UpdatedAt = DateTimeOffset.Now
            };
            stateHistory.Add(stateEntry);

            // Changes the store to the new state
            this.store = newStore;

            // Notifies the subscribers to cild states
            if (actionString != null && callbacks.ContainsKey(actionString))
            {
                foreach (var callback in callbacks[actionString])
                {
                    callback.DynamicInvoke(currentObject);
                }
            }

            // Notifies the main subscribers
            if (actionString == null)
            {
                foreach (var callback in maincallbacks)
                {
                    callback.DynamicInvoke(currentObject);
                }
            }

            // Notifies that the state was changed
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.State)));
        }

        private object GetStoreObjectByPath(T newStore, string[] pathArr)
        {
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

            return currentObject;
        }
    }
}
