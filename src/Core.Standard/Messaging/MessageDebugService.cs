using Onbox.Abstractions.VDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.VDev.Messaging
{
    /// <summary>
    /// Implementation for messages using <see cref="System.Diagnostics.Debug"/> console
    /// </summary>
    public class MessageDebugService : IMessageService
    {
        private string title = "Message Log Service";

        /// <summary>
        /// Shows an error message
        /// </summary>
        public void Error(string message)
        {
            System.Diagnostics.Debug.WriteLine($"****** {title} Error ******");
            System.Diagnostics.Debug.WriteLine(message);
        }

        /// <summary>
        /// Shows a question message
        /// </summary>
        public bool Question(string message)
        {
            System.Diagnostics.Debug.WriteLine($"****** {title} Question ******");
            System.Diagnostics.Debug.WriteLine(message);
            System.Diagnostics.Debug.WriteLine($"****** Log service will always return true ******");
            return true;
        }

        /// <summary>
        /// Sets the title of the messages
        /// </summary>
        public void SetTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                Error("Can not set title to empty.");
                return;
            }
            System.Diagnostics.Debug.WriteLine($"****** {title} Set Title: {newTitle} ******");
            title = newTitle;
        }

        /// <summary>
        /// Shows a message
        /// </summary>
        public void Show(string message)
        {
            System.Diagnostics.Debug.WriteLine($"****** {title} Show ******");
            System.Diagnostics.Debug.WriteLine(message);
        }

        /// <summary>
        /// Shows a warning message
        /// </summary>
        public void Warning(string message)
        {
            System.Diagnostics.Debug.WriteLine($"****** {title} Warning ******");
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}