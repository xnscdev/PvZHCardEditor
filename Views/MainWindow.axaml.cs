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
        // TODO: Fix dialog not appearing
        Closing += async (_, args) =>
        {
            var dialog = new YesNoDialog();
            var model = new YesNoDialogViewModel
            {
                Prompt = "Save changes before closing?"
            };
            dialog.DataContext = model;
            var result = await dialog.ShowDialog<bool>(this);
            if (result && !ViewModel!.SaveWorkspace())
                args.Cancel = true;
        };
        this.WhenActivated(d =>
        {
            d(ViewModel!.ShowSelectFolderDialog.RegisterHandler(ShowSelectFolderDialogAsync));
            d(ViewModel!.ShowYesNoDialog.RegisterHandler(ShowYesNoDialogAsync));
            d(ViewModel!.ShowEditPrimitiveDialog.RegisterHandler(async interaction =>
                await ShowDialog<EditPrimitiveDialog, EditDialogViewModel, bool>(interaction)));
            d(ViewModel!.ShowEditListDialog.RegisterHandler(async interaction =>
                await ShowDialog<EditListDialog, EditDialogViewModel, bool>(interaction)));
            d(ViewModel!.ShowEditObjectDialog.RegisterHandler(async interaction =>
                await ShowDialog<EditObjectDialog, EditDialogViewModel, bool>(interaction)));
            d(ViewModel!.ShowEditComponentDialog.RegisterHandler(async interaction =>
                await ShowDialog<EditComponentDialog, EditDialogViewModel, bool>(interaction)));
            d(ViewModel!.ShowEditOptionalComponentDialog.RegisterHandler(async interaction =>
                await ShowDialog<EditOptionalComponentDialog, EditDialogViewModel, bool>(interaction)));
            d(ViewModel!.ShowEditDescriptionDialog.RegisterHandler(async interaction =>
                await ShowDialog<EditDescriptionDialog, EditDialogViewModel, bool>(interaction)));
            d(ViewModel!.ShowEditStatsDialog.RegisterHandler(async interaction =>
                await ShowDialog<EditStatsDialog, EditDialogViewModel, bool>(interaction)));
            d(ViewModel!.ShowEditTribesDialog.RegisterHandler(async interaction =>
                await ShowDialog<EditTribesDialog, EditDialogViewModel, bool>(interaction)));
            d(ViewModel!.ShowEditAttributesDialog.RegisterHandler(async interaction =>
                await ShowDialog<EditAttributesDialog, EditDialogViewModel, bool>(interaction)));
            d(ViewModel!.ShowFindCardDialog.RegisterHandler(async interaction =>
                await ShowDialog<FindCardDialog, FindCardDialogViewModel, bool>(interaction)));
            d(ViewModel!.ShowCreateCardDialog.RegisterHandler(async interaction =>
                await ShowDialog<CreateCardDialog, EditDialogViewModel, bool>(interaction)));
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

    private async Task ShowDialog<TDialog, TIn, TOut>(InteractionContext<TIn, TOut> interaction)
        where TDialog : Window, new()
    {
        var dialog = new TDialog
        {
            DataContext = interaction.Input
        };
        var result = await dialog.ShowDialog<TOut>(this);
        interaction.SetOutput(result);
    }
}