using Autodesk.Revit.DB;

namespace Onbox.Revit.V7.ExtensibleStorage
{
    /// <summary>
    /// Onbox's Revit Storage, it uses json to store and retrieve data classes
    /// </summary>
    public interface IRevitJsonStorage<T> where T: class, new()
    {
        /// <summary>
        /// Saves this data to this element's extensible storage. This REQUIRES a transaction
        /// </summary>
        void Save(Element element, T data);

        /// <summary>
        /// Retrieves data from this element's extensible storage
        /// </summary>
        T Load(Element element);

        /// <summary>
        /// Resets the extensible storage of this type for this element. This REQUIRES a transaction
        /// </summary>
        void Reset(Element element);
    }
}
