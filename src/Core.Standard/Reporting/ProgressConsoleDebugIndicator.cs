using Onbox.Abstractions.VDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.VDev.Reporting
{
    /// <summary>
    /// Reports progress to the debug console
    /// </summary>
    public class ProgressConsoleDebugIndicator : IProgressIndicator
    {
        private int total;
        private int current;

        private bool finishedSuccessfully;

        /// <summary>
        /// Indicates if the process has finished and was successful
        /// </summary>
        /// <returns></returns>
        public bool FinishedSuccessfully()
        {
            return this.finishedSuccessfully;
        }

        /// <summary>
        /// Adds one iteration to the process with a given message
        /// </summary>
        public void Iterate(string name)
        {
            System.Diagnostics.Debug.WriteLine($"****** Progress Indicator {++current} of {total}  ******");
            System.Diagnostics.Debug.WriteLine($"****** {name} ******");
        }

        /// <summary>
        /// Runs progress action
        /// </summary>
        public void Run(int total, bool canCancel, Action action)
        {
            this.total = total <= 1 ? 1 : total;
            try
            {
                action?.Invoke();
                finishedSuccessfully = true;
            }
            catch (ProgressCancelledException e)
            {
                System.Diagnostics.Debug.WriteLine($"****** Progress Indicator Cancelled ******");
                if (e.HasMessage())
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"****** Progress Indicator Failed ******");
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Resets the progress
        /// </summary>
        public void Reset(int total)
        {
            this.total = total <= 1 ? 1 : total;
            this.current = 0;
        }
    }
}