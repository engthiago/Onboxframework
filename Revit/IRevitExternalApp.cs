using Autodesk.Revit.UI;
using Onbox.Di.V7;
using Onbox.Revit.V7.UI;

namespace Onbox.Revit.V7
{
    public interface IRevitExternalApp: IExternalApplication
    {
        void OnCreateRibbon(IRibbonManager ribbonManager);
        Result OnShutdown(IContainerResolver container, UIControlledApplication application);
        Result OnStartup(IContainer container, UIControlledApplication application);
    }
}