﻿using System;
using System.Windows.Input;

namespace TPUM.Client.ViewModel
{
    public class Command : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public Command(Action<object> execute) : this(execute, null) { }

        public Command(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => _execute.Invoke(parameter);

        public void RaiseExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
