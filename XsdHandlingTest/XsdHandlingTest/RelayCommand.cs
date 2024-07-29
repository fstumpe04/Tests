using System.Windows.Input;

namespace XsdHandlingTest
{
    internal class RelayCommand : ICommand
    {
        private readonly Action _Execute;
        public RelayCommand(Action execute)
        {
            _Execute = execute;
        }

        #region ICommand

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            _Execute.Invoke();
        }

        #endregion
    }
}