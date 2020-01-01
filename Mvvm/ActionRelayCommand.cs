using System;
using System.Windows.Input;

namespace Onbox.Mvvm.V1
{
    public class ActionRelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;
        private ActionRelayCommand openManagePresetsCommand;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public ActionRelayCommand(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public ActionRelayCommand(Action execute) : this(execute, null)
        {
        }

        public ActionRelayCommand(ActionRelayCommand openManagePresetsCommand)
        {
            this.openManagePresetsCommand = openManagePresetsCommand;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute != null ? canExecute.Invoke() : true;
        }

        public void Execute(object parameter)
        {
            execute?.Invoke();
        }
    }
}
