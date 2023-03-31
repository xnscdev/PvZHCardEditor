using ReactiveUI;

namespace PvZHCardEditor.ViewModels;

public class CheckBoxWrapper<T> : ReactiveObject
{
    private bool _isSelected;
    private T _value;

    public CheckBoxWrapper(T value)
    {
        _value = value;
    }

    public T Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => this.RaiseAndSetIfChanged(ref _isSelected, value);
    }
}