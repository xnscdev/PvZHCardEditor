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
        this.WhenActivated(d =>
        {
            d(ViewModel!.ShowSelectFolderDialog.RegisterHandler(ShowSelectFolderDialogAsync));
            d(ViewModel!.ShowYesNoDialog.RegisterHandler(ShowYesNoDialogAsync));
            d(ViewModel!.ShowEditPrimitiveDialog.RegisterHandler(ShowEditPrimitiveDialogAsync));
        });
    }

    private async Task ShowSelectFolderDialogAsync(InteractionContext<MainWindowViewModel, string?> interaction)
    {
        var dialog = new OpenFolderDialog();
        var result = await dialog.ShowAsync(this);
        interaction.SetOutput(result);
    }

    private async Task ShowYesNoDialogAsync(InteractionContext<string, bool> interaction)
    {
        var dialog = new YesNoDialog();
        var model = new YesNoDialogViewModel
        {
            Prompt = interaction.Input
        };
        dialog.DataContext = model;
        var result = await dialog.ShowDialog<bool>(this);
        interaction.SetOutput(result);
    }

    private async Task ShowEditPrimitiveDialogAsync(InteractionContext<EditPrimitiveDialogViewModel, bool> interaction)
    {
        var dialog = new EditPrimitiveDialog
        {
            DataContext = interaction.Input
        };
        var result = await dialog.ShowDialog<bool>(this);
        interaction.SetOutput(result);
    }
}