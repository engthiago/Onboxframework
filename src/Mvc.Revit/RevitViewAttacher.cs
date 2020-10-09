using Onbox.Mvc.V7;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Shell;

namespace Onbox.Mvc.Revit.V7
{
    public class RevitViewAttacher
    {
        private readonly TitleVisibility titleVisibility = TitleVisibility.HideMinimizeAndMaximize;
        private readonly Window window;
        private readonly IntPtr hwnd;
        private const int GWL_STYLE = -16,
                  WS_MAXIMIZEBOX = 0x10000,
                  WS_MINIMIZEBOX = 0x20000;

        [DllImport("user32.dll")]
        extern private static int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        extern private static int SetWindowLong(IntPtr hwnd, int index, int value);

        public RevitViewAttacher(Window window, IntPtr hwnd, TitleVisibility titleVisibility)
        {
            this.window = window;
            this.hwnd = hwnd;
            this.titleVisibility = titleVisibility;
        }

        internal void Attach()
        {
            System.Windows.Interop.WindowInteropHelper helper = new System.Windows.Interop.WindowInteropHelper(this.window);
            helper.Owner = this.hwnd;
            window.SourceInitialized += RevitViewMvcBase_SourceInitialized;
        }

        /// <summary>
        /// Hides both Maximize and Minimize button from this WPF Window. This should be placed on Loaded event OR Rendered event, NOT ON THE CONSTRUCTOR
        /// </summary>
        private void HideMinimizeMaximizeButton(Window window)
        {
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
            var currentStyle = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, (currentStyle & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX));
        }

        /// <summary>
        /// Hides both Minimize button from this WPF Window. This should be placed on Loaded event OR Rendered event, NOT ON THE CONSTRUCTOR
        /// </summary>
        private void HideMinimizeButton(Window window)
        {
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
            var currentStyle = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, (currentStyle & ~WS_MINIMIZEBOX));
        }

        /// <summary>
        /// Hides the Title bat from this WPF Window. This should be placed on Loaded event OR Rendered event, NOT ON THE CONSTRUCTOR
        /// </summary>
        private void HideTitleBar(Window window)
        {
            var wChrome = new WindowChrome();
            wChrome.CaptionHeight = 0;
            WindowChrome.SetWindowChrome(window, wChrome);
        }

        private void RevitViewMvcBase_SourceInitialized(object sender, EventArgs e)
        {
            switch (this.titleVisibility)
            {
                case TitleVisibility.Default:
                    break;
                case TitleVisibility.HideMinimize:
                    this.HideMinimizeButton(this.window);
                    break;
                case TitleVisibility.HideMinimizeAndMaximize:
                    this.HideMinimizeMaximizeButton(this.window);
                    break;
                case TitleVisibility.HideTitleBar:
                    this.HideTitleBar(this.window);
                    break;
                default:
                    break;
            }
        }

    }
}
