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
public class EnterBoardTrigger : EntityComponent
{
}

[DataContract]
public class EnvironmentComponent : EntityComponent
{
}

[DataContract]
public class EvolutionRestrictionComponent : QueryComponent
{
    public EvolutionRestrictionComponent()
    {
    }

    public EvolutionRestrictionComponent(ComponentWrapper<EntityQuery> query) : base(query)
    {
    }
}

[DataContract]
public class EvolvableComponent : EntityComponent
{
}

[DataContract]
public class ExtraAttackEffectDescriptor : EntityComponent
{
}

[DataContract]
public class ExtraAttackTrigger : EntityComponent
{
}

[DataContract]
public class FrenzyComponent : TraitComponent
{
    public FrenzyComponent()
    {
    }

    [JsonConstructor]
    public FrenzyComponent(TraitCounters counters) : base(counters)
    {
    }
}

[DataContract]
public class FromBurstComponent : EntityComponent
{
}

[DataContract]
public class GainSunEffectDescriptor : EntityComponent
{
    public GainSunEffectDescriptor() : this(0, "EndOfTurn", "Immediate")
    {
    }

    [JsonConstructor]
    public GainSunEffectDescriptor(int amount, string duration, string activationTime)
    {
        Amount = new ComponentPrimitive<int>(amount);
        Duration = new ComponentPrimitive<string>(duration);
        ActivationTime = new ComponentPrimitive<string>(activationTime);
        Children = this.CreateReactiveProperties(
            (nameof(Amount), Amount),
            (nameof(Duration), Duration),
            (nameof(ActivationTime), ActivationTime));
    }

