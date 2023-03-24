using System;
using ReactiveUI;

namespace PvZHCardEditor.Models;

public class ComponentProperty : ReactiveObject
{
    private string _key;
    private ComponentValue _value;

    public ComponentProperty(string key, ComponentValue value)
    {
        _key = key;
        _value = value;
        Value.WhenAnyValue(x => x.Children).Subscribe(_ => this.RaisePropertyChanged(nameof(Value)));
    }

    public string Key
    {
        get => _key;
        set => this.RaiseAndSetIfChanged(ref _key, value);
    }

    public ComponentValue Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }

    public string TitleText => Value.Text == null ? Key : $"{Key} = {Value.Text}";
    public FullObservableCollection<ComponentProperty> Children => Value.Children;
}