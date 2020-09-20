using System;

namespace Onbox.Core.V7
{
    /// <summary>
    /// Onbox's abstract base class
    /// </summary>
    public abstract class OnboxExceptionBase : Exception
    {
        /// <summary>
        /// Indicates if this exception has message to report
        /// </summary>
        protected bool hasMessage;

        /// <summary>
        /// Contructor
        /// </summary>
        public OnboxExceptionBase()
        {

        }

        /// <summary>
        /// Contructor with a message
        /// </summary>
        public OnboxExceptionBase(string message) : base(message)
        {
            this.hasMessage = !string.IsNullOrWhiteSpace(message);
        }

        /// <summary>
        /// Checks if the exception has a message to display
        /// </summary>
        public bool HasMessage()
        {
            return this.hasMessage;
        }
    }
}
