using ReactiveUI;

namespace PvZHCardEditor.Models;

public class ComponentProperty : ReactiveObject
{
    public ComponentProperty(string key, ComponentValue value)
    {
        Key = key;
        Value = value;
        Value.PropertyChanged += (_, _) => this.RaisePropertyChanged(nameof(Value));
    }

    public string Key { get; }

    public ComponentValue Value { get; }

    public bool IsExpanded
    {
        get => Value.IsExpanded;
        set => Value.IsExpanded = value;
    }

    public string TitleText => Value.Text == null ? Key : $"{Key} = {Value.Text}";
    public FullObservableCollection<ComponentProperty> Children => Value.Children;
}