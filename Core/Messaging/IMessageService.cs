namespace Onbox.Core.V3.Messaging
{
    /// <summary>
    /// Contract for messaging activities
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// Shows an error message
        /// </summary>
        void Error(string message);
        /// <summary>
        /// Shows a question message
        /// </summary>
        bool Question(string message);
        /// <summary>
        /// Sets the title of the messages
        /// </summary>
        void SetTitle(string newTitle);
        /// <summary>
        /// Shows a message
        /// </summary>
        void Show(string message);
        /// <summary>
        /// Shows a warning message
        /// </summary>
        void Warning(string message);
    }
}