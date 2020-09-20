using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace Onbox.Mvc.V7
{
    /// <summary>
    /// Provides base functionality for WPF MVC Pattern
    /// </summary>
    public abstract class MvcViewBase : Window, INotifyPropertyChanged, IMvcView, IMvcLifecycleView
    {
        /// <summary>
        /// Event that gets fired when any property changes on child classes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsLoading { get; set; }
        public string Error { get; set; }
        public string Warning { get; set; }
        public string Message { get; set; }

        public bool CanRetryOnError { get; set; }
        public bool CanRetryOnWarning { get; set; }


        private Func<Task> onInitAsyncFunc;
        private Action<string> onInitAsyncError;
        private Action onInitComplete;

        public MvcViewBase()
        {
            this.DataContext = this;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ShowInTaskbar = false;
            this.ResizeMode = ResizeMode.CanResizeWithGrip;

            this.Loaded += this.OnViewLoaded;
            this.ContentRendered += this.OnViewRendered;
            this.Deactivated += this.OnDeactivated;
            this.Activated += this.OnActivated;
        }

        private void OnActivated(object sender, EventArgs e)
        {
            if (this.Content is UIElement uIElement)
            {
                uIElement.Opacity = 1;
            }
        }

        private void OnDeactivated(object sender, EventArgs e)
        {
            if (this.Content is UIElement uIElement)
            {
                uIElement.Opacity = 0.5;
            }
        }

        private async void OnViewLoaded(object sender, RoutedEventArgs e)
        {
            OnInit();

            await OnInitAsync();

            if (onInitAsyncFunc != null)
            {
                Error = null;
                IsLoading = true;
                try
                {
                    await onInitAsyncFunc?.Invoke();
                }
                catch (Exception ex)
                {
                    Error = ex.Message;
                    this.onInitAsyncError?.Invoke(ex.Message);
                }
                finally
                {
                    IsLoading = false;
                    this.onInitComplete?.Invoke();
                }
            }
        }

        private void OnViewRendered(object sender, EventArgs e)
        {
            OnAfterInit();
        }

        public void SetOwner(object owner)
        {
            if (owner is Window ownerWindow)
            {
                this.Owner = ownerWindow;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (DialogResult != true)
            {
                e.Cancel = !this.CanCloseDialog();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            OnDestroy();
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnInit()
        {

        }

        public virtual Task OnInitAsync()
        {
            return Task.Delay(0);
        }

        public virtual void OnDestroy()
        {
        }

        public virtual void OnAfterInit()
        {
        }

        public virtual void OnErrorRetry()
        {
        }

        public virtual void OnWarningRetry()
        {
        }

        public async Task<bool> PerformAsync(Func<Task> func, Action<Exception> onError = null, Action onComplete = null)
        {
            this.Error = null;
            this.Warning = null;
            this.IsLoading = true;
            try
            {
                await func?.Invoke();
                return true;
            }
            catch (Exception e)
            {
                onError?.Invoke(e);
                return false;
            }
            finally
            {
                onComplete?.Invoke();
                this.IsLoading = false;
            }
        }

        /// <summary>
        /// Refresh a single property to UI
        /// </summary>
        /// <param name="propertyName"></param>
        public void RefreshProperty(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Refresh all properties to UI
        /// </summary>
        public void RefreshAllProperties()
        {
            System.Reflection.PropertyInfo[] properties = this.GetType().GetProperties();
            foreach (var property in properties)
            {
                RefreshProperty(property.Name);
            }
        }

        public void SetTitle(string title)
        {
            this.Title = !string.IsNullOrWhiteSpace(title) ? title : "";
        }



        public void RunOnInitFunc(Func<Task> func, Action<string> error = null, Action complete = null)
        {
            this.onInitAsyncFunc = func;
            this.onInitAsyncError = error;
            this.onInitComplete = complete;
        }
    }

    public enum TitleVisibility
    {
        Default = 0,
        HideMinimize = 1,
        HideMinimizeAndMaximize = 2,
        HideTitleBar = 3
    }
}
