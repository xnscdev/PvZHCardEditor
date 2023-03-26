using System.Runtime.Serialization;

namespace PvZHCardEditor.Models;

public abstract class EntityQuery : EntityComponentBase
{
    protected override bool IsQuery => true;
}

[DataContract]
public class HasComponentQuery : EntityQuery
{
    public HasComponentQuery(ComponentTypeString componentType)
    {
        ComponentType = componentType;
        Children = CreateProperties((nameof(ComponentType), ComponentType));
    }

    [DataMember] public ComponentTypeString ComponentType { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}