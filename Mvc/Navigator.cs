using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Onbox.Mvc.V7
{
    public interface INavigator
    {
        INavigatorSubscription Attach<TView, TNavComponent>(TView window, TNavComponent component)
            where TView : Window
            where TNavComponent : NavigatorComponent;
        Page GetCurrentPage<TView>(string componentName = "Navigator") where TView : Window;
        void ClearNavigation<TView>(string componentName = "Navigator") where TView : Window;
        void Navigate<TView, TPage>(string componentName = "Navigator")
            where TView : Window
            where TPage : Page, new();

        INavigatorSubscription Subscribe<TView>(Action<Page> action) where TView : Window;
        INavigatorSubscription Subscribe<TView>(string componentName, Action<Page> action) where TView : Window;
    }

    public class Navigator : INavigator
    {
        public readonly Dictionary<string, Dictionary<string, Type>> pageDictionary = new Dictionary<string, Dictionary<string, Type>>();
        public readonly Dictionary<string, Dictionary<string, List<Action<Page>>>> actionDictionary = new Dictionary<string, Dictionary<string, List<Action<Page>>>>();

        public INavigatorSubscription Attach<TView, TNavComponent>(TView window, TNavComponent component) where TView : Window where TNavComponent : NavigatorComponent
        {
            if (window == null)
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

            var windowIdentifier = GetWindowIdentifier(window);

            if (pageDictionary.ContainsKey(windowIdentifier))
            {
                var navigators = pageDictionary[windowIdentifier];
                if (navigators == null)
                {
                    navigators = new Dictionary<string, Type>();
                }
            }
            else
            {
                pageDictionary[windowIdentifier] = new Dictionary<string, Type>();
            }

            var subs = Subscribe(windowIdentifier, component.Name, (page) => component.Content = page);
            component.NavigatorSubscription = subs;
            return subs;
        }

        private void Component_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is NavigatorComponent navigatorComponent)
            {
                // Go up until get a window
                if (navigatorComponent.Parent is Window window)
                {
                    var windowIdentifier = GetWindowIdentifier(window);
                    window.Dispatcher.Invoke(() => navigatorComponent.Content = GetCurrentPage(windowIdentifier, navigatorComponent.Name));
                }
            }
        }

        private void Component_Unloaded(object sender, RoutedEventArgs e)
        {
            if (sender is NavigatorComponent navigatorComponent)
            {
                navigatorComponent.NavigatorSubscription?.UnSubscribe();
            }
        }

        private string GetWindowIdentifier(Window window)
        {
            var type = window.GetType();
            return type.FullName;
        }

        private string GetWindowIdentifier<TView>() where TView : Window
        {
            var type = typeof(TView);
            var identitifier = type.FullName;
            return identitifier;
        }

        public Page GetCurrentPage<TView>(string componentName = "Navigator") where TView : Window
        {
            string identitifier = GetWindowIdentifier<TView>();
            return GetCurrentPage(identitifier, componentName);
        }

        private Page GetCurrentPage(string windowIdentifier, string componentName)
        {
            if (pageDictionary.ContainsKey(windowIdentifier))
            {
                var navigators = pageDictionary[windowIdentifier];
                if (navigators != null && navigators.ContainsKey(componentName))
                {
                    var pageType = navigators[componentName];
                    return InstantiatePage(pageType);
                }
            }

            return null;
        }

        private Page InstantiatePage(Type pageType)
        {
            if (pageType != null)
            {
                return Activator.CreateInstance(pageType) as Page;
            }
            else
            {
                return null;
            }
        }

        public void Navigate<TView, TPage>(string componentName = "Navigator") where TView : Window where TPage : Page, new()
        {
            var windowIdentifier = GetWindowIdentifier<TView>();
            var pageType = typeof(TPage);
            Navigate(windowIdentifier, componentName, pageType);
        }

        public void ClearNavigation<TView>(string componentName = "Navigator") where TView : Window
        {
            var windowIdentifier = GetWindowIdentifier<TView>();
            Navigate(windowIdentifier, componentName, null);
        }

        private void Navigate(string windowIdentifier, string componentName, Type pageType)
        {
            if (!pageDictionary.ContainsKey(windowIdentifier))
            {
                pageDictionary[windowIdentifier] = new Dictionary<string, Type>();
            }

            var navigators = pageDictionary[windowIdentifier];
            if (navigators == null)
            {
                navigators = new Dictionary<string, Type>();
                pageDictionary[componentName] = navigators;
            }

            navigators[componentName] = pageType;
            NotifySubscribers(windowIdentifier, componentName, pageType);
        }

        private void NotifySubscribers(string windowIdentifier, string componentName, Type pageType)
        {
            if (actionDictionary.ContainsKey(windowIdentifier))
            {
                var navigatorsActions = actionDictionary[windowIdentifier];

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
                            var page = InstantiatePage(pageType);
                            action.Invoke(page);
                        }
                    }
                }
            }
        }

        public INavigatorSubscription Subscribe<TView>(Action<Page> action) where TView : Window
        {
            var componentName = "Navigator";
            return Subscribe<TView>(componentName, action);
        }

        public INavigatorSubscription Subscribe<TView>(string componentName, Action<Page> action) where TView : Window
        {
            var windowIdentifier = GetWindowIdentifier<TView>();
            return Subscribe(windowIdentifier, componentName, action);
        }

        private INavigatorSubscription Subscribe(string windowIdentifier, string componentName, Action<Page> action)
        {
            if (string.IsNullOrWhiteSpace(componentName))
            {
                componentName = "Navigator";
            }

            var windowNavigatorActions = new Dictionary<string, List<Action<Page>>>();
            var navigatorActions = new List<Action<Page>>();

            if (actionDictionary.ContainsKey(windowIdentifier))
            {
                windowNavigatorActions = actionDictionary[windowIdentifier];
            }
            else
            {
                actionDictionary[windowIdentifier] = windowNavigatorActions;
            }

            if (windowNavigatorActions.ContainsKey(componentName))
            {
                navigatorActions = windowNavigatorActions[componentName];
            }
            else
            {
                windowNavigatorActions[componentName] = navigatorActions;
            }

            navigatorActions.Add(action);
            var pageSubs = new NavigatorSubscription(action, navigatorActions);
            return pageSubs;
        }
    }

    public interface INavigatorSubscription
    {
        void UnSubscribe();
    }

    public class NavigatorSubscription : INavigatorSubscription
    {
        private Action<Page> action;
        private List<Action<Page>> actions;

        public NavigatorSubscription(Action<Page> action, List<Action<Page>> actions)
        {
            this.action = action;
            this.actions = actions;
        }

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
