using ReactiveUI;

namespace PvZHCardEditor.ViewModels;

public class EditPrimitiveDialogViewModel<T> : EditDialogViewModel
{
    private T _value = default!;

    public string Prompt => $"{typeof(T).Name} value";

    public T Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }
}