using System;

namespace Onbox.Mvvm.V1
{
    /// <summary>
    /// The base class for all ViewModels. It is also responsible for handling the Notifications to UI and handling DialogResults
    /// </summary>
    public abstract class ViewModelBase<T> : NotifyPropertyBase where T : IViewWindow, new()
    {
        public T View { get; set; }
        public TitleVisibility TitleVisibility { get; set; }

        /// <summary>
        /// Tells the ViewModel if the Dialog was confirmed e.g. OK button was pressed or not confirmed e.g. Cancel button was pressed.
        /// </summary>
        public bool DialogResult { get; set; }

        /// <summary>
        /// Changes the state of the <see cref="DialogResult"/>
        /// </summary>
        public virtual void ConfirmDialog()
        {
            this.DialogResult = true;
        }

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public bool ShowDialog()
        {
            this.View = new T();
            this.View.DataContext = this;
            this.View.OnInit = this.OnInit;
            this.View.OnDestroy = this.OnDestroy;
            this.View.TitleVisibility = this.TitleVisibility;
            this.View.CanCloseDialog = this.CanCloseDialog;
            return (bool)this.View.ShowDialog();
        }

        public virtual void OnInit()
        {
        }

        public virtual void OnDestroy()
        {
        }
    }
}
