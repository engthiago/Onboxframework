using System;

namespace Onbox.Revit.VDev
{   
    /// <summary>
    /// Provides a way for the Onbox framework to locate the IOC container for your application
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed public class ContainerProviderAttribute : Attribute
    {
        /// <summary>
        /// The GUID to identify your apps container
        /// </summary>
        public string ContainerGuid { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ContainerProviderAttribute(string containerGuid)
        {
            this.ContainerGuid = containerGuid;
        }
    }
}