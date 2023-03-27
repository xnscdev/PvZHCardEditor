using System.Collections.Generic;
using System.Linq;
using PvZHCardEditor.Models;
using ReactiveUI;

namespace PvZHCardEditor.ViewModels;

public abstract class EditComponentDialogViewModel : EditDialogViewModel
{
    public abstract string ComponentValue { get; set; }
}

public class EditComponentDialogViewModel<T> : EditComponentDialogViewModel where T : EntityComponentBase
{
    private string _componentType = string.Empty;

    public EditComponentDialogViewModel()
    {
        TypeComboBoxOptions = GameDataManager.GetComponentTypes<T>().Select(EntityComponentBase.GetDisplayTypeString);
    }

    private static string ObjectTypeString => typeof(T) == typeof(EntityComponent) ? "Component" : "Query";

    public string Prompt => $"{ObjectTypeString} type";

    public override string ComponentValue
    {
        get => _componentType;
        set => this.RaiseAndSetIfChanged(ref _componentType, value);
    }

    public IEnumerable<string> TypeComboBoxOptions { get; }
}