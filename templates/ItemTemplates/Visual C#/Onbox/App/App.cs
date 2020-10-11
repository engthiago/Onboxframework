using Onbox.Abstractions.V7;
using Onbox.Revit.V7;
using Onbox.Revit.V7.Applications;
using Onbox.Revit.V7.UI;
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
