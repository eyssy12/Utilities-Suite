namespace Zagorapps.Utilities.Suite.UI.Commands
{
    using System;
    using System.Windows.Input;

    public interface ICommandProvider
    {
        ICommand CreateRelayCommand(Action action);

        ICommand CreateRelayCommand<T>(Action<T> action);
    }
}