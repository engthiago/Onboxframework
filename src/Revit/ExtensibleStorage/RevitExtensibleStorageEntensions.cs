using Onbox.Abstractions.V7;
using System;
using System.Collections.Generic;

namespace Onbox.Revit.V7.ExtensibleStorage
{
    public class RevitStorageBuilder
    {
        private readonly IContainer container;
        private RevitExtensibleStorageSettings storageSettings;

        internal RevitStorageBuilder(IContainer container)
        {
            this.container = container;
            storageSettings = new RevitExtensibleStorageSettings();
            storageSettings.SchemaSettings = new Dictionary<Type, RevitSchemaSettings>();
        }

        public RevitStorageBuilder ConfigureJsonStorage<T>(Action<RevitSchemaSettings> config) where T : class, new()
        {
            var storageType = typeof(T);
            if (config == null)
            {
                throw new ArgumentException($"Adding Revit Json Storage of type {storageType.FullName} requires Schema settings.");
            }

            var settings = new RevitSchemaSettings();
            config.Invoke(settings);

            if (Guid.Empty == settings.SchemaGuid)
            {
                throw new ArgumentException($"Revit Json Storage of type {storageType.FullName} requires Schema Guid.");
            }

            if (string.IsNullOrWhiteSpace(settings.SchemaName))
            {
                throw new ArgumentException($"Revit Json Storage of type {storageType.FullName} requires Schema name.");
            }

            this.storageSettings.SchemaSettings.Add(storageType, settings);
            this.container.AddSingleton<IRevitJsonStorage<T>, RevitJsonStorage<T>>();
            return this;
        }

        public void Build()
        {
            this.container.AddSingleton(storageSettings);
        }
    }

    public static class RevitExtensibleStorageEntensions
    {
        public static RevitStorageBuilder CreateExtensibleStorageBuilder(this IContainer container)
        {
            var builder = new RevitStorageBuilder(container);
            return builder;
        }
    }
}
