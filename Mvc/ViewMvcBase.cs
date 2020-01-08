using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;

namespace Onbox.Mvc.V1
{
    public abstract class ViewMvcBase : Window, INotifyPropertyChanged, IViewMvc
    {
        /// <summary>
        /// Event that gets fired when any property changes on child classes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsLoading { get; set; }
        public string Error { get; set; }
        public string Warning { get; set; }
        public string Message { get; set; }

        private TitleVisibility titleVisibility = TitleVisibility.HideMinimizeAndMaximize;

        private const int GWL_STYLE = -16,
                  WS_MAXIMIZEBOX = 0x10000,
                  WS_MINIMIZEBOX = 0x20000;

        private Func<Task> onInitAsyncFunc;
        private Action<string> onInitAsyncError;
        private Action onInitComplete;

        [DllImport("user32.dll")]
        extern private static int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        extern private static int SetWindowLong(IntPtr hwnd, int index, int value);

        public ViewMvcBase()
        {
            this.DataContext = this;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ShowInTaskbar = false;
            this.SetRevitAsParent();
            this.ResizeMode = ResizeMode.CanResizeWithGrip;

            this.Loaded += this.OnViewLoaded;
            this.ContentRendered += this.OnViewRendered;
        }

        private async void OnViewLoaded(object sender, RoutedEventArgs e)
        {
            switch (this.titleVisibility)
            {
                case TitleVisibility.Default:
                    break;
                case TitleVisibility.HideMinimize:
                    this.HideMinimizeButton();
                    break;
                case TitleVisibility.HideMinimizeAndMaximize:
                    this.HideMinimizeMaximizeButton();
                    break;
                case TitleVisibility.HideTitleBar:
                    this.HideTitleBar();
                    break;
                default:
                    break;
            }

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

        /// <summary>
        /// Sets Revit as the parent window of this WPF Window. This should only be placed ON THE CONSTRUCTOR
        /// </summary>
        private void SetRevitAsParent()
        {
            System.Windows.Interop.WindowInteropHelper x = new System.Windows.Interop.WindowInteropHelper(this);
            x.Owner = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;

            IntPtr hwnd = x.Handle;
            var currentStyle = GetWindowLong(hwnd, GWL_STYLE);

            SetWindowLong(hwnd, GWL_STYLE, (currentStyle & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX));
        }

        /// <summary>
        /// Hides both Maximize and Minimize button from this WPF Window. This should be placed on Loaded event OR Rendered event, NOT ON THE CONSTRUCTOR
        /// </summary>
        protected void HideMinimizeMaximizeButton()
        {
            System.Windows.Interop.WindowInteropHelper x = new System.Windows.Interop.WindowInteropHelper(this);
            IntPtr hwnd = x.Handle;
            var currentStyle = GetWindowLong(hwnd, GWL_STYLE);

            SetWindowLong(hwnd, GWL_STYLE, (currentStyle & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX));
        }

        /// <summary>
        /// Hides both Minimize button from this WPF Window. This should be placed on Loaded event OR Rendered event, NOT ON THE CONSTRUCTOR
        /// </summary>
        protected void HideMinimizeButton()
        {
            System.Windows.Interop.WindowInteropHelper x = new System.Windows.Interop.WindowInteropHelper(this);
            IntPtr hwnd = x.Handle;
            var currentStyle = GetWindowLong(hwnd, GWL_STYLE);

            SetWindowLong(hwnd, GWL_STYLE, (currentStyle & ~WS_MINIMIZEBOX));
        }

        /// <summary>
        /// Hides the Title bat from this WPF Window. This should be placed on Loaded event OR Rendered event, NOT ON THE CONSTRUCTOR
        /// </summary>
        private void HideTitleBar()
        {
            var wChrome = new WindowChrome();
            wChrome.CaptionHeight = 0;
            WindowChrome.SetWindowChrome(this, wChrome);
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
            return Task.CompletedTask;
        }

        public virtual void OnDestroy()
        {
        }

        public virtual void OnAfterInit()
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

        public void SetTitleVisibility(TitleVisibility titleVisibility)
        {
            this.titleVisibility = titleVisibility;
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
