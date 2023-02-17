using System;

namespace PvZHCardEditor
{
    internal class EditorActionStack
    {
        private const int StackSize = 16;
        private readonly LimitedStack<EditorAction> _undoStack = new(StackSize);
        private readonly LimitedStack<EditorAction> _redoStack = new(StackSize);

        public void AddAction(EditorAction action)
        {
            _undoStack.Push(action);
            _redoStack.Clear();
            action.DoAction();
            GameDataManager.MarkUnsavedChanges();
        }

        public string UndoAction()
        {
            if (!_undoStack.TryPop(out var action))
                return "Nothing to undo";
            _redoStack.Push(action);
            action.DoReverseAction();
            GameDataManager.MarkUnsavedChanges();
            return $"Undo {action.Description}";
        }

        public string RedoAction()
        {
            if (!_redoStack.TryPop(out var action))
                return "Nothing to redo";
            _undoStack.Push(action);
            action.DoAction();
            GameDataManager.MarkUnsavedChanges();
            return $"Redo {action.Description}";
        }

        public void Reset()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }
    }

    internal class EditorAction
    {
        private readonly Func<object?, object?> _action;
        private readonly Action<object?, object?> _reverseAction;
        private readonly object? _parameter;
        private object? _data;

        public string Description { get; }

        public EditorAction(Func<object?, object?> action, Action<object?, object?> reverseAction, object? parameter, string description)
        {
            _action = action;
            _reverseAction = reverseAction;
            _parameter = parameter;
            Description = description;
        }

        public void DoAction()
        {
            _data = _action(_parameter);
        }

        public void DoReverseAction()
        {
            _reverseAction(_parameter, _data);
            _data = null;
        }
    }
}
