using Onbox.Core.V7.Reporting;
using Onbox.Revit.V7;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Onbox.Mvc.Revit.V7
{
    /// <summary>
    /// Reports progress via a WPF Window
    /// </summary>
    public partial class RevitProgressIndicatorView : RevitViewMvcBase, IProgressIndicator
    {
        public int Total { get; set; }
        public int CurrentProgress { get; set; }
        public string CurrentTaskName { get; set; }
        public bool Finished { get; set; }
        public bool RequestCancel { get; set; }

        public bool CanCancel { get; set; }
        public bool CanClose { get; set; }

        public string CancellingMessage { get; set; }


        private Action action;


        public RevitProgressIndicatorView(IRevitUIApp revitUIApp) : base(revitUIApp)
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.CancellingMessage = "Cancelling... Please wait";
        }

        public override void OnAfterInit()
        {
            try
            {
                for (int i = 0; i < Total; i++)
                {
                    this.action?.Invoke();
                }
                this.Finish();
            }
            catch (ProgressCancelledException e)
            {
                if (e.HasMessage())
                {
                    this.CancellingMessage = e.Message;
                }
                this.Cancel();
            }
            catch (Exception e)
            {
                this.Error = e.Message;
                this.CanCancel = false;
                this.CanClose = true;
            }
        }

        public void Run(int total, bool canCancel, Action action)
        {
            this.Total = total <= 1 ? 1 : total;
            this.CanCancel = canCancel;
            this.action = action;
            this.ShowDialog();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Cancel();
            }
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            this.Cancel();
        }

        private async void Cancel()
        {
            try
            {
                if (this.CanCancel == false) return;

                this.CanCancel = false; //No need for it anymore, it will hide itself on UI
                this.RequestCancel = true;

                await Task.Delay(1000);

                this.Close();
            }
            catch (Exception e)
            {
            }
        }

        public void Iterate(string name)
        {
            this.CurrentTaskName = name;
            this.CurrentProgress++;
            this.RefreshAllProperties();

            // Focus the window
            this.Activate();
            this.Focus();

            this.HandleCancelled();
        }

        public void Finish()
        {
            try
            {
                this.RefreshAllProperties();

                this.Finished = true;
                this.DialogResult = true;
            }
            catch (Exception e)
            {
            }
        }

        public void HandleCancelled()
        {
            if (this.RequestCancel)
            {
                this.Cancel();
                throw new ProgressCancelledException();
            }
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public bool FinishedSuccessfully()
        {
            return this.Finished;
        }

        public void Reset(int total)
        {
            this.Total = total <= 1 ? 1 : total;
            this.CurrentProgress = 0;
        }
    }

}
