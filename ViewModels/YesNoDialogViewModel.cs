using System.Reactive;
using ReactiveUI;

namespace PvZHCardEditor.ViewModels;

public class YesNoDialogViewModel : ViewModelBase
{
    private string _prompt = string.Empty;

    public ReactiveCommand<Unit, bool> YesCommand { get; } = ReactiveCommand.Create(() => true);
    public ReactiveCommand<Unit, bool> NoCommand { get; } = ReactiveCommand.Create(() => false);

    public string Prompt
    {
        get => _prompt;
        set => this.RaiseAndSetIfChanged(ref _prompt, value);
    }
}