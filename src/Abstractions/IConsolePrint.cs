namespace Onbox.Abstractions.VDev
{
    /// <summary>
    /// A contract to types that can print messages to console
    /// </summary>
    public interface IConsolePrint
    {
        /// <summary>
        /// Enables console printing
        /// </summary>
        /// <param name="enabled">flag to enable or disable console priting.</param>
        void EnableConsolePrinting(bool enabled);
    }

}
