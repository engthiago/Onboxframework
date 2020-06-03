using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Revit.V7
{
    public class RevitUIApp : IRevitUIApp
    {
        public RevitLanguage languageType;
        public string subVersionNumber;
        public string versionNumber;
        public string versionBuild;
        public string versionName;
        public IntPtr revitWindowHandle;
        public bool isViewerMode;

        public RevitLanguage GetLanguage()
        {
            return languageType;
        }

        public string GetSubVersionNumber()
        {
            return subVersionNumber;
        }

        public string GetVersionBuild()
        {
            return versionBuild;
        }

        public string GetVersionName()
        {
            return versionName;
        }

        public string GetVersionNumber()
        {
            return versionNumber;
        }

        public IntPtr GetRevitWindowHandle()
        {
            return revitWindowHandle;
        }

        public bool GetIsViewerMode()
        {
            return isViewerMode;
        }
    }
}
