using Onbox.Abstractions.VDev;
using System;
using System.Collections.Generic;

namespace Onbox.Revit.VDev.ExtensibleStorage
{
    /// <summary>
    /// A builder for Revit Extensible Storage Schemas
    /// </summary>
    public class RevitStorageBuilder
    {
        private readonly IContainer container;
        private RevitExtensibleStorageSettings storageSettings;
        private List<Action> storageConfigurations;

        internal RevitStorageBuilder(IContainer container)
        {
            this.container = container;
            this.storageConfigurations = new List<Action>();
            this.storageSettings = new RevitExtensibleStorageSettings();
            this.storageSettings.SchemaSettings = new Dictionary<Type, RevitSchemaSettings>();
        }

        /// <summary>
        /// Configure a Json Storage of a specific data class
        /// </summary>
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
            this.storageConfigurations.Add(() =>
            {
                this.container.AddSingleton<IRevitJsonStorage<T>, RevitJsonStorage<T>>();
            });
            return this;
        }

        /// <summary>
        /// Builds the Storage and adds all the dependencies to the container
        /// </summary>
        public void Build()
        {
            this.container.AddSingleton(storageSettings);
            foreach (var configuration in this.storageConfigurations)
            {
                configuration.Invoke();
            }
        }
    }

    /// <summary>
    /// Extensions for adding Extensible Storage Support
    /// </summary>
    public static class RevitExtensibleStorageEntensions
    {
        /// <summary>
        /// Creates a builder for Revit Extensible Storage Schemas
        /// </summary>
        public static RevitStorageBuilder CreateExtensibleStorageBuilder(this IContainer container)
        {
            var builder = new RevitStorageBuilder(container);
            return builder;
        }
    }
}