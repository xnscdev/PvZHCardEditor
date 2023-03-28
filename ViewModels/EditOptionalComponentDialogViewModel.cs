using PvZHCardEditor.Models;
using ReactiveUI;

namespace PvZHCardEditor.ViewModels;

public class EditOptionalComponentDialogViewModel<T> : EditComponentDialogViewModel<T> where T : EntityComponentBase
{
    private bool _isNull;

    public bool IsNull
    {
        get => _isNull;
        set => this.RaiseAndSetIfChanged(ref _isNull, value);
    }
}