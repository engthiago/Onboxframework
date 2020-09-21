using System;

namespace Onbox.Revit.V7.ExtensibleStorage
{
    /// <summary>
    /// Definition of a schema
    /// </summary>
    public class RevitSchemaSettings
    {
        /// <summary>
        ///  The identifier of the Schema
        /// </summary>
        public Guid SchemaGuid { get; set; }
        /// <summary>
        /// The overall description of the Schema
        /// </summary>
        public string SchemaDocumentation { get; set; }
        /// <summary>
        /// The user-friendly name of the Schema
        /// </summary>
        public string SchemaName { get; set; }
        /// <summary>
        /// Read access level of the schema
        /// </summary>
        public AccessLevel ReadAccessLevel { get; set; }
        /// <summary>
        /// Write access level of the schema
        /// </summary>
        public AccessLevel WriteAccessLevel { get; set; }
        /// <summary>
        ///  The id of the third-party vendor that may access entities of this Schema
        /// </summary>
        public string VendorId { get; set; }
    }
}
