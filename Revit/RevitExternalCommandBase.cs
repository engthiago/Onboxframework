using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Onbox.Di.V7;
using System.Reflection;

namespace Onbox.Revit.V7
{
    public abstract class RevitExternalCommandBase<T> : IExternalCommand where T : RevitExternalAppBase, new ()
    {
        protected readonly IContainerProvider container;

        public RevitExternalCommandBase()
        {
            var method = typeof(T).BaseType.GetMethod(nameof(RevitExternalAppBase.GetContainer), BindingFlags.Static | BindingFlags.Public);
            this.container = method.Invoke(null, null) as IContainerProvider;

            if (this.container == null)
            {
                this.container = Container.Default();
            }
        }

        public abstract Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements);
    }
}
