using System;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using PvZHCardEditor.ViewModels;
using ReactiveUI;

namespace PvZHCardEditor.Views;

public partial class EditPrimitiveDialog : ReactiveWindow<EditPrimitiveDialogViewModel>
{
    public EditPrimitiveDialog()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        this.WhenActivated(d =>
        {
            d(ViewModel!.EditCommand.Subscribe(r => Close(r)));
            d(ViewModel!.CancelCommand.Subscribe(r => Close(r)));
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}