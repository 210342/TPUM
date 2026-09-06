using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TPUM.Client.ViewModel
{
    public class AsyncCommand : ICommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public AsyncCommand(Func<object, Task> execute) : this(execute, null) { }

        public AsyncCommand(Func<object, Task> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => _execute.Invoke(parameter);

        public Task ExecuteAsync(object parameter) => _execute.Invoke(parameter);

        public void RaiseExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
