using System;

namespace Onbox.Revit.V7
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed public class ContainerProviderAttribute : Attribute
    {
        public string containerGuid { get; }

        public ContainerProviderAttribute(string containerGuid)
        {
            this.containerGuid = containerGuid;
        }
    }
}
