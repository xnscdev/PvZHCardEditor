using System.Reactive;
using ReactiveUI;

namespace PvZHCardEditor.ViewModels;

public class EditPrimitiveDialogViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, bool> EditCommand { get; } = ReactiveCommand.Create(() => true);
    public ReactiveCommand<Unit, bool> CancelCommand { get; } = ReactiveCommand.Create(() => false);
}

public class EditPrimitiveDialogViewModel<T> : EditPrimitiveDialogViewModel
{
    private T _value;

    public string Prompt => $"{typeof(T).Name} value";

    public T Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }
}