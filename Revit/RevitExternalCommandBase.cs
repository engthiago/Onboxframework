using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Di.V7;

namespace Onbox.Revit.V7
{
    public abstract class RevitExternalCommandBase<TApplication> : IExternalCommand where TApplication : RevitExternalAppBase, new ()
    {
        protected readonly IContainerResolver container;

        public RevitExternalCommandBase()
        {
            var type = typeof(TApplication);

            var containerGuid = ContainerProviderReflector.GetContainerGuid(type);
            this.container = RevitExternalAppBase.GetContainer(containerGuid);

            if (this.container == null)
            {
                this.container = Container.Default();
            }
        }

        public abstract Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements);
    }
}
