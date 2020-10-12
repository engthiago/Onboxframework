using Onbox.Mvc.Abstractions.VDev;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Onbox.Mvc.VDev
{
    public abstract class MvcComponentBase : Page, INotifyPropertyChanged, IMvcComponent, IMvcLifecycleComponent
    {
        public bool IsLoading { get; set; }
        public string Error { get; set; }
        public string Warning { get; set; }
        public string Message { get; set; }

        public bool CanRetryOnError { get; set; }
        public bool CanRetryOnWarning { get; set; }

        public Func<Task> onInitAsyncFunc { get; set; }
        public Action<string> onInitAsyncError { get; set; }
        public Action onInitComplete { get; set; }

        /// <summary>
        /// Event that gets fired when any property changes on child classes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public MvcComponentBase()
        {
            this.DataContext = this;


            this.Loaded += this.OnComponentLoaded;
            this.Unloaded += this.OnComponentUnloaded;
        }

        private void OnComponentUnloaded(object sender, RoutedEventArgs e)
        {
            OnDestroy();
        }

        private async void OnComponentLoaded(object sender, RoutedEventArgs e)
        {
            OnInit();

            await OnInitAsync();

            if (this.onInitAsyncFunc != null)
            {
                this.Error = null;
                this.IsLoading = true;
                try
                {
                    await this.onInitAsyncFunc?.Invoke();
                }
                catch (Exception ex)
                {
                    this.Error = ex.Message;
                    this.onInitAsyncError?.Invoke(ex.Message);
                }
                finally
                {
                    this.IsLoading = false;
                    this.onInitComplete?.Invoke();
                }
            }
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

        public virtual void OnErrorRetry()
        {
        }

        public virtual void OnWarningRetry()
        {
        }

        public void RunOnInitFunc(Func<Task> func, Action<string> error = null, Action complete = null)
        {
            this.onInitAsyncFunc = func;
            this.onInitAsyncError = error;
            this.onInitComplete = complete;
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
    }
}