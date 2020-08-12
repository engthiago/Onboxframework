using System;
using System.Collections.Generic;
using Onbox.Abstractions.V7;
using System.Linq.Expressions;
using System.Linq;

namespace Onbox.Store.V7
{
    /// <summary>
    /// A snapshot of the state in a give point in time
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public class StateEntry<TState> where TState : class, new()
    {
        /// <summary>
        /// When the state was updated
        /// </summary>
        public DateTimeOffset UpdatedAt { get; set; }
        /// <summary>
        /// The state before this modification
        /// </summary>
        public TState OldState { get; set; }
        /// <summary>
        /// The state after this modification
        /// </summary>
        public TState NewState { get; set; }
        /// <summary>
        /// The action responsible for this modification
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// The action path responsible this modification
        /// </summary>
        public string ActionPath { get; set; }
    }

    /// <summary>
    /// A copy of the subscription for Store Actions
    /// </summary>
    public interface IStorageSubscription
    {
        /// <summary>
        /// Unsubscribe and cleanup
        /// </summary>
        void Unsubscribe();
    }

    /// <summary>
    /// A copy of the subscription for Store Actions
    /// </summary>
    public class StorageSubscription : IStorageSubscription
    {
        private readonly List<Delegate> callbacks;
        private Delegate target;

        internal StorageSubscription(List<Delegate> callbacks, Delegate target)
        {
            this.callbacks = callbacks;
            this.target = target;
        }

        /// <summary>
        /// Unsubscribe and cleanup
        /// </summary>
        public void Unsubscribe()
        {
            Console.WriteLine("Unsubscribing...");
            callbacks.Remove(target);
            target = null;
        }
    }

    /// <summary>
    /// An action responsible for dispatching events and accessing a particular slice of the store
    /// </summary>
    /// <typeparam name="TState">The type of the global state</typeparam>
    /// <typeparam name="TSlice">The type of slice</typeparam>
    public interface IStoreAction<TState, TSlice> where TState : class, new() where TSlice : class, new()
    {
        /// <summary>
        /// The name that will be shown on the state history
        /// </summary>
        /// <returns></returns>
        string GetActionName();

        /// <summary>
        /// The expression responsible for accessing the slice of the state
        /// </summary>
        /// <returns></returns>
        Expression<Func<TState, TSlice>> GetActionPath();
    }

    /// <summary>
    /// The global state management
    /// </summary>
    /// <typeparam name="TState">The type of the global state</typeparam>
    public interface IStore<TState> where TState : class, new()
    {
        /// <summary>
        /// Enable logging of the state when actions are dispatched
        /// </summary>
        void EnableLogging();
        /// <summary>
        /// Disables logging of the state when actions are dispatched
        /// </summary>
        void DisableLogging();
        /// <summary>
        /// Sets a specific slice of the global state
        /// </summary>
        /// <typeparam name="TSlice">The type of the slice that will be changed</typeparam>
        /// <param name="action">The action responsible for the slice</param>
        /// <param name="state">The new state to be dispatched</param>
        void SetState<TSlice>(IStoreAction<TState, TSlice> action, TSlice state) where TSlice : class, new();
        /// <summary>
        /// Selects a specific slice of the global state
        /// </summary>
        /// <typeparam name="TSlice">The type of the slice</typeparam>
        /// <param name="action">The action responsible for the slice</param>
        TSlice Select<TSlice>(IStoreAction<TState, TSlice> action) where TSlice : class, new();
        /// <summary>
        /// Subscribe to a specifc slice of the global state
        /// </summary>
        /// <typeparam name="TSlice">The type of the slice</typeparam>
        /// <param name="action">The action responsible for the slice</param>
        /// <param name="callback">The callback perfomerd when the slice is changed</param>
        IStorageSubscription Subscribe<TSlice>(IStoreAction<TState, TSlice> action, Action<TSlice> callback) where TSlice : class, new();
        /// <summary>
        /// Gets the complete history of the global state
        /// </summary>
        List<StateEntry<TState>> GetHistory();
    }

    /// <summary>
    /// The global state management
    /// </summary>
    /// <typeparam name="TState">The type of the global state</typeparam>
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

        /// <summary>
        /// The constructor for this implementation of state management
        /// </summary>
        public Store(IMapper mapper, ILoggingService logging, IJsonService jsonService)
        {
            this.stateHistory.Add(new StateEntry<TState> { ActionName = null, NewState = null, UpdatedAt = DateTimeOffset.UtcNow });
            this.mapper = mapper;
            this.logging = logging;
            this.jsonService = jsonService;
        }

        /// <summary>
        /// Enable logging of the state when actions are dispatched
        /// </summary>
        public void EnableLogging()
        {
            isLogEnabled = true;
        }

        /// <summary>
        /// Disables logging of the state when actions are dispatched
        /// </summary>
        public void DisableLogging()
        {
            isLogEnabled = false;
        }

        /// <summary>
        /// Sets a specific slice of the global state
        /// </summary>
        /// <typeparam name="TSlice">The type of the slice that will be changed</typeparam>
        /// <param name="action">The action responsible for the slice</param>
        /// <param name="slice">The new state to be dispatched</param>
        public void SetState<TSlice>(IStoreAction<TState, TSlice> action, TSlice slice) where TSlice : class, new()
        {
            // Ensures that the action is valid
            EnsureValidAction(action);

            var actionPath = this.GetPath(action);
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

        /// <summary>
        /// Selects a specific slice of the global state
        /// </summary>
        /// <typeparam name="TSlice">The type of the slice</typeparam>
        /// <param name="action">The action responsible for the slice</param>
        /// <returns></returns>
        public TSlice Select<TSlice>(IStoreAction<TState, TSlice> action) where TSlice : class, new()
        {
            EnsureValidAction(action);

            // Gets the Object that will be changed
            object currentObject = GetStoreObjectByPath(this.state, action);

            return this.mapper.Clone<TSlice>(currentObject);
        }

        /// <summary>
        /// Subscribe to a specifc slice of the global state
        /// </summary>
        /// <typeparam name="TSlice">The type of the slice</typeparam>
        /// <param name="action">The action responsible for the slice</param>
        /// <param name="callback">The callback perfomerd when the slice is changed</param>
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

        /// <summary>
        /// Gets the complete history of the global state
        /// </summary>
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

        private void EnsureValidAction<TSlice>(IStoreAction<TState, TSlice> action) where TSlice : class, new()
        {
            if (action == null)
            {
                throw new ArgumentException($"Action can not be null");
            }
        }

    }
}
