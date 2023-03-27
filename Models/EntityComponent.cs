using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PvZHCardEditor.ViewModels;
using ReactiveUI;

namespace PvZHCardEditor.Models;

public abstract class EntityComponent : EntityComponentBase
{
    public override bool IsQuery => false;
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
    public CardComponent() : this(0)
    {
    }

    [JsonConstructor]
    public CardComponent(int guid)
    {
        Guid = new ComponentPrimitive<int>(guid);
        Children = CreateProperties((nameof(Guid), Guid));
    }

    [DataMember] public ComponentPrimitive<int> Guid { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class DamageEffectDescriptor : EntityComponent
{
}

[DataContract]
public class DiscardFromPlayTrigger : EntityComponent
{
}

[DataContract]
public class EffectEntitiesDescriptor : EntityComponent
{
    public EffectEntitiesDescriptor() : this(new ComponentList<EffectEntity>())
    {
    }
    
    [JsonConstructor]
    public EffectEntitiesDescriptor(ComponentList<EffectEntity> entities)
    {
        Entities = entities;
        Entities.WhenAnyValue(x => x.Children).Subscribe(_ => this.RaisePropertyChanged(nameof(Children)));
    }

    [DataMember]
    [JsonProperty(PropertyName = "entities")]
    public ComponentList<EffectEntity> Entities { get; }

    public override FullObservableCollection<ComponentProperty> Children => Entities.Children;
    public override ComponentValue EditHandler => Entities;
}

[DataContract]
public class EffectEntityGrouping : EntityComponent
{
}

[DataContract]
public class GrantTriggeredAbilityEffectDescriptor : EntityComponent
{
}

[DataContract]
public class HealthComponent : EntityComponent
{
    public HealthComponent() : this(new BaseValueWrapper<int> { BaseValue = new ComponentPrimitive<int>(0) }, 0)
    {
    }

    [JsonConstructor]
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
public class PrimaryTargetFilter : EntityComponent
{
}

[DataContract]
public class SubtypesComponent : EntityComponent
{
    public SubtypesComponent() : this(new ComponentList<ComponentPrimitive<int>>())
    {
    }

    [JsonConstructor]
    public SubtypesComponent(ComponentList<ComponentPrimitive<int>> subtypes)
    {
        Subtypes = subtypes;
        Subtypes.WhenAnyValue(x => x.Children).Subscribe(_ => this.RaisePropertyChanged(nameof(Children)));
    }

    [DataMember]
    [JsonProperty(PropertyName = "subtypes")]
    public ComponentList<ComponentPrimitive<int>> Subtypes { get; }

    public override FullObservableCollection<ComponentProperty> Children => Subtypes.Children;
    public override ComponentValue EditHandler => Subtypes;
}

[DataContract]
public class TriggerTargetFilter : EntityComponent
{
}

#region Helper types

public struct BaseValueWrapper<T>
{
    public ComponentPrimitive<T> BaseValue;
}

[DataContract]
public class EffectEntity : ComponentValue
{
    public EffectEntity() : this(new ComponentList<ComponentWrapper<EntityComponent>>())
    {
    }

    [JsonConstructor]
    public EffectEntity(ComponentList<ComponentWrapper<EntityComponent>> components)
    {
        Components = components;
        Components.WhenAnyValue(x => x.Children).Subscribe(_ => this.RaisePropertyChanged(nameof(Children)));
    }

    [DataMember]
    [JsonProperty(PropertyName = "components")]
    public ComponentList<ComponentWrapper<EntityComponent>> Components { get; }

    public override string? Text => Components.Text;
    public override FullObservableCollection<ComponentProperty> Children => Components.Children;

    public override bool IsExpanded
    {
        get => Components.IsExpanded;
        set => Components.IsExpanded = value;
    }

    public override async Task Edit(MainWindowViewModel model)
    {
        await Components.Edit(model);
    }
}

#endregion