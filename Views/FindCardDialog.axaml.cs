using System;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using PvZHCardEditor.ViewModels;
using ReactiveUI;

namespace PvZHCardEditor.Views;

public partial class FindCardDialog : ReactiveWindow<FindCardDialogViewModel>
{
    public FindCardDialog()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        this.WhenActivated(d => { d(ViewModel!.CloseCommand.Subscribe(r => Close(r))); });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}