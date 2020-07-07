using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Di.V7;

namespace Onbox.Revit.V7
{
    /// <summary>
    /// Base class to implement when implementing RevitExternal Commands with containers
    /// </summary>
    public abstract class RevitExternalCommandBase<TApplication> : IExternalCommand where TApplication : RevitExternalAppBase, new ()
    {
        /// <summary>
        /// Execution of External Command
        /// </summary>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            IContainer container = GetContainer();

            return Execute(container, commandData, ref message, elements);
        }

        private static IContainer GetContainer()
        {
            var type = typeof(TApplication);
            var containerGuid = ContainerProviderReflector.GetContainerGuid(type);
            var container = RevitContainerProviderBase.GetContainer(containerGuid);
            return container;
        }

        /// <summary>
        /// Execution of External Command
        /// </summary>
        public abstract Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements);
    }
}
