using System;
using System.Collections.Specialized;
using ReactiveUI;

namespace PvZHCardEditor.Models;

public abstract class ComponentRenderable : ReactiveObject
{
    public abstract string? Text { get; }
    public abstract FullObservableCollection<ComponentRenderable> Children { get; }
}

public class ComponentProperty : ReactiveObject
{
    private string _key;
    private ComponentRenderable _value;

    public string Key
    {
        get => _key;
        set => this.RaiseAndSetIfChanged(ref _key, value);
    }

    public ComponentRenderable Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }

    public string TitleText => Value.Text == null ? Key : $"{Key} = {Value.Text}";
    public FullObservableCollection<ComponentRenderable> Children => Value.Children;

    public ComponentProperty(string key, ComponentRenderable value)
    {
        _key = key;
        _value = value;
        Value.WhenAnyValue(x => x.Children).Subscribe(_ => this.RaisePropertyChanged(nameof(Value)));
    }
}

public class ComponentList<T> : ComponentRenderable
{
    public FullObservableCollection<ComponentRenderable> Elements { get; }

    public override string? Text => null;
    public override FullObservableCollection<ComponentRenderable> Children => Elements;

    public ComponentList(FullObservableCollection<ComponentRenderable> elements)
    {
        Elements = elements;
        Elements.CollectionChanged += OnCollectionChanged;
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        this.RaisePropertyChanged(nameof(Elements));
    }
}