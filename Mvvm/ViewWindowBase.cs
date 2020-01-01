using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Shell;

namespace Onbox.Mvvm.V1
{
    public class ViewWindowBase : Window, IViewWindow
    {
        public TitleVisibility TitleVisibility { get; set; } = TitleVisibility.HideMinimizeAndMaximize;
        public Func<bool> CanCloseDialog { get ; set ; }
        public Action OnInit { get; set; }
        public Action OnDestroy { get; set; }

        private const int GWL_STYLE = -16,
                  WS_MAXIMIZEBOX = 0x10000,
                  WS_MINIMIZEBOX = 0x20000;

        [DllImport("user32.dll")]
        extern private static int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        extern private static int SetWindowLong(IntPtr hwnd, int index, int value);

        public ViewWindowBase()
        {
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

            OnInit?.Invoke();
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
            if (owner is Window window)
            {
                this.Owner = window;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (CanCloseDialog != null)
            {
                e.Cancel = !this.CanCloseDialog.Invoke();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            OnDestroy?.Invoke();
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
