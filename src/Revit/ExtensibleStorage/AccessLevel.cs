namespace Onbox.Revit.V7.ExtensibleStorage
{
    /// <summary>
    /// Defines access levels to objects in the Extensible Storage framework
    /// </summary>
    public enum AccessLevel
    {
        /// <summary>
        ///  Anybody has access to the object
        /// </summary>
        Public = 1,
        /// <summary>
        /// Only object vendor has access to it
        /// </summary>
        Vendor = 2,
        /// <summary>
        ///  Only application that created the object has access to it
        /// </summary>
        Application = 3
    }
}
