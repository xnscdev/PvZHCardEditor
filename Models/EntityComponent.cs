using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PvZHCardEditor.ViewModels;
using ReactiveUI;

namespace PvZHCardEditor.Models;

#region Abstract classes

public abstract class EntityComponent : EntityComponentBase
{
    public override bool IsQuery => false;
}

[DataContract]
public abstract class TraitComponent : EntityComponent
{
    protected TraitComponent() : this(new TraitCounters())
    {
    }

    protected TraitComponent(TraitCounters counters)
    {
        Counters = counters;
        Counters.WhenAnyValue(x => x.Children).Subscribe(_ => this.RaisePropertyChanged(nameof(Children)));
    }

    [DataMember] public TraitCounters Counters { get; }

    public override FullObservableCollection<ComponentProperty> Children => Counters.Children;
}

[DataContract]
public abstract class QueryComponent : EntityComponent
{
    protected QueryComponent() : this(new ComponentWrapper<EntityQuery>())
    {
    }

    protected QueryComponent(ComponentWrapper<EntityQuery> query)
    {
        Query = query;
        Children = this.CreateReactiveProperties((nameof(Query), Query));
    }

    [DataMember] public ComponentWrapper<EntityQuery> Query { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

#endregion

[DataContract]
public class ActiveTargetsComponent : EntityComponent
{
}

[DataContract]
public class AquaticComponent : TraitComponent
{
    public AquaticComponent()
    {
    }

    [JsonConstructor]
    public AquaticComponent(TraitCounters counters) : base(counters)
    {
    }
}

[DataContract]
public class ArmorComponent : EntityComponent
{
    public ArmorComponent() : this(new BaseValueWrapper<int> { BaseValue = new ComponentPrimitive<int>(0) })
    {
    }

    [JsonConstructor]
    public ArmorComponent(BaseValueWrapper<int> armorAmount)
    {
        ArmorAmount = armorAmount;
        Children = this.CreateReactiveProperties((nameof(ArmorAmount), ArmorAmount.BaseValue));
    }

    [DataMember] public BaseValueWrapper<int> ArmorAmount { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class AttackComponent : EntityComponent
{
    public AttackComponent() : this(new BaseValueWrapper<int> { BaseValue = new ComponentPrimitive<int>(0) })
    {
    }

    [JsonConstructor]
    public AttackComponent(BaseValueWrapper<int> attackValue)
    {
        AttackValue = attackValue;
        Children = this.CreateReactiveProperties((nameof(AttackValue), AttackValue.BaseValue));
    }

    [DataMember] public BaseValueWrapper<int> AttackValue { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class AttackInLaneEffectDescriptor : EntityComponent
{
    public AttackInLaneEffectDescriptor() : this(0)
    {
    }

    [JsonConstructor]
    public AttackInLaneEffectDescriptor(int damageAmount)
    {
        DamageAmount = new ComponentPrimitive<int>(damageAmount);
        Children = this.CreateReactiveProperties((nameof(DamageAmount), DamageAmount));
    }

    [DataMember] public ComponentPrimitive<int> DamageAmount { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class AttackOverrideComponent : EntityComponent
{
}

[DataContract]
public class AttacksInAllLanesComponent : EntityComponent
{
}

[DataContract]
public class AttacksOnlyInAdjacentLanesComponent : EntityComponent
{
}

[DataContract]
public class BoardAbilityComponent : EntityComponent
{
}

[DataContract]
public class BuffEffectDescriptor : EntityComponent
{
    public BuffEffectDescriptor() : this(0, 0, "Permanent")
    {
    }

    [JsonConstructor]
    public BuffEffectDescriptor(int attackAmount, int healthAmount, string buffDuration)
    {
        AttackAmount = new ComponentPrimitive<int>(attackAmount);
        HealthAmount = new ComponentPrimitive<int>(healthAmount);
        BuffDuration = new ComponentPrimitive<string>(buffDuration);
        Children = this.CreateReactiveProperties(
            (nameof(AttackAmount), AttackAmount),
            (nameof(HealthAmount), HealthAmount),
            (nameof(BuffDuration), BuffDuration));
    }

    [DataMember] public ComponentPrimitive<int> AttackAmount { get; }
    [DataMember] public ComponentPrimitive<int> HealthAmount { get; }
    [DataMember] public ComponentPrimitive<string> BuffDuration { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class BuffTrigger : EntityComponent
{
}

[DataContract]
public class BurstComponent : EntityComponent
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
        Children = this.CreateReactiveProperties((nameof(Guid), Guid));
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
        Children = this.CreateReactiveProperties(
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
public class SelfEntityFilter : QueryComponent
{
    public SelfEntityFilter()
    {
    }

    [JsonConstructor]
    public SelfEntityFilter(ComponentWrapper<EntityQuery> query) : base(query)
    {
    }
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
public class TraitCounters : ComponentValue
{
    public TraitCounters() : this(true,
        new ComponentList<TraitCounter>(new FullObservableCollection<TraitCounter> { new() }))
    {
    }

    [JsonConstructor]
    public TraitCounters(bool isPersistent, ComponentList<TraitCounter> counters)
    {
        IsPersistent = new ComponentPrimitive<bool>(isPersistent);
        Counters = counters;
        Children = this.CreateReactiveProperties(
            (nameof(IsPersistent), IsPersistent),
            (nameof(Counters), Counters));
    }

    [DataMember] public ComponentPrimitive<bool> IsPersistent { get; }
    [DataMember] public ComponentList<TraitCounter> Counters { get; }

    public override string? Text => null;
    public override FullObservableCollection<ComponentProperty> Children { get; }

    public override Task Edit(MainWindowViewModel model)
    {
        return Task.CompletedTask;
    }
}

[DataContract]
public class TraitCounter : ComponentValue
{
    public TraitCounter() : this(-1, 0, 0)
    {
    }

    [JsonConstructor]
    public TraitCounter(int sourceId, int duration, int value)
    {
        SourceId = new ComponentPrimitive<int>(sourceId);
        Duration = new ComponentPrimitive<int>(duration);
        Value = new ComponentPrimitive<int>(value);
        Children = this.CreateReactiveProperties(
            (nameof(SourceId), SourceId),
            (nameof(Duration), Duration),
            (nameof(Value), Value));
    }

    [DataMember] public ComponentPrimitive<int> SourceId { get; }
    [DataMember] public ComponentPrimitive<int> Duration { get; }
    [DataMember] public ComponentPrimitive<int> Value { get; }

    public override string? Text => null;
    public override FullObservableCollection<ComponentProperty> Children { get; }

    public override Task Edit(MainWindowViewModel model)
    {
        return Task.CompletedTask;
    }
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