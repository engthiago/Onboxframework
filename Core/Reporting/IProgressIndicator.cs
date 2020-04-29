using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.V3.Reporting
{
    /// <summary>
    /// Contract that reports information each iteration
    /// </summary>
    public interface IProgressIndicator
    {
        /// <summary>
        /// Indicates if the process is already finished and was successful
        /// </summary>
        /// <returns></returns>
        bool FinishedSuccessfully();

        /// <summary>
        /// Adds one iteration to the process with a given message
        /// </summary>
        /// <param name="name"></param>
        void Iterate(string name);

        /// <summary>
        /// Runs progress action
        /// </summary>
        void Run(int total, bool canCancel, Action action);

        /// <summary>
        /// Resets the progress
        /// </summary>
        void Reset(int total);
    }
}
