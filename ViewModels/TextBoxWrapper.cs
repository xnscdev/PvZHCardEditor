using ReactiveUI;

namespace PvZHCardEditor.ViewModels;

public class TextBoxWrapper : ReactiveObject
{
    private string _text;

    public TextBoxWrapper(string text)
    {
        _text = text;
    }

    public string Text
    {
        get => _text;
        set => this.RaiseAndSetIfChanged(ref _text, value);
    }
}