using System.Reactive;
using ReactiveUI;

namespace PvZHCardEditor.ViewModels;

public class EditDialogViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, bool> EditCommand { get; } = ReactiveCommand.Create(() => true);
    public ReactiveCommand<Unit, bool> CancelCommand { get; } = ReactiveCommand.Create(() => false);
}