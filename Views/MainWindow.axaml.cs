using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using PvZHCardEditor.ViewModels;
using ReactiveUI;

namespace PvZHCardEditor.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        this.WhenActivated(d => d(ViewModel!.OpenInteraction.RegisterHandler(DoOpenAsync)));
    }

    private async Task DoOpenAsync(InteractionContext<MainWindowViewModel, string?> interaction)
    {
        var dialog = new OpenFolderDialog();
        var result = await dialog.ShowAsync(this);
        interaction.SetOutput(result);
    }
}