using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;

namespace Onbox.Mvc.V1
{
    public abstract class ViewMvcBase : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// Event that gets fired when any property changes on child classes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public TitleVisibility TitleVisibility { get; set; } = TitleVisibility.HideMinimizeAndMaximize;
        public bool IsLoading { get; set; }
        public string Error { get; set; }
        public string Warning { get; set; }

        private const int GWL_STYLE = -16,
                  WS_MAXIMIZEBOX = 0x10000,
                  WS_MINIMIZEBOX = 0x20000;

        [DllImport("user32.dll")]
        extern private static int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        extern private static int SetWindowLong(IntPtr hwnd, int index, int value);

        public ViewMvcBase()
        {
            this.DataContext = this;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.SetRevitAsParent();
            this.Loaded += this.OnViewLoaded;
        }

        private void OnViewLoaded(object sender, RoutedEventArgs e)
        {
            switch (this.TitleVisibility)
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

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            e.Cancel = !this.CanCloseDialog();
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

        public virtual void OnDestroy()
        {
        }

        public async Task<bool> PerformAsync(Func<Task> func, Action<string> error = null)
        {
            this.Error = null;
            this.IsLoading = true;
            try
            {
                await func.Invoke();
                this.IsLoading = false;
                return true;
            }
            catch (Exception e)
            {
                error?.Invoke(e.Message);
                this.IsLoading = false;
                return false;
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
    }

    public enum TitleVisibility
    {
        Default = 0,
        HideMinimize = 1,
        HideMinimizeAndMaximize = 2,
        HideTitleBar = 3
    }
}
