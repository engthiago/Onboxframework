using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Di.V7;

namespace Onbox.Revit.V7
{
    static internal class RevitInjector
    {
        static internal void AddRevitUI(IContainer container, UIControlledApplication application)
        {
            var revitUIApp = new RevitUIApp
            {
                isViewerMode = application.IsViewerModeActive,
                languageType = (RevitLanguage)application.ControlledApplication.Language.GetHashCode(),
                versionBuild = application.ControlledApplication.VersionBuild,
                versionNumber = application.ControlledApplication.VersionNumber,
                subVersionNumber = application.ControlledApplication.SubVersionNumber,
                versionName = application.ControlledApplication.VersionName,
                revitWindowHandle = application.MainWindowHandle
            };

            container.AddSingleton<IRevitUIApp>(revitUIApp);
            container.AddSingleton<Document>(null);
        }
    }
}
