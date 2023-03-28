using System.Linq;
using System.Reactive;
using Avalonia.Collections;
using PvZHCardEditor.Models;
using ReactiveUI;

namespace PvZHCardEditor.ViewModels;

public class EditObjectDialogViewModel<T> : EditDialogViewModel where T : ComponentValue, new()
{
    private string _key = string.Empty;
    private AvaloniaList<ComponentProperty> _properties = null!;
    private ComponentProperty? _selected;

    public EditObjectDialogViewModel()
    {
        var selection =
            this.WhenAnyValue(x => x.Selected, selected => selected != null && Properties.Contains(selected));
        AddCommand = ReactiveCommand.Create(DoAdd);
        RemoveCommand = ReactiveCommand.Create(DoRemove, selection);
    }

    public ReactiveCommand<Unit, Unit> AddCommand { get; }
    public ReactiveCommand<Unit, Unit> RemoveCommand { get; }

    public string Key
    {
        get => _key;
        set => this.RaiseAndSetIfChanged(ref _key, value);
    }

    public AvaloniaList<ComponentProperty> Properties
    {
        get => _properties;
        set => this.RaiseAndSetIfChanged(ref _properties, value);
    }

    public ComponentProperty? Selected
    {
        get => _selected;
        set => this.RaiseAndSetIfChanged(ref _selected, value);
    }

    private void DoAdd()
    {
        if (string.IsNullOrWhiteSpace(Key) || Properties.Any(p => p.Key == Key))
            return;
        var item = new ComponentProperty(Key, new T());
        Properties.Add(item);
        Selected = item;
        Key = string.Empty;
    }

    private void DoRemove()
    {
        Properties.Remove(Selected!);
    }
}