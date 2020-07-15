using Onbox.Core.V7.Mapping;
using System;
using System.Collections.Generic;
using Onbox.Abstractions.V7;
using System.Linq.Expressions;

namespace Onbox.Store.V7
{
    public class StateEntry<TState> where TState : class, new()
    {
        public DateTimeOffset UpdatedAt { get; set; }
        public TState OldState { get; set; }
        public TState NewState { get; set; }
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

    public interface IStoreAction<TStore, TState, TSlice> where TStore : IStore<TState> where TState : class, new() where TSlice : class, new()
    {
        string GetActionName();
        Expression<Func<TState, TSlice>> GetActionPath();
    }

    public interface IStore<TState> where TState : class, new()
    {
        void EnableLogging();
        void DisableLogging();
        void SetState<TStore, TSlice>(TState state, IStoreAction<TStore, TState, TSlice> action) where TStore : IStore<TState> where TSlice : class, new();
        TSlice Select<TStore, TSlice>(IStoreAction<TStore, TState, TSlice> action) where TStore : IStore<TState> where TSlice : class, new();
        IStorageSubscription Subscribe<TStore, TSlice>(IStoreAction<TStore, TState, TSlice> action, Action<TSlice> callback) where TStore : IStore<TState> where TSlice : class, new();
        List<StateEntry<TState>> GetHistory();
    }

    public class Store<TState> : IStore<TState> where TState : class, new() 
    {
        private TState state = new TState();
        //public T State => this.mapper.Map<T>(this.store);

        private readonly IMapper mapper;
        private readonly ILoggingService logging;
        private readonly IJsonService jsonService;

        private bool isLogEnabled;

        private readonly List<StateEntry<TState>> stateHistory = new List<StateEntry<TState>>();
        public List<Delegate> maincallbacks = new List<Delegate>();
        public Dictionary<string, List<Delegate>> callbacks = new Dictionary<string, List<Delegate>>();

        public Store(IMapper mapper, ILoggingService logging, IJsonService jsonService)
        {
            this.stateHistory.Add(new StateEntry<TState> { ActionName = null, NewState = null, UpdatedAt = DateTimeOffset.UtcNow });
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




        public IStorageSubscription Subscribe<TStore, TSlice>(IStoreAction<TStore, TState, TSlice> action, Action<TSlice> callback) where TStore : IStore<TState> where TSlice : class, new()
        {
            if (action == null)
            {
                throw new ArgumentException("Can not subscribe to Onbox Store with a null action");
            }

            var actionString = action.GetActionPath().Body.ToString();

            if (actionString == null)
            {
                maincallbacks.Add(callback);
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

        public TSlice Select<TStore, TSlice>(IStoreAction<TStore, TState, TSlice> action) where TStore : IStore<TState> where TSlice : class, new()
        {
            // Ensures that the action has the correct type
            var actionString = action?.GetActionPath().Body.ToString();
            EnsureActionType(action, actionString);

            var pathArr = actionString?.Split('.');

            // Gets the Object that will be changed
            object currentObject = GetStoreObjectByPath(this.state, pathArr);

            if (currentObject == null)
            {
                currentObject = new TSlice();
            }

            return this.mapper.Map<TSlice>(currentObject);
        }

        public void SetState<TStore, TSlice>(TState state, IStoreAction<TStore, TState, TSlice> action) where TStore : IStore<TState> where TSlice : class, new()
        {
            // Ensures that the action has the correct type
            var actionPath = action?.GetActionPath().Body.ToString();
            var actionName = action?.GetActionName();
            EnsureActionType(action, actionPath);

            // Copies the new state of the store
            var newStore = this.mapper.Map<TState>(this.state);
            var pathArr = actionPath?.Split('.');

            // Gets the Object that will be changed
            object currentObject = GetStoreObjectByPath(newStore, pathArr);

            // Maps the new state to the object that will be changed
            mapper.Map(state, currentObject);

            // Adds the state history
            var stateEntry = new StateEntry<TState>
            {
                ActionPath = actionPath,
                ActionName = actionName,
                OldState = this.state,
                NewState = newStore,
                UpdatedAt = DateTimeOffset.Now
            };
            stateHistory.Add(stateEntry);

            // Changes the store to the new state
            this.state = newStore;

            // Notifies the main subscribers
            foreach (var callback in maincallbacks)
            {
                callback.DynamicInvoke(this.state);
            }

            // Notifies the subscribers to child states
            if (actionPath != null && callbacks.ContainsKey(actionPath))
            {
                foreach (var callback in callbacks[actionPath])
                {
                    callback.DynamicInvoke(currentObject);
                }
            }

            // Logs the history
            if (isLogEnabled)
            {
                logging.Log($"*** State changed on {typeof(TState).Name} ***");
                logging.Log(jsonService.Serialize(stateEntry));
            }

            // Notifies that the state was changed
            //this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.State)));
        }





        public List<StateEntry<TState>> GetHistory()
        {
            return this.mapper.Map<List<StateEntry<TState>>>(this.stateHistory);
        }

        private object GetStoreObjectByPath(TState newStore, string[] pathArr)
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

        private void EnsureActionType<TStore, TState, TSlice>(IStoreAction<TStore, TState, TSlice> action, string actionString) 
            where TStore : IStore<TState> where TState : class, new() where TSlice : class, new()
        {
            if (actionString != null)
            {
                var propertyType = GetPropertyType(typeof(TSlice), actionString);
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
