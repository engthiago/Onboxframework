using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
