using System;
using System.Windows.Input;

namespace PvZHCardEditor
{
    internal class DelegateCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private readonly Action<object?> _action;
        private readonly Func<object?, bool>? _canExecute;

        public DelegateCommand(Action<object?> action, Func<object?, bool>? canExecute = null)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public void Execute(object? parameter) => _action.Invoke(parameter);

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void InvokeCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
