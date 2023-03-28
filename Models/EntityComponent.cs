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
public abstract class DamageAmountComponent : EntityComponent
{
    protected DamageAmountComponent() : this(0)
    {
    }

    [JsonConstructor]
    protected DamageAmountComponent(int damageAmount)
    {
        DamageAmount = new ComponentPrimitive<int>(damageAmount);
        Children = this.CreateReactiveProperties((nameof(DamageAmount), DamageAmount));
    }

    [DataMember] public ComponentPrimitive<int> DamageAmount { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
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
public class AttackInLaneEffectDescriptor : DamageAmountComponent
{
    public AttackInLaneEffectDescriptor()
    {
    }

    [JsonConstructor]
    public AttackInLaneEffectDescriptor(int damageAmount) : base(damageAmount)
    {
    }
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
public class ChargeBlockMeterEffectDescriptor : EntityComponent
{
    public ChargeBlockMeterEffectDescriptor() : this(0)
    {
    }

    [JsonConstructor]
    public ChargeBlockMeterEffectDescriptor(int chargeAmount)
    {
        ChargeAmount = new ComponentPrimitive<int>(chargeAmount);
        Children = this.CreateReactiveProperties((nameof(ChargeAmount), ChargeAmount));
    }

    [DataMember] public ComponentPrimitive<int> ChargeAmount { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class CombatEndTrigger : EntityComponent
{
}

[DataContract]
public class ContinuousComponent : EntityComponent
{
}

[DataContract]
public class CopyCardEffectDescriptor : EntityComponent
{
    public CopyCardEffectDescriptor() : this(true, false, true)
    {
    }

    [JsonConstructor]
    public CopyCardEffectDescriptor(bool grantTeamup, bool forceFaceDown, bool createInFront)
    {
        GrantTeamup = new ComponentPrimitive<bool>(grantTeamup);
        ForceFaceDown = new ComponentPrimitive<bool>(forceFaceDown);
        CreateInFront = new ComponentPrimitive<bool>(createInFront);
        Children = this.CreateReactiveProperties(
            (nameof(GrantTeamup), GrantTeamup),
            (nameof(ForceFaceDown), ForceFaceDown),
            (nameof(CreateInFront), CreateInFront));
    }

    [DataMember] public ComponentPrimitive<bool> GrantTeamup { get; }
    [DataMember] public ComponentPrimitive<bool> ForceFaceDown { get; }
    [DataMember] public ComponentPrimitive<bool> CreateInFront { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class CopyStatsEffectDescriptor : EntityComponent
{
}

[DataContract]
public class CreateCardEffectDescriptor : EntityComponent
{
    public CreateCardEffectDescriptor() : this(0, false)
    {
    }

    [JsonConstructor]
    public CreateCardEffectDescriptor(int cardGuid, bool forceFaceDown)
    {
        CardGuid = new ComponentPrimitive<int>(cardGuid);
        ForceFaceDown = new ComponentPrimitive<bool>(forceFaceDown);
        Children = this.CreateReactiveProperties(
            (nameof(CardGuid), CardGuid),
            (nameof(ForceFaceDown), ForceFaceDown));
    }

    [DataMember] public ComponentPrimitive<int> CardGuid { get; }
    [DataMember] public ComponentPrimitive<bool> ForceFaceDown { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class CreateCardFromSubsetEffectDescriptor : EntityComponent
{
    public CreateCardFromSubsetEffectDescriptor() : this(false, new ComponentWrapper<EntityQuery>())
    {
    }

    [JsonConstructor]
    public CreateCardFromSubsetEffectDescriptor(bool forceFaceDown, ComponentWrapper<EntityQuery> subsetQuery)
    {
        ForceFaceDown = new ComponentPrimitive<bool>(forceFaceDown);
        SubsetQuery = subsetQuery;
        Children = this.CreateReactiveProperties(
            (nameof(ForceFaceDown), ForceFaceDown),
            (nameof(SubsetQuery), SubsetQuery));
    }

    [DataMember] public ComponentPrimitive<bool> ForceFaceDown { get; }
    [DataMember] public ComponentWrapper<EntityQuery> SubsetQuery { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class CreateCardInDeckEffectDescriptor : EntityComponent
{
    public CreateCardInDeckEffectDescriptor() : this(0, 0, "Random")
    {
    }

    [JsonConstructor]
    public CreateCardInDeckEffectDescriptor(int cardGuid, int amountToCreate, string deckPosition)
    {
        CardGuid = new ComponentPrimitive<int>(cardGuid);
        AmountToCreate = new ComponentPrimitive<int>(amountToCreate);
        DeckPosition = new ComponentPrimitive<string>(deckPosition);
        Children = this.CreateReactiveProperties(
            (nameof(CardGuid), CardGuid),
            (nameof(AmountToCreate), AmountToCreate),
            (nameof(DeckPosition), DeckPosition));
    }

    [DataMember] public ComponentPrimitive<int> CardGuid { get; }
    [DataMember] public ComponentPrimitive<int> AmountToCreate { get; }
    [DataMember] public ComponentPrimitive<string> DeckPosition { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class CreateInFrontComponent : EntityComponent
{
}

[DataContract]
public class DamageEffectDescriptor : DamageAmountComponent
{
    public DamageEffectDescriptor()
    {
    }

    [JsonConstructor]
    public DamageEffectDescriptor(int damageAmount) : base(damageAmount)
    {
    }
}

[DataContract]
public class DamageEffectRedirectorComponent : EntityComponent
{
}

[DataContract]
public class DamageEffectRedirectorDescriptor : EntityComponent
{
}

[DataContract]
public class DamageTrigger : EntityComponent
{
}

[DataContract]
public class DeadlyComponent : TraitComponent
{
    public DeadlyComponent()
    {
    }

    [JsonConstructor]
    public DeadlyComponent(TraitCounters counters) : base(counters)
    {
    }
}

[DataContract]
public class DestroyCardEffectDescriptor : EntityComponent
{
}

[DataContract]
public class DestroyCardTrigger : EntityComponent
{
}

[DataContract]
public class DiscardFromPlayTrigger : EntityComponent
{
}

[DataContract]
public class DrawCardEffectDescriptor : EntityComponent
{
    public DrawCardEffectDescriptor() : this(0)
    {
    }

    [JsonConstructor]
    public DrawCardEffectDescriptor(int drawAmount)
    {
        DrawAmount = new ComponentPrimitive<int>(drawAmount);
        Children = this.CreateReactiveProperties((nameof(DrawAmount), DrawAmount));
    }

    [DataMember] public ComponentPrimitive<int> DrawAmount { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class DrawCardFromSubsetEffectDescriptor : EntityComponent
{
    public DrawCardFromSubsetEffectDescriptor() : this(new ComponentWrapper<EntityQuery>(), 0)
    {
    }

    [JsonConstructor]
    public DrawCardFromSubsetEffectDescriptor(ComponentWrapper<EntityQuery> subsetQuery, int drawAmount)
    {
        SubsetQuery = subsetQuery;
        DrawAmount = new ComponentPrimitive<int>(drawAmount);
        Children = this.CreateReactiveProperties(
            (nameof(SubsetQuery), SubsetQuery),
            (nameof(DrawAmount), DrawAmount));
    }

    [DataMember] public ComponentWrapper<EntityQuery> SubsetQuery { get; }
    [DataMember] public ComponentPrimitive<int> DrawAmount { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class DrawCardFromSubsetTrigger : EntityComponent
{
}

[DataContract]
public class DrawCardTrigger : EntityComponent
{
}

[DataContract]
public class DrawnCardCostMultiplier : EntityComponent
{
    public DrawnCardCostMultiplier() : this(0)
    {
    }

    [JsonConstructor]
    public DrawnCardCostMultiplier(int divider)
    {
        Divider = new ComponentPrimitive<int>(divider);
        Children = this.CreateReactiveProperties((nameof(Divider), Divider));
    }

    [DataMember] public ComponentPrimitive<int> Divider { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
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
    public EffectEntityGrouping() : this(0)
    {
    }

    [JsonConstructor]
    public EffectEntityGrouping(int abilityGroupId)
    {
        AbilityGroupId = new ComponentPrimitive<int>(abilityGroupId);
        Children = this.CreateReactiveProperties((nameof(AbilityGroupId), AbilityGroupId));
    }

    [DataMember] public ComponentPrimitive<int> AbilityGroupId { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class EffectValueCondition : EntityComponent
{
    public EffectValueCondition() : this("TotalBuffAmount", "GreaterOrEqual", 1)
    {
    }

    [JsonConstructor]
    public EffectValueCondition(string effectValue, string comparisonOperator, int valueAmount)
    {
        EffectValue = new ComponentPrimitive<string>(effectValue);
        ComparisonOperator = new ComponentPrimitive<string>(comparisonOperator);
        ValueAmount = new ComponentPrimitive<int>(valueAmount);
        Children = this.CreateReactiveProperties(
            (nameof(EffectValue), EffectValue),
            (nameof(ComparisonOperator), ComparisonOperator),
            (nameof(ValueAmount), ValueAmount));
    }

    [DataMember] public ComponentPrimitive<string> EffectValue { get; }
    [DataMember] public ComponentPrimitive<string> ComparisonOperator { get; }
    [DataMember] public ComponentPrimitive<int> ValueAmount { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class EffectValueDescriptor : EntityComponent
{
    public EffectValueDescriptor() : this(new ComponentObject<ComponentPrimitive<string>>())
    {
    }

    [JsonConstructor]
    public EffectValueDescriptor(ComponentObject<ComponentPrimitive<string>> destToSourceMap)
    {
        DestToSourceMap = destToSourceMap;
        Children = this.CreateReactiveProperties((nameof(DestToSourceMap), DestToSourceMap));
    }

    [DataMember] public ComponentObject<ComponentPrimitive<string>> DestToSourceMap { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class GrantTriggeredAbilityEffectDescriptor : EntityComponent
{
}

[DataContract]
public class HealEffectDescriptor : EntityComponent
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
public class TriggerSourceFilter : EntityComponent
{
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