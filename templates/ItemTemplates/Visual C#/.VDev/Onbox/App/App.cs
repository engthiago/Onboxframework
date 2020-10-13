using Onbox.Abstractions.VDev;
using Onbox.Revit.VDev;
using Onbox.Revit.VDev.Applications;
using Onbox.Revit.VDev.UI;
using Autodesk.Revit.UI;

namespace $rootnamespace$
{
	[ContainerProvider("$guid1$")]
    public class $safeitemname$ : RevitApp
    {
		public override void OnCreateRibbon(IRibbonManager ribbonManager)
        {
			
        }

        public override Result OnStartup(IContainer container, UIControlledApplication application)
        {
			
			return Result.Succeeded;
        }

        public override Result OnShutdown(IContainerResolver container, UIControlledApplication application)
        {
			
			return Result.Succeeded;
        }
    }

}