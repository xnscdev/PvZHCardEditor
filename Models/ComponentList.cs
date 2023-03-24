using System.Collections.Specialized;
using System.Linq;
using ReactiveUI;

namespace PvZHCardEditor.Models;

public class ComponentList<T> : ComponentRenderable
{
    public ComponentList(FullObservableCollection<ComponentRenderable> elements)
    {
        Elements = elements;
        Elements.CollectionChanged += OnCollectionChanged;
    }

    public FullObservableCollection<ComponentRenderable> Elements { get; }

    public override string? Text => null;

    public override FullObservableCollection<ComponentProperty> Children =>
        new(Elements.Select((e, i) => new ComponentProperty($"[{i}]", e)));

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        this.RaisePropertyChanged(nameof(Elements));
    }
}