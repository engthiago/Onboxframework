using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onbox.Core.V3.Reporting
{
    /// <summary>
    /// Reports progress to the debug console
    /// </summary>
    public class ProgressConsoleDebugIndicator : IProgressIndicator
    {
        private int total;
        private int current;
        private bool canCancel;

        private bool finishedSuccessfully;

        public bool FinishedSuccessfully()
        {
            return this.finishedSuccessfully;
        }

        public void Iterate(string name)
        {
            System.Diagnostics.Debug.WriteLine($"****** Progress Indicator {++current} of {total}  ******");
            System.Diagnostics.Debug.WriteLine($"****** {name} ******");
        }

        public void Run(int total, bool canCancel, Action action)
        {
            this.total = total <= 1 ? 1 : total;
            this.canCancel = canCancel;
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

        public void Reset(int total)
        {
            this.total = total <= 1 ? 1 : total;
            this.current = 0;
        }
    }
}
