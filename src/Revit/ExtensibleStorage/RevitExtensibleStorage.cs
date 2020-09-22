using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Onbox.Abstractions.V7;
using System;

namespace Onbox.Revit.V7.ExtensibleStorage
{
    /// <summary>
    /// Onbox's Revit Storage, it uses json to store and retrieve data classes
    /// </summary>
    public class RevitJsonStorage<T> : IRevitJsonStorage<T> where T : class, new()
    {
        private readonly string fieldName = "Storage";
        private readonly IJsonService jsonService;
        private readonly RevitExtensibleStorageSettings storageSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        public RevitJsonStorage(IJsonService jsonService, RevitExtensibleStorageSettings storageSettings)
        {
            this.jsonService = jsonService;
            this.storageSettings = storageSettings;
        }

        /// <summary>
        /// Retrieves data from this element's extensible storage
        /// </summary>
        public T Load(Element element)
        {
            using (var entity = this.GetSchemaEntity(element))
            {
                var json = entity.Get<string>(this.fieldName);
                var storage = this.jsonService.Deserialize<T>(json);

                return storage;
            }
        }

        /// <summary>
        /// Resets the extensible storage of this type for this element. This REQUIRES a transaction
        /// </summary>
        public void Reset(Element element)
        {
            using (var entity = this.GetSchemaEntity(element))
            {
                element.DeleteEntity(entity.Schema);
            }
        }

        /// <summary>
        /// Saves this data to this element's extensible storage. This REQUIRES a transaction
        /// </summary>
        public void Save(Element element, T data)
        {
            using (var entity = this.GetSchemaEntity(element))
            {
                var json = this.jsonService.Serialize(data);
                entity.Set(this.fieldName, json);

                element.SetEntity(entity);
            }
        }

        private Entity GetSchemaEntity(Element element)
        {
            var type = typeof(T);

            var containsKey = this.storageSettings.SchemaSettings.ContainsKey(type);
            if (!containsKey)
            {
                throw new Exception($"Extensible Storage for type {type.Name} is not registred on IOC container");
            }

            var schemaSettings = this.storageSettings.SchemaSettings[type];
            var schema = Schema.Lookup(schemaSettings.SchemaGuid);

            if (schema == null)
            {
                schema = this.CreateSchema(schemaSettings);
            }

            var entity = element.GetEntity(schema);
            if (entity.Schema == null)
            {
                entity = new Entity(schema);
            }

            return entity;
        }

        private Schema CreateSchema(RevitSchemaSettings schemaSettings)
        {
            Schema schema;
            SchemaBuilder builder = new SchemaBuilder(schemaSettings.SchemaGuid);

            if (Enum.IsDefined(typeof(AccessLevel), schemaSettings.ReadAccessLevel))
            {
                builder.SetReadAccessLevel((Autodesk.Revit.DB.ExtensibleStorage.AccessLevel)schemaSettings.ReadAccessLevel.GetHashCode());
            }

            if (Enum.IsDefined(typeof(AccessLevel), schemaSettings.WriteAccessLevel))
            {
                builder.SetWriteAccessLevel((Autodesk.Revit.DB.ExtensibleStorage.AccessLevel)schemaSettings.WriteAccessLevel.GetHashCode());
            }
           
            if (!string.IsNullOrWhiteSpace(schemaSettings.VendorId))
            {
                builder.SetVendorId(schemaSettings.VendorId);
            }

            if (!string.IsNullOrWhiteSpace(schemaSettings.SchemaDocumentation))
            {
                builder.SetDocumentation(schemaSettings.SchemaDocumentation);
            }
            
            builder.SetSchemaName(schemaSettings.SchemaName);

            builder.AddSimpleField(this.fieldName, typeof(string));
            schema = builder.Finish();

            if (schema == null)
            {
                throw new Exception($"Onbox was unable to create schema {schemaSettings.SchemaName}: {schemaSettings.SchemaGuid}");
            }

            return schema;
        }
    }
}
