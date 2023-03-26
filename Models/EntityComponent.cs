using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using ReactiveUI;

namespace PvZHCardEditor.Models;

public abstract class EntityComponent : EntityComponentBase
{
    protected override bool IsQuery => false;
}

[DataContract]
public class AquaticComponent : EntityComponent
{
}

[DataContract]
public class BoardAbilityComponent : EntityComponent
{
}

[DataContract]
public class CardComponent : EntityComponent
{
    public CardComponent(int guid)
    {
        Guid = new ComponentPrimitive<int>(guid);
        Children = CreateProperties((nameof(Guid), Guid));
    }

    [DataMember] public ComponentPrimitive<int> Guid { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class GrantTriggeredAbilityEffectDescriptor : EntityComponent
{
}

[DataContract]
public class HealthComponent : EntityComponent
{
    public HealthComponent(BaseValueWrapper<int> maxHealth, int currentDamage)
    {
        MaxHealth = maxHealth;
        CurrentDamage = new ComponentPrimitive<int>(currentDamage);
        Children = CreateProperties(
            (nameof(MaxHealth), MaxHealth.BaseValue),
            (nameof(CurrentDamage), CurrentDamage));
    }

    [DataMember] public BaseValueWrapper<int> MaxHealth { get; }
    [DataMember] public ComponentPrimitive<int> CurrentDamage { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class SubtypesComponent : EntityComponent
{
    public SubtypesComponent(ComponentList<ComponentPrimitive<int>> subtypes)
    {
        Subtypes = subtypes;
        Subtypes.WhenAnyValue(x => x.Children).Subscribe(_ => this.RaisePropertyChanged(nameof(Children)));
    }

    [DataMember]
    [JsonProperty(PropertyName = "subtypes")]
    public ComponentList<ComponentPrimitive<int>> Subtypes { get; }

    public override FullObservableCollection<ComponentProperty> Children => Subtypes.Children;
    protected override ComponentValue EditHandler => Subtypes;
}

public struct BaseValueWrapper<T>
{
    public ComponentPrimitive<T> BaseValue;
}