using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;

namespace PvZHCardEditor.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _id = string.Empty;
    private string _statusText = "Open a folder to begin editing";
    private bool _statusShown = true;

    public ICommand OpenCommand => ReactiveCommand.CreateFromTask(DoOpenAsync);
    public Interaction<MainWindowViewModel, string?> OpenInteraction { get; } = new();
    
    public bool DataLoaded { get; set; }

    public string Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    public string StatusText
    {
        get => _statusText;
        set
        {
            this.RaiseAndSetIfChanged(ref _statusText, value);
            // I couldn't find a better way to replay the status fade animation
            StatusShown = false;
            StatusShown = true;
        }
    }

    public bool StatusShown
    {
        get => _statusShown;
        set => this.RaiseAndSetIfChanged(ref _statusShown, value);
    }

    private async Task DoOpenAsync()
    {
        var result = await OpenInteraction.Handle(this);
        if (result == null)
            return;
        StatusText = "Opened folder " + result;
        DataLoaded = true;
    }
}