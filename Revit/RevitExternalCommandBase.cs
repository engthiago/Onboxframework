using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Di.V7;

namespace Onbox.Revit.V7
{
    public abstract class RevitExternalCommandBase<TApplication> : IExternalCommand where TApplication : RevitExternalAppBase, new ()
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            IContainer container = GetContainer();

            return Execute(container, commandData, ref message, elements);
        }

        private static IContainer GetContainer()
        {
            var type = typeof(TApplication);
            var containerGuid = ContainerProviderReflector.GetContainerGuid(type);
            var container = RevitContainerBase.GetContainer(containerGuid);
            return container;
        }

        public abstract Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements);
    }
}
