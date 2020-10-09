using Onbox.Abstractions.V7;
using Onbox.Mvc.Abstractions.V7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Onbox.Mvc.V7
{
    /// <summary>
    /// Provides ways to Navigate between components and get notified when a <see cref="NavigatorComponent"/> recieves a new component to navigate to
    /// </summary>
    public interface INavigator
    {
        /// <summary>
        /// Hooks up a <see cref="NavigatorComponent"/> and its parent to the Navigation System. It will automatically be notified of navigation changes and control its life cycle and cleaning up after unloading.
        /// </summary>
        /// <typeparam name="TParent">The Parent to the Navigator Component, generally a View or a MvcComponent</typeparam>
        /// <typeparam name="TNavComponent">the Navigator Component itself</typeparam>
        void Attach<TParent, TNavComponent>(TParent parentComponent, TNavComponent component)
            where TParent : IMvcLifecycleComponent
            where TNavComponent : NavigatorComponent;

        /// <summary>
        /// Gets the current Component associated to a specific <see cref="NavigatorComponent"/>
        /// </summary>
        IMvcComponent GetCurrentComponent<TParent>(string componentName = "Navigator") 
            where TParent : IMvcLifecycleComponent;
        /// <summary>
        /// Clears a specific <see cref="NavigatorComponent"/>, setting its child to null
        /// </summary>
        void ClearNavigation<TParent>(string componentName = "Navigator") 
            where TParent : IMvcLifecycleComponent;
        /// <summary>
        /// Navigates a specific <see cref="NavigatorComponent"/> to a specific <see cref="IMvcComponent"/>
        /// </summary>
        void Navigate<TParent, TComponent>(string componentName = "Navigator")
            where TParent : IMvcLifecycleComponent
            where TComponent : IMvcComponent;
        /// <summary>
        /// Gets notified when the default <see cref="NavigatorComponent"/> (x:Name == 'Navigator') of a specific Parent changes. Remember to call Unsubscribe to avoid memory leaks.
        /// </summary>
        INavigatorSubscription Subscribe<TParent>(Action<IMvcComponent> action) 
            where TParent : IMvcLifecycleComponent;
        /// <summary>
        /// Gets notified when a <see cref="NavigatorComponent"/> of a specific Parent changes. Remember to call Unsubscribe to avoid memory leaks.
        /// </summary>
        INavigatorSubscription Subscribe<TParent>(string componentName, Action<IMvcComponent> action) 
            where TParent : IMvcLifecycleComponent;
    }

    /// <summary>
    /// Provides ways to Navigate between components and get notified when a <see cref="NavigatorComponent"/> recieves a new component to navigate to
    /// </summary>
    public class Navigator : INavigator
    {
        private readonly Dictionary<string, Dictionary<string, Type>> componentDictionary = new Dictionary<string, Dictionary<string, Type>>();
        private readonly Dictionary<string, Dictionary<string, List<Action<IMvcComponent>>>> actionDictionary = new Dictionary<string, Dictionary<string, List<Action<IMvcComponent>>>>();
        private readonly IContainerResolver container;

        private readonly int maxNavigatorHierarchy = 16;

        /// <summary>
        /// Provides ways to Navigate between components and get notified when a <see cref="NavigatorComponent"/> recieves a new component to navigate to
        /// </summary>
        public Navigator(IContainerResolver container)
        {
            this.container = container;
        }

        /// <summary>
        /// Hooks up a <see cref="NavigatorComponent"/> and its parent to the Navigation System. It will automatically be notified of navigation changes and control its life cycle and cleaning up after unloading.
        /// </summary>
        /// <typeparam name="TParent">The Parent to the Navigator Component, generally a View or a MvcComponent</typeparam>
        /// <typeparam name="TNavComponent">the Navigator Component itself</typeparam>
        public void Attach<TParent, TNavComponent>(TParent parentComponent, TNavComponent component) where TParent : IMvcLifecycleComponent where TNavComponent : NavigatorComponent
        {
            if (parentComponent == null)
            {
                throw new ArgumentException("Can not attach Navigator into a null View");
            }

            if (component == null)
            {
                throw new ArgumentException("Can not attach Navigator into a null Navigator Component");
            }

            if (string.IsNullOrWhiteSpace(component.Name))
            {
                throw new ArgumentException("Can not attach Navigator into Navigator Component with no name, please set x:Name on Xaml of your Navigator Component");
            }

            component.Loaded += this.Component_Loaded;
            component.Unloaded += this.Component_Unloaded;

            var parentIdentifier = GetParentIdentifier(parentComponent);

            if (componentDictionary.ContainsKey(parentIdentifier))
            {
                var navigators = componentDictionary[parentIdentifier];
                if (navigators == null)
                {
                    navigators = new Dictionary<string, Type>();
                }
            }
            else
            {
                componentDictionary[parentIdentifier] = new Dictionary<string, Type>();
            }

            var subs = Subscribe(parentIdentifier, component.Name, (comp) =>
            {
                if (!ReachedMaxNavigatorHierarchy(component))
                {
                    component.CurrentComponent = comp;
                }
            });
            component.NavigatorSubscription = subs;
        }

        private void Component_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is NavigatorComponent navigatorComponent)
            {
                var parent = VisualTreeHelpers.GetParentMvcComponent(navigatorComponent);
                if (parent != null)
                {
                    var parentIdentifier = GetParentIdentifier(parent);
                    var current = GetCurrentComponent(parentIdentifier, navigatorComponent.Name);
                    if (!ReachedMaxNavigatorHierarchy(navigatorComponent))
                    {
                        navigatorComponent.CurrentComponent = current;
                    }
                }
            }
        }

        private bool ReachedMaxNavigatorHierarchy(NavigatorComponent navigator)
        {
            return VisualTreeHelpers.GetCircularParentHierarchyCount(navigator, maxNavigatorHierarchy);
        }

        private void Component_Unloaded(object sender, RoutedEventArgs e)
        {
            if (sender is NavigatorComponent navigatorComponent)
            {
                navigatorComponent.NavigatorSubscription?.UnSubscribe();
            }
        }

        private string GetParentIdentifier(IMvcLifecycleComponent parent)
        {
            var type = parent.GetType();
            return type.FullName;
        }

        private string GetParentIdentifier<TParent>() where TParent : IMvcLifecycleComponent
        {
            var type = typeof(TParent);
            var identitifier = type.FullName;
            return identitifier;
        }

        /// <summary>
        /// Gets the current Component associated to a specific <see cref="NavigatorComponent"/>
        /// </summary>
        public IMvcComponent GetCurrentComponent<TParent>(string componentName = "Navigator") where TParent : IMvcLifecycleComponent
        {
            string identitifier = GetParentIdentifier<TParent>();
            return GetCurrentComponent(identitifier, componentName);
        }

        private MvcComponentBase GetCurrentComponent(string parentIdentifier, string componentName)
        {
            if (componentDictionary.ContainsKey(parentIdentifier))
            {
                var navigators = componentDictionary[parentIdentifier];
                if (navigators != null && navigators.ContainsKey(componentName))
                {
                    var pageType = navigators[componentName];
                    return InstantiateComponent(pageType);
                }
            }

            return null;
        }

        private MvcComponentBase InstantiateComponent(Type componentType)
        {
            if (componentType != null)
            {
                return this.container.Resolve(componentType) as MvcComponentBase;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Navigates a specific <see cref="NavigatorComponent"/> to a specific <see cref="IMvcComponent"/>
        /// </summary>
        public void Navigate<TParent, TComponent>(string componentName = "Navigator") where TParent : IMvcLifecycleComponent where TComponent : IMvcComponent
        {
            var parentIdentifier = GetParentIdentifier<TParent>();
            var componentType = typeof(TComponent);
            Navigate(parentIdentifier, componentName, componentType);
        }

        /// <summary>
        /// Clears a specific <see cref="NavigatorComponent"/>, setting its child to null
        /// </summary>
        public void ClearNavigation<TParent>(string componentName = "Navigator") where TParent : IMvcLifecycleComponent
        {
            var parentIdentifier = GetParentIdentifier<TParent>();
            Navigate(parentIdentifier, componentName, null);
        }

        private void Navigate(string parentIdentifier, string componentName, Type componentType)
        {
            if (!componentDictionary.ContainsKey(parentIdentifier))
            {
                componentDictionary[parentIdentifier] = new Dictionary<string, Type>();
            }

            var navigators = componentDictionary[parentIdentifier];
            if (navigators == null)
            {
                navigators = new Dictionary<string, Type>();
                componentDictionary[componentName] = navigators;
            }

            navigators[componentName] = componentType;
            NotifySubscribers(parentIdentifier, componentName, componentType);
        }

        private void NotifySubscribers(string parentIdentifier, string componentName, Type componentType)
        {
            if (actionDictionary.ContainsKey(parentIdentifier))
            {
                var navigatorsActions = actionDictionary[parentIdentifier];

                if (navigatorsActions == null)
                {
                    return;
                }

                if (navigatorsActions.ContainsKey(componentName))
                {
                    var actions = navigatorsActions[componentName];
                    if (actions != null)
                    {
                        foreach (var action in actions)
                        {
                            var page = InstantiateComponent(componentType);
                            action.Invoke(page);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Gets notified when the default <see cref="NavigatorComponent"/> (x:Name == 'Navigator') of a specific Parent changes. Remember to call Unsubscribe to avoid memory leaks.
        /// </summary>
        public INavigatorSubscription Subscribe<TParent>(Action<IMvcComponent> action) where TParent : IMvcLifecycleComponent
        {
            var componentName = "Navigator";
            return Subscribe<TParent>(componentName, action);
        }
        /// <summary>
        /// Gets notified when a <see cref="NavigatorComponent"/> of a specific Parent changes. Remember to call Unsubscribe to avoid memory leaks.
        /// </summary>
        public INavigatorSubscription Subscribe<TParent>(string componentName, Action<IMvcComponent> action) where TParent : IMvcLifecycleComponent
        {
            var parentIdentifier = GetParentIdentifier<TParent>();
            return Subscribe(parentIdentifier, componentName, action);
        }

        private INavigatorSubscription Subscribe(string parentIdentifier, string componentName, Action<IMvcComponent> action)
        {
            if (string.IsNullOrWhiteSpace(componentName))
            {
                componentName = "Navigator";
            }

            var parentNavigatorActions = new Dictionary<string, List<Action<IMvcComponent>>>();
            var navigatorActions = new List<Action<IMvcComponent>>();

            if (actionDictionary.ContainsKey(parentIdentifier))
            {
                parentNavigatorActions = actionDictionary[parentIdentifier];
            }
            else
            {
                actionDictionary[parentIdentifier] = parentNavigatorActions;
            }

            if (parentNavigatorActions.ContainsKey(componentName))
            {
                navigatorActions = parentNavigatorActions[componentName];
            }
            else
            {
                parentNavigatorActions[componentName] = navigatorActions;
            }

            navigatorActions.Add(action);
            var pageSubs = new NavigatorSubscription(action, navigatorActions);
            return pageSubs;
        }
    }

    /// <summary>
    /// Subscription to a <see cref="INavigator"/>
    /// </summary>
    public interface INavigatorSubscription
    {
        /// <summary>
        /// Unsubscribes from the callback and cleans up the resources
        /// </summary>
        void UnSubscribe();
    }

    /// <summary>
    /// Subscription to a <see cref="INavigator"/>
    /// </summary>
    public class NavigatorSubscription : INavigatorSubscription
    {
        private Action<IMvcComponent> action;
        private List<Action<IMvcComponent>> actions;

        /// <summary>
        /// Subscription to a <see cref="INavigator"/>
        /// </summary>
        public NavigatorSubscription(Action<IMvcComponent> action, List<Action<IMvcComponent>> actions)
        {
            this.action = action;
            this.actions = actions;
        }

        /// <summary>
        /// Unsubscribes from the callback and cleans up the resources
        /// </summary>
        public void UnSubscribe()
        {
            this.actions.Remove(action);
            this.action = null;
            if (!actions.Any())
            {
                actions = null;
            }
        }
    }
}
