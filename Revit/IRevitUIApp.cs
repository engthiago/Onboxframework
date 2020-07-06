using System;

namespace Onbox.Revit.V7
{
    public interface IRevitUIApp
    {
        string GetVersionBuild();
        string GetVersionName();
        string GetVersionNumber();
        string GetSubVersionNumber();
        RevitLanguage GetLanguage();
        IntPtr GetRevitWindowHandle();
        bool GetIsViewerMode();
    }
}
