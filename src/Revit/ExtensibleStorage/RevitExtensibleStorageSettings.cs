using System;
using System.Collections.Generic;

namespace Onbox.Revit.VDev.ExtensibleStorage
{
    /// <summary>
    /// Contains definitions and settings for types and their schemas
    /// </summary>
    public class RevitExtensibleStorageSettings
    {
        /// <summary>
        /// Type dictionaries for types and their schemas
        /// </summary>
        public Dictionary<Type, RevitSchemaSettings> SchemaSettings { get; set; }
    }
}