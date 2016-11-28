namespace Zagorapps.Utilities.Suite.UI.Commands
{
    using System;
    using System.Windows.Input;

    public class CommandProvider : ICommandProvider
    {
        public ICommand CreateRelayCommand(Action action)
        {
            Action<object> wrapper = value =>
            {
                action();
            };

            return new RelayCommand<object>(wrapper);
        }

        public ICommand CreateRelayCommand<T>(Action<T> action)
        {
            return new RelayCommand<T>(parameter => action(parameter));
        }
    }
}