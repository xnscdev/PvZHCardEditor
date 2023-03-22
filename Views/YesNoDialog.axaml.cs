using System;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using PvZHCardEditor.ViewModels;
using ReactiveUI;

namespace PvZHCardEditor.Views;

public partial class YesNoDialog : ReactiveWindow<YesNoDialogViewModel>
{
    public YesNoDialog()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        this.WhenActivated(d =>
        {
            d(ViewModel!.YesCommand.Subscribe(r => Close(r)));
            d(ViewModel!.NoCommand.Subscribe(r => Close(r)));
        });
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}