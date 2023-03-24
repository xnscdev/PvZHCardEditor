using ReactiveUI;

namespace PvZHCardEditor.Models;

public abstract class ComponentRenderable : ReactiveObject
{
    public abstract string? Text { get; }
    public abstract FullObservableCollection<ComponentProperty> Children { get; }
}