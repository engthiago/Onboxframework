using System;
using System.Linq;

namespace Onbox.Revit.V7
{
    /// <summary>
    /// Helper class to locate the container Guid belonging to an app
    /// </summary>
    static internal class ContainerProviderReflector
    {
        /// <summary>
        /// Locates the container guid passing in an instance of an app
        /// </summary>
        static internal string GetContainerGuid(RevitContainerProviderBase target)
        {
            var type = target.GetType();
            var containerGuid = GetContainerGuid(type);
            return containerGuid;
        }

        /// <summary>
        /// Locates the container guid passing in a type of an app
        /// </summary>
        static internal string GetContainerGuid(Type appType)
        {
            var attribute = GetContainerAttribute(appType);
            var containerGuid = GetContainerAttributeGuid(attribute);

            return containerGuid;
        }

        static private object GetContainerAttribute(Type appType)
        {
            var attributes = appType.GetCustomAttributes(typeof(ContainerProviderAttribute), false);
            var attribute = attributes.ElementAtOrDefault(0);
            if (attribute == null)
            {
                throw new Exception($"No {nameof(ContainerProviderAttribute)} found on {appType.FullName}.");
            }

            return attribute;
        }

        static private string GetContainerAttributeGuid(object attribute)
        {
            var containerProviderType = attribute.GetType();
            var containerGuidProperty = containerProviderType.GetProperty(nameof(ContainerProviderAttribute.containerGuid));
            var containerGuid = containerGuidProperty?.GetValue(attribute)?.ToString();

            if (string.IsNullOrWhiteSpace(containerGuid))
            {
                throw new Exception($"{nameof(ContainerProviderAttribute)} should have a valid non-empty Guid.");
            }

            return containerGuid;
        }
    }
}
