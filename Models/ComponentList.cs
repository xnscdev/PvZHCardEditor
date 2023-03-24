using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;

namespace PvZHCardEditor.Models;

public class ComponentList<T> : ComponentValue
{
    public ComponentList(FullObservableCollection<ComponentValue> elements)
    {
        Elements = elements;
        Elements.CollectionChanged += OnCollectionChanged;
    }

    public FullObservableCollection<ComponentValue> Elements { get; }

    public override string? Text => null;

    public override FullObservableCollection<ComponentProperty> Children =>
        new(Elements.Select((e, i) => new ComponentProperty($"[{i}]", e)));

    public override Task Edit()
    {
        Console.WriteLine("Implement GUI to allow reordering, inserting, removing elements");
        throw new NotImplementedException();
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        this.RaisePropertyChanged(nameof(Elements));
    }
}