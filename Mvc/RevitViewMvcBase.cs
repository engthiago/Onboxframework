using Onbox.Revit.V7;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Shell;

namespace Onbox.Mvc.V7
{
    public abstract class RevitViewMvcBase : ViewMvcBase, IRevitViewMvcBase
    {
        private TitleVisibility titleVisibility = TitleVisibility.HideMinimizeAndMaximize;

        private IntPtr mainWindowHandle;

        private const int GWL_STYLE = -16,
                  WS_MAXIMIZEBOX = 0x10000,
                  WS_MINIMIZEBOX = 0x20000;

        [DllImport("user32.dll")]
        extern private static int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        extern private static int SetWindowLong(IntPtr hwnd, int index, int value);

        public RevitViewMvcBase(IRevitUIApp revitUIApp)
        {
            this.mainWindowHandle = revitUIApp.GetRevitWindowHandle();

            this.SetRevitAsParent();
            this.Loaded += this.RevitViewMvcBase_Loaded;
        }

        private void RevitViewMvcBase_Loaded(object sender, RoutedEventArgs e)
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
        }

        /// <summary>
        /// Sets Revit as the parent window of this WPF Window. This should only be placed ON THE CONSTRUCTOR
        /// </summary>
        private void SetRevitAsParent()
        {
            GetWindowHandle();

            var currentStyle = GetWindowLong(this.mainWindowHandle, GWL_STYLE);
            SetWindowLong(this.mainWindowHandle, GWL_STYLE, (currentStyle & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX));
        }

        private void GetWindowHandle()
        {
            if (this.mainWindowHandle == null)
            {
                System.Windows.Interop.WindowInteropHelper x = new System.Windows.Interop.WindowInteropHelper(this);
                x.Owner = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
                this.mainWindowHandle = x.Handle;
            }
        }

        /// <summary>
        /// Hides both Maximize and Minimize button from this WPF Window. This should be placed on Loaded event OR Rendered event, NOT ON THE CONSTRUCTOR
        /// </summary>
        protected void HideMinimizeMaximizeButton()
        {
            GetWindowHandle();

            var currentStyle = GetWindowLong(this.mainWindowHandle, GWL_STYLE);
            SetWindowLong(this.mainWindowHandle, GWL_STYLE, (currentStyle & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX));
        }

        /// <summary>
        /// Hides both Minimize button from this WPF Window. This should be placed on Loaded event OR Rendered event, NOT ON THE CONSTRUCTOR
        /// </summary>
        protected void HideMinimizeButton()
        {
            GetWindowHandle();

            var currentStyle = GetWindowLong(this.mainWindowHandle, GWL_STYLE);
            SetWindowLong(this.mainWindowHandle, GWL_STYLE, (currentStyle & ~WS_MINIMIZEBOX));
        }

        public void SetTitleVisibility(TitleVisibility titleVisibility)
        {
            this.titleVisibility = titleVisibility;
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
    }
}
