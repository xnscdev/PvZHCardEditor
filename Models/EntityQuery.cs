using System.Runtime.Serialization;

namespace PvZHCardEditor.Models;

public abstract class EntityQuery : EntityComponentBase
{
    public override bool IsQuery => true;
}

[DataContract]
public class CompositeAllQuery : EntityQuery
{
}

[DataContract]
public class HasComponentQuery : EntityQuery
{
    public HasComponentQuery(ComponentTypeString componentType)
    {
        ComponentType = componentType;
        Children = this.CreateReactiveProperties((nameof(ComponentType), ComponentType));
    }

    [DataMember] public ComponentTypeString ComponentType { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}