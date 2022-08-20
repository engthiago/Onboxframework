using System;
using System.Collections.Generic;
using System.Windows;

namespace Onbox.Mvc.VDev
{
    internal class EventCallbacks
    {
        internal Action<Window> OnActivate;
        internal Action<Window> OnDeactivate;
    }

    public class ActivateUIEffect : IDisposable
    {
        static private ActivateUIEffect singleton;

        private Dictionary<Window, EventCallbacks> events;

        public ActivateUIEffect()
        {
        }

        public static ActivateUIEffect GetSingleton()
        {
            if (singleton == null)
            {
                singleton = new ActivateUIEffect();
            }
            return singleton;
        }

        public bool Attach(Window window, Action<Window> onActivate, Action<Window> onDeactivate)
        {
            if (events.ContainsKey(window))
            {
                return false;
            }

            events.Add(window, new EventCallbacks { OnActivate = onActivate, OnDeactivate = onDeactivate });
            return true;
        }

        public bool Disattach(Window window)
        {
            if (events.ContainsKey(window))
            {
                events.Remove(window);
                return true;
            }

            return false;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            var window = sender as Window;
            if (window != null) return;
            if (events.TryGetValue(window, out var value)) 
            {
                value.OnDeactivate?.Invoke(window);
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            var window = sender as Window;
            if (window != null) return;
            if (events.TryGetValue(window, out var value))
            {
                value.OnActivate?.Invoke(window);
            }
        }

        public void Dispose()
        {
            foreach (var event_ in events)
            {
                this.Disattach(event_.Key);
            }

            events.Clear();
        }
    }
}
