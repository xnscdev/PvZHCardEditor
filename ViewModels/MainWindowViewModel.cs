using ReactiveUI;

namespace PvZHCardEditor.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _id = string.Empty;

    public string Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }
}