using System;
using System.Windows.Input;

namespace Onbox.Mvc.V1
{
    /// <summary>
    /// Also known as DelegateCommand or RelayCommand, this class is responsible for delegating actions for ViewModels
    /// </summary>
    public class ActionRelayParameterCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Constructs a <see cref="ActionRelayCommand"/> with a delegate that indicates whether it can or can not run
        /// </summary>
        /// <param name="execute">The delegated void method</param>
        /// <param name="canExecute">The Delegated method that indicates whether it can or can not run</param>
        public ActionRelayParameterCommand(Action<object> execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Constructs a <see cref="ActionRelayCommand"/> that can always run
        /// </summary>
        /// <param name="execute">The delegated void method</param>
        public ActionRelayParameterCommand(Action<object> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// The Delegated method that indicates whether it can or can not run
        /// </summary>
        /// <param name="parameter">Generic input object</param>
        /// <returns>Can or can not run</returns>
        public bool CanExecute(object parameter)
        {
            return canExecute != null ? canExecute.Invoke() : true;
        }

        /// <summary>
        /// The delegated void method
        /// </summary>
        /// <param name="parameter">Generic input object</param>
        public void Execute(object parameter)
        {
            if (execute != null)
            {
                execute.Invoke(parameter);
            }
        }
    }
}
