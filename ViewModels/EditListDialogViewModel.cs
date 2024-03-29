using System.Reactive;
using Avalonia.Collections;
using Newtonsoft.Json;
using PvZHCardEditor.Models;
using ReactiveUI;

namespace PvZHCardEditor.ViewModels;

public class EditListDialogViewModel<T> : EditDialogViewModel where T : ComponentValue, new()
{
    private AvaloniaList<T> _elements = null!;
    private T? _selected;

    public EditListDialogViewModel()
    {
        var selection = this.WhenAnyValue(x => x.Selected, selected => selected != null && Elements.Contains(selected));
        AddCommand = ReactiveCommand.Create(DoAdd);
        RemoveCommand = ReactiveCommand.Create(DoRemove, selection);
        MoveUpCommand = ReactiveCommand.Create(DoMoveUp, selection);
        MoveDownCommand = ReactiveCommand.Create(DoMoveDown, selection);
        CloneCommand = ReactiveCommand.Create(DoClone, selection);
    }

    public ReactiveCommand<Unit, Unit> AddCommand { get; }
    public ReactiveCommand<Unit, Unit> RemoveCommand { get; }
    public ReactiveCommand<Unit, Unit> MoveUpCommand { get; }
    public ReactiveCommand<Unit, Unit> MoveDownCommand { get; }
    public ReactiveCommand<Unit, Unit> CloneCommand { get; }

    public AvaloniaList<T> Elements
    {
        get => _elements;
        set => this.RaiseAndSetIfChanged(ref _elements, value);
    }

    public T? Selected
    {
        get => _selected;
        set => this.RaiseAndSetIfChanged(ref _selected, value);
    }

    private void DoAdd()
    {
        var index = Selected != null && Elements.Contains(Selected) ? Elements.IndexOf(Selected) : Elements.Count;
        var item = new T();
        Elements.Insert(index, item);
        Selected = item;
    }

    private void DoRemove()
    {
        Elements.Remove(Selected!);
    }

    private void DoMoveUp()
    {
        var index = Elements.IndexOf(Selected!);
        if (index > 0)
            Elements.Move(index, index - 1);
    }

    private void DoMoveDown()
    {
        var index = Elements.IndexOf(Selected!);
        if (index > -1 && index < Elements.Count - 1)
            Elements.Move(index, index + 1);
    }

    private void DoClone()
    {
        var index = Elements.IndexOf(Selected!);
        var cloned = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(Selected))!;
        Elements.Insert(index + 1, cloned);
    }
}