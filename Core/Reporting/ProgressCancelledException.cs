using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.V4.Reporting
{
    public class ProgressCancelledException : Exception
    {
        private bool hasMessage;

        public ProgressCancelledException()
        {
        }

        public ProgressCancelledException(string message) : base(message)
        {
            this.hasMessage = true;
        }

        public bool HasMessage()
        {
            return this.hasMessage;
        }
    }
}
