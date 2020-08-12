using System;
using System.Collections.Generic;
using Onbox.Abstractions.V7;
using System.Linq.Expressions;
using System.Linq;

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

    public interface IStoreAction<TState, TSlice> where TState : class, new() where TSlice : class, new()
    {
        string GetActionName();
        Expression<Func<TState, TSlice>> GetActionPath();
    }

    public interface IStore<TState> where TState : class, new()
    {
        void EnableLogging();
        void DisableLogging();
        void SetState<TSlice>(IStoreAction<TState, TSlice> action, TSlice state) where TSlice : class, new();
        TSlice Select<TSlice>(IStoreAction<TState, TSlice> action) where TSlice : class, new();
        IStorageSubscription Subscribe<TSlice>(IStoreAction<TState, TSlice> action, Action<TSlice> callback) where TSlice : class, new();
        List<StateEntry<TState>> GetHistory();
    }

    public class Store<TState> : IStore<TState> where TState : class, new()
    {
        private TState state = new TState();

        private readonly IMapper mapper;
        private readonly ILoggingService logging;
        private readonly IJsonService jsonService;

        private bool isLogEnabled;

        private readonly List<StateEntry<TState>> stateHistory = new List<StateEntry<TState>>();
        private readonly List<Delegate> maincallbacks = new List<Delegate>();
        private readonly Dictionary<string, List<Delegate>> callbacks = new Dictionary<string, List<Delegate>>();

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




        public void SetState<TSlice>(IStoreAction<TState, TSlice> action, TSlice slice) where TSlice : class, new()
        {
            // Ensures that the action is valid
            EnsureValidAction(action);

            var actionPath = action?.GetActionPath().Body.ToString();
            var actionName = action?.GetActionName();

            // Copies the new state of the store
            var newState = this.mapper.Clone(this.state);

            // Gets the Object that will be changed
            object newSlice = GetStoreObjectByPath(newState, action);

            // Maps the new state to the object that will be changed
            mapper.Map(slice, newSlice);

            // Adds the state history
            var stateEntry = new StateEntry<TState>
            {
                ActionPath = actionPath,
                ActionName = actionName,
                OldState = this.state,
                NewState = newState,
                UpdatedAt = DateTimeOffset.Now
            };
            stateHistory.Add(stateEntry);

            // Changes the store to the new state
            this.state = newState;

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
                    callback.DynamicInvoke(newSlice);
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

        public TSlice Select<TSlice>(IStoreAction<TState, TSlice> action) where TSlice : class, new()
        {
            EnsureValidAction(action);

            // Gets the Object that will be changed
            object currentObject = GetStoreObjectByPath(this.state, action);

            return this.mapper.Clone<TSlice>(currentObject);
        }

        public IStorageSubscription Subscribe<TSlice>(IStoreAction<TState, TSlice> action, Action<TSlice> callback) where TSlice : class, new()
        {
            EnsureValidAction(action);

            var actionPath = this.GetPath(action);

            if (string.IsNullOrWhiteSpace(actionPath))
            {
                maincallbacks.Add(callback);
                return new StorageSubscription(maincallbacks, callback);
            }

            if (callbacks.ContainsKey(actionPath))
            {
                var list = callbacks[actionPath];
                list.Add(callback);
                return new StorageSubscription(list, callback);
            }
            else
            {
                var list = new List<Delegate>();
                list.Add(callback);
                callbacks.Add(actionPath, list);
                return new StorageSubscription(list, callback);
            }
        }

        public List<StateEntry<TState>> GetHistory()
        {
            return this.mapper.Clone(this.stateHistory);
        }



        private string GetPath<TSlice>(IStoreAction<TState, TSlice> action) where TSlice : class, new()
        {
            IEnumerable<string> pathArray = GetPathArray(action);
            var path = string.Join(".", pathArray);

            return path;
        }

        private IEnumerable<string> GetPathArray<TSlice>(IStoreAction<TState, TSlice> action) where TSlice : class, new()
        {
            var expression = action.GetActionPath();
            var pathArray = expression.Body.ToString().Split('.').Skip(1);
            return pathArray;
        }

        private object GetStoreObjectByPath<TSlice>(TState newState, IStoreAction<TState, TSlice> action) where TSlice : class, new()
        {
            object currentObject = newState;
            var pathArray = this.GetPathArray(action);

            foreach (var pathItem in pathArray)
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

            return currentObject;
        }

        private void EnsureValidAction<TState, TSlice>(IStoreAction<TState, TSlice> action)
             where TState : class, new() where TSlice : class, new()
        {
            if (action == null)
            {
                throw new ArgumentException($"Action can not be null");
            }
        }

        private Type GetPropertyType(Type type, string path)
        {
            var currentType = type;
            var pathArr = path?.Split('.').Skip(1);

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