    [DataMember] public ComponentPrimitive<int> Amount { get; }
    [DataMember] public ComponentPrimitive<string> Duration { get; }
    [DataMember] public ComponentPrimitive<string> ActivationTime { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class GrantAbilityEffectDescriptor : EntityComponent
{
    public GrantAbilityEffectDescriptor() : this(string.Empty, "Permanent", 0)
    {
    }

    [JsonConstructor]
    public GrantAbilityEffectDescriptor(string grantableAbilityType, string duration, int abilityValue)
    {
        GrantableAbilityType = new ComponentPrimitive<string>(grantableAbilityType);
        Duration = new ComponentPrimitive<string>(duration);
        AbilityValue = new ComponentPrimitive<int>(abilityValue);
        Children = this.CreateReactiveProperties(
            (nameof(GrantableAbilityType), GrantableAbilityType),
            (nameof(Duration), Duration),
            (nameof(AbilityValue), AbilityValue));
    }

    [DataMember] public ComponentPrimitive<string> GrantableAbilityType { get; }
    [DataMember] public ComponentPrimitive<string> Duration { get; }
    [DataMember] public ComponentPrimitive<int> AbilityValue { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class GrantTriggeredAbilityEffectDescriptor : EntityComponent
{
    public GrantTriggeredAbilityEffectDescriptor() : this(0, "None", 0)
    {
    }

    [JsonConstructor]
    public GrantTriggeredAbilityEffectDescriptor(int abilityGuid, string abilityValueType, int abilityValueAmount)
    {
        AbilityGuid = new ComponentPrimitive<int>(abilityGuid);
        AbilityValueType = new ComponentPrimitive<string>(abilityValueType);
        AbilityValueAmount = new ComponentPrimitive<int>(abilityValueAmount);
        Children = this.CreateReactiveProperties(
            (nameof(AbilityGuid), AbilityGuid),
            (nameof(AbilityValueType), AbilityValueType),
            (nameof(AbilityValueAmount), AbilityValueAmount));
    }

    [DataMember] public ComponentPrimitive<int> AbilityGuid { get; }
    [DataMember] public ComponentPrimitive<string> AbilityValueType { get; }
    [DataMember] public ComponentPrimitive<int> AbilityValueAmount { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class GrantedTriggeredAbilitiesComponent : EntityComponent
{
    public GrantedTriggeredAbilitiesComponent() : this(DefaultObject)
    {
    }

    [JsonConstructor]
    public GrantedTriggeredAbilitiesComponent(ComponentList<ComponentObject<ComponentPrimitive<int>>> a)
    {
        A = a;
        A.WhenAnyValue(x => x.Children).Subscribe(_ => this.RaisePropertyChanged(nameof(Children)));
    }

    private static ComponentList<ComponentObject<ComponentPrimitive<int>>> DefaultObject => new()
    {
        Elements =
        {
            new ComponentObject<ComponentPrimitive<int>>(new FullObservableCollection<ComponentProperty>
            {
                new("g", new ComponentPrimitive<int>()),
                new("vt", new ComponentPrimitive<int>()),
                new("va", new ComponentPrimitive<int>())
            })
        }
    };

    [DataMember]
    [JsonProperty(PropertyName = "a")]
    public ComponentList<ComponentObject<ComponentPrimitive<int>>> A { get; }

    public override FullObservableCollection<ComponentProperty> Children => A.Children;
    public override ComponentValue EditHandler => A;
}

[DataContract]
public class HealEffectDescriptor : EntityComponent
{
    public HealEffectDescriptor() : this(0)
    {
    }

    [JsonConstructor]
    public HealEffectDescriptor(int healAmount)
    {
        HealAmount = new ComponentPrimitive<int>(healAmount);
        Children = this.CreateReactiveProperties((nameof(HealAmount), HealAmount));
    }

    [DataMember] public ComponentPrimitive<int> HealAmount { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class HealTrigger : EntityComponent
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
public class HeraldEntitiesComponent : EntityComponent
{
    public HeraldEntitiesComponent() : this(
        new ComponentList<ComponentPrimitive<int>>(new FullObservableCollection<ComponentPrimitive<int>> { new() }))
    {
    }

    [JsonConstructor]
    public HeraldEntitiesComponent(ComponentList<ComponentPrimitive<int>> ids)
    {
        Ids = ids;
        Ids.WhenAnyValue(x => x.Children).Subscribe(_ => this.RaisePropertyChanged(nameof(Children)));
    }

    [DataMember]
    [JsonProperty(PropertyName = "ids")]
    public ComponentList<ComponentPrimitive<int>> Ids { get; }

    public override FullObservableCollection<ComponentProperty> Children => Ids.Children;
    public override ComponentValue EditHandler => Ids;
}

[DataContract]
public class HeroHealthMultiplier : EntityComponent
{
    public HeroHealthMultiplier() : this(string.Empty, 1)
    {
    }

    [JsonConstructor]
    public HeroHealthMultiplier(string faction, int divider)
    {
        Faction = new ComponentPrimitive<string>(faction);
        Divider = new ComponentPrimitive<int>(divider);
        Children = this.CreateReactiveProperties(
            (nameof(Faction), Faction),
            (nameof(Divider), Divider));
    }

    [DataMember] public ComponentPrimitive<string> Faction { get; }
    [DataMember] public ComponentPrimitive<int> Divider { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class LaneCombatEndTrigger : EntityComponent
{
}

[DataContract]
public class LaneCombatStartTrigger : EntityComponent
{
}

[DataContract]
public class MixedUpGravediggerEffectDescriptor : EntityComponent
{
}

[DataContract]
public class ModifySunCostEffectDescriptor : EntityComponent
{
    public ModifySunCostEffectDescriptor() : this(0, "Permanent")
    {
    }

    [JsonConstructor]
    public ModifySunCostEffectDescriptor(int sunCostAmount, string buffDuration)
    {
        SunCostAmount = new ComponentPrimitive<int>(sunCostAmount);
        BuffDuration = new ComponentPrimitive<string>(buffDuration);
        Children = this.CreateReactiveProperties(
            (nameof(SunCostAmount), SunCostAmount),
            (nameof(BuffDuration), BuffDuration));
    }

    [DataMember] public ComponentPrimitive<int> SunCostAmount { get; }
    [DataMember] public ComponentPrimitive<string> BuffDuration { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class MoveCardToLanesEffectDescriptor : EntityComponent
{
}

[DataContract]
public class MoveTrigger : EntityComponent
{
}

[DataContract]
public class MultishotComponent : EntityComponent
{
}

[DataContract]
public class OncePerGameCondition : EntityComponent
{
}

[DataContract]
public class OncePerTurnCondition : EntityComponent
{
}

[DataContract]
public class PersistsAfterTransformComponent : EntityComponent
{
}

[DataContract]
public class PlantsComponent : EntityComponent
{
}

[DataContract]
public class PlayTrigger : EntityComponent
{
}

[DataContract]
public class PlayerInfoCondition : EntityComponent
{
    public PlayerInfoCondition() : this(string.Empty, new ComponentWrapper<EntityQuery>())
    {
    }

    public PlayerInfoCondition(string faction, ComponentWrapper<EntityQuery> query)
    {
        Faction = new ComponentPrimitive<string>(faction);
        Query = query;
        Children = this.CreateReactiveProperties(
            (nameof(Faction), Faction),
            (nameof(Query), Query));
    }

    [DataMember] public ComponentPrimitive<string> Faction { get; }
    [DataMember] public ComponentWrapper<EntityQuery> Query { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class PlaysFaceDownComponent : EntityComponent
{
}

[DataContract]
public class PrimarySuperpowerComponent : EntityComponent
{
}

[DataContract]
public class PrimaryTargetFilter : EntityComponent
{
    public PrimaryTargetFilter() : this("All", 0, "All", "None", "None", "None",
        new OptionalComponentWrapper<EntityQuery>(), false, new ComponentWrapper<EntityQuery>())
    {
    }

    [JsonConstructor]
    public PrimaryTargetFilter(string selectionType, int numTargets, string targetScopeType,
        string targetScopeSortValue, string targetScopeSortMethod, string additionalTargetType,
        OptionalComponentWrapper<EntityQuery> additionalTargetQuery, bool onlyApplyEffectsOnAdditionalTargets,
        ComponentWrapper<EntityQuery> query)
    {
        SelectionType = new ComponentPrimitive<string>(selectionType);
        NumTargets = new ComponentPrimitive<int>(numTargets);
        TargetScopeType = new ComponentPrimitive<string>(targetScopeType);
        TargetScopeSortValue = new ComponentPrimitive<string>(targetScopeSortValue);
        TargetScopeSortMethod = new ComponentPrimitive<string>(targetScopeSortMethod);
        AdditionalTargetType = new ComponentPrimitive<string>(additionalTargetType);
        AdditionalTargetQuery = additionalTargetQuery;
        OnlyApplyEffectsOnAdditionalTargets = new ComponentPrimitive<bool>(onlyApplyEffectsOnAdditionalTargets);
        Query = query;
        Children = this.CreateReactiveProperties(
            (nameof(SelectionType), SelectionType),
            (nameof(NumTargets), NumTargets),
            (nameof(TargetScopeType), TargetScopeType),
            (nameof(TargetScopeSortValue), TargetScopeSortValue),
            (nameof(TargetScopeSortMethod), TargetScopeSortMethod),
            (nameof(AdditionalTargetType), AdditionalTargetType),
            (nameof(AdditionalTargetQuery), AdditionalTargetQuery),
            (nameof(OnlyApplyEffectsOnAdditionalTargets), OnlyApplyEffectsOnAdditionalTargets),
            (nameof(Query), Query));
    }

    [DataMember] public ComponentPrimitive<string> SelectionType { get; }
    [DataMember] public ComponentPrimitive<int> NumTargets { get; }
    [DataMember] public ComponentPrimitive<string> TargetScopeType { get; }
    [DataMember] public ComponentPrimitive<string> TargetScopeSortValue { get; }
    [DataMember] public ComponentPrimitive<string> TargetScopeSortMethod { get; }

    [DataMember] public ComponentPrimitive<string> AdditionalTargetType { get; }

    [DataMember] public OptionalComponentWrapper<EntityQuery> AdditionalTargetQuery { get; }
    [DataMember] public ComponentPrimitive<bool> OnlyApplyEffectsOnAdditionalTargets { get; }
    [DataMember] public ComponentWrapper<EntityQuery> Query { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
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