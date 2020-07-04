using System;

namespace Onbox.Revit.V7
{
    static internal class ContainerProviderReflector
    {
        static internal string GetContainerGuid(object target)
        {
            var type = target.GetType();

            var containerGuid = GetContainerGuid(type);
            return containerGuid;
        }

        static internal string GetContainerGuid(Type type)
        {
            // Gets the Container Provider Attribute
            var attributes = type.GetCustomAttributes(typeof(ContainerProviderAttribute), false);
            if (attributes.Length == 0)
            {
                throw new NotImplementedException($"{type.FullName} not decorated with {nameof(ContainerProviderAttribute)}");
            }

            var containerProvider = attributes[0];
            var containerProviderType = attributes[0].GetType();
            var containerGuidProperty = containerProviderType.GetProperty(nameof(ContainerProviderAttribute.containerGuid));
            var containerGuid = containerGuidProperty.GetValue(containerProvider).ToString();

            if (string.IsNullOrWhiteSpace(containerGuid))
            {
                throw new NullReferenceException($"{nameof(ContainerProviderAttribute)} of {type.FullName} has an empty or invalid guid");
            }

            return containerGuid;
        }
    }
}
