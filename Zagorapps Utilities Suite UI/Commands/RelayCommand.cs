namespace Zagorapps.Utilities.Suite.UI.Commands
{
    using System;
    using System.Windows.Input;

    public class RelayCommand<T> : ICommand
    {
        protected readonly Action<T> Action = null;
        protected readonly Predicate<T> CanExecutePredicate = null;

        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<T> action, Predicate<T> canExecute)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action), "No executable action has been provided - The relay command would not be able to invoke an operation");
            }

            this.Action = action;
            this.CanExecutePredicate = canExecute;
        }

                public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return this.CanExecutePredicate == null ? true : this.CanExecutePredicate((T)parameter);
        }

        public void Execute(object parameter)
        {
            this.Action((T)parameter);
        }
    }
}