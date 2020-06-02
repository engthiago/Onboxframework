using Onbox.Core.V7.Mapping;
using Onbox.Core.V7.Logging;
using System;
using System.Collections.Generic;
using Onbox.Core.V7.Json;

namespace Onbox.Store.V7
{
    public class StateEntry<T> where T : class, new()
    {
        public DateTimeOffset UpdatedAt { get; set; }
        public T OldState { get; set; }
        public T NewState { get; set; }
        public string ActionName { get; set; }
        public string ActionPath { get; set; }
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
        string GetActionName();
        string GetActionPath();
    }

    public interface IStore<T> where T : class, new()
    {
        void EnableLogging();
        void DisableLogging();
        void SetState<TState>(TState state, IStoreAction<TState> action) where TState : new();
        TState Select<TState>(IStoreAction<TState> action) where TState : new();
        IStorageSubscription Subscribe<TState>(IStoreAction<TState> action, Action<TState> callback) where TState : new();
        List<StateEntry<T>> GetHistory();
    }

    public class Store<T> : IStore<T> where T : class, new()
    {
        private T store = new T();
        //public T State => this.mapper.Map<T>(this.store);

        private readonly IMapper mapper;
        private readonly ILoggingService logging;
        private readonly IJsonService jsonService;

        private bool isLogEnabled;

        private readonly List<StateEntry<T>> stateHistory = new List<StateEntry<T>>();
        public List<Delegate> maincallbacks = new List<Delegate>();
        public Dictionary<string, List<Delegate>> callbacks = new Dictionary<string, List<Delegate>>();

        public Store(IMapper mapper, ILoggingService logging, IJsonService jsonService)
        {
            this.stateHistory.Add(new StateEntry<T> { ActionName = null, NewState = null, UpdatedAt = DateTimeOffset.UtcNow });
            this.mapper = mapper;
            this.logging = logging;
            this.jsonService = jsonService;
        }

        public void EnableLogging()
        {
            isLogEnabled = true;
        }

        public void DisableLogging()
        {
            isLogEnabled = false;
        }

        public IStorageSubscription Subscribe<TState>(IStoreAction<TState> action, Action<TState> callback) where TState : new()
        {
            var actionString = action.GetActionPath();

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

        public TState Select<TState>(IStoreAction<TState> action) where TState: new()
        {
            // Ensures that the action has the correct type
            var actionString = action?.GetActionPath();
            EnsureActionType(action, actionString);

            var pathArr = actionString?.Split('.');

            // Gets the Object that will be changed
            object currentObject = GetStoreObjectByPath(this.store, pathArr);

            if (currentObject == null)
            {
                new TState();
            }

            return this.mapper.Map<TState>(currentObject);
        }

        public void SetState<TState>(TState state, IStoreAction<TState> action) where TState : new()
        {
            // Ensures that the action has the correct type
            var actionPath = action?.GetActionPath();
            var actionName = action?.GetActionName();
            EnsureActionType(action, actionPath);

            // Copies the new state of the store
            var newStore = this.mapper.Map<T>(this.store);
            var pathArr = actionPath?.Split('.');

            // Gets the Object that will be changed
            object currentObject = GetStoreObjectByPath(newStore, pathArr);

            // Maps the new state to the object that will be changed
            mapper.Map(state, currentObject);

            // Adds the state history
            var stateEntry = new StateEntry<T>
            {
                ActionPath = actionPath,
                ActionName = actionName,
                OldState = this.store,
                NewState = newStore,
                UpdatedAt = DateTimeOffset.Now
            };
            stateHistory.Add(stateEntry);

            // Changes the store to the new state
            this.store = newStore;

            // Notifies the subscribers to cild states
            if (actionPath != null && callbacks.ContainsKey(actionPath))
            {
                foreach (var callback in callbacks[actionPath])
                {
                    callback.DynamicInvoke(currentObject);
                }
            }

            // Notifies the main subscribers
            if (actionPath == null)
            {
                foreach (var callback in maincallbacks)
                {
                    callback.DynamicInvoke(currentObject);
                }
            }

            // Logs the history
            if (isLogEnabled)
            {
                logging.Log($"*** State changed on {typeof(T).Name} ***");
                logging.Log(jsonService.Serialize(stateEntry));
            }

            // Notifies that the state was changed
            //this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.State)));
        }

        public List<StateEntry<T>> GetHistory()
        {
            return this.mapper.Map<List<StateEntry<T>>>(this.stateHistory);
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

        private Type GetPropertyType(Type type, string path)
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
    }
}
