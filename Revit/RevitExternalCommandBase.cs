using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Di.V7;
using System.Reflection;

namespace Onbox.Revit.V7
{
    public abstract class RevitExternalCommandBase<TApplication> : IExternalCommand where TApplication : RevitExternalAppBase, new ()
    {
        protected readonly IContainerProvider container;

        public RevitExternalCommandBase()
        {
            var type = typeof(TApplication);
            var isContainerOrigin = false;

            while (!isContainerOrigin)
            {
                type = type.BaseType;
                if (type == null)
                {
                    throw new System.Exception("Container Origin not found!");
                }
                isContainerOrigin = type.GetCustomAttribute<ContainerOriginAttribute>() != null ? true : false;
            }

            var method = type.GetMethod(nameof(RevitExternalAppBase.GetContainer), BindingFlags.Static | BindingFlags.Public);
            this.container = method.Invoke(null, null) as IContainerProvider;

            if (this.container == null)
            {
                this.container = Container.Default();
            }
        }

        public abstract Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements);
    }
}
