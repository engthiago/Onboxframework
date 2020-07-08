using System;
using System.Windows;
using System.Windows.Controls;

namespace Onbox.Mvc.V7.CustomControls
{
    /// <summary>
    /// Interaction logic for ProgressBarControl.xaml
    /// </summary>
    public partial class ProgressBarControl : UserControl
    {
        public int CurrentPercentage { get; protected set; }
        private delegate void ProgressBarDelegate();

        public int Total
        {
            get { return (int)GetValue(TotalProperty); }
            set { SetValue(TotalProperty, value); }
        }

        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register("Total", typeof(int), typeof(ProgressBarControl), new PropertyMetadata(0, OnTotalChanged));

        private static void OnTotalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProgressBarControl progressControl)
            {
                progressControl.UpdatePercentages();
            }
        }

        private void UpdatePercentages()
        {
            //Avoid division to zero
            int total = Total <= 0 ? 1 : Total;
            CurrentPercentage = CurrentProgress * 100 / total;
            try
            {
                Dispatcher.Invoke(new ProgressBarDelegate(Update), System.Windows.Threading.DispatcherPriority.Background);
            }
            catch (Exception){}
        }

        private void Update()
        {
            PART_Progress.Value = CurrentPercentage;
            if (!string.IsNullOrWhiteSpace(CurrentTaskName))
            {
                PART_TaskName.Text = CurrentTaskName;
            }
        }

        public int CurrentProgress
        {
            get { return (int)GetValue(CurrentProgressProperty); }
            set { SetValue(CurrentProgressProperty, value); }
        }

        public static readonly DependencyProperty CurrentProgressProperty =
            DependencyProperty.Register("CurrentProgress", typeof(int), typeof(ProgressBarControl), new PropertyMetadata(0, OnCurrentProgressChanged));

        private static void OnCurrentProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProgressBarControl progressControl)
            {
                progressControl.UpdatePercentages();
            }
        }

        public string CurrentTaskName
        {
            get { return (string)GetValue(CurrentTaskNameProperty); }
            set { SetValue(CurrentTaskNameProperty, value); }
        }

        public static readonly DependencyProperty CurrentTaskNameProperty =
            DependencyProperty.Register("CurrentTaskName", typeof(string), typeof(ProgressBarControl), new PropertyMetadata(null));

        public ProgressBarControl()
        {
            InitializeComponent();
        }



        public bool Finished
        {
            get { return (bool)GetValue(FinishedProperty); }
            set { SetValue(FinishedProperty, value); }
        }

        public static readonly DependencyProperty FinishedProperty =
            DependencyProperty.Register("Finished", typeof(bool), typeof(ProgressBarControl), new PropertyMetadata(false, OnFinished));

        private static void OnFinished(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProgressBarControl progressControl)
            {
                if (progressControl.Finished == true)
                {
                    progressControl.Finish();
                }
            }
        }

        private void Finish()
        {
            Window window = VisualTreeHelpers.GetParent<Window>(this);
            window?.Close();
        }
    }
}
