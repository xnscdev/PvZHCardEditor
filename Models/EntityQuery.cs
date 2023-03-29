using System;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using ReactiveUI;

namespace PvZHCardEditor.Models;

#region Abstract classes

public abstract class EntityQuery : EntityComponentBase
{
}

[DataContract]
public abstract class NestedQuery : EntityQuery
{
    protected NestedQuery() : this(new ComponentWrapper<EntityQuery>())
    {
    }

    protected NestedQuery(ComponentWrapper<EntityQuery> query)
    {
        Query = query;
        Children = this.CreateReactiveProperties((nameof(Query), Query));
    }

    [DataMember] public ComponentWrapper<EntityQuery> Query { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public abstract class LaneSideQuery : EntityQuery
{
    protected LaneSideQuery() : this("Either")
    {
    }

    protected LaneSideQuery(string side)
    {
        Side = new ComponentPrimitive<string>(side);
        Children = this.CreateReactiveProperties((nameof(Side), Side));
    }

    [DataMember] public ComponentPrimitive<string> Side { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public abstract class ComponentTypeQuery : EntityQuery
{
    protected ComponentTypeQuery() : this(new ComponentTypeString(
        GetHasComponentTypeString(GetDisplayTypeString(GameDataManager.GetComponentTypes<EntityQuery>().First()))))
    {
    }

    protected ComponentTypeQuery(ComponentTypeString componentType)
    {
        ComponentType = componentType;
        Children = this.CreateReactiveProperties((nameof(ComponentType), ComponentType));
    }

    [DataMember] public ComponentTypeString ComponentType { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public abstract class CompositeQuery : EntityQuery
{
    protected CompositeQuery() : this(new ComponentList<ComponentWrapper<EntityQuery>>())
    {
    }

    protected CompositeQuery(ComponentList<ComponentWrapper<EntityQuery>> queries)
    {
        Queries = queries;
        Queries.WhenAnyValue(x => x.Children).Subscribe(_ => this.RaisePropertyChanged(nameof(Children)));
    }

    [DataMember]
    [JsonProperty(PropertyName = "queries")]
    public ComponentList<ComponentWrapper<EntityQuery>> Queries { get; }

    public override FullObservableCollection<ComponentProperty> Children => Queries.Children;
    public override ComponentValue EditHandler => Queries;
}

#endregion

[DataContract]
public class AdjacentLaneQuery : EntityQuery
{
    public AdjacentLaneQuery() : this("Either", "Self")
    {
    }

    [JsonConstructor]
    public AdjacentLaneQuery(string side, string originEntityType)
    {
        Side = new ComponentPrimitive<string>(side);
        OriginEntityType = new ComponentPrimitive<string>(originEntityType);
        Children = this.CreateReactiveProperties(
            (nameof(Side), Side),
            (nameof(OriginEntityType), OriginEntityType));
    }

    [DataMember] public ComponentPrimitive<string> Side { get; }
    [DataMember] public ComponentPrimitive<string> OriginEntityType { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class AlwaysMatchesQuery : EntityQuery
{
}

[DataContract]
public class AttackComparisonQuery : EntityQuery
{
    public AttackComparisonQuery() : this("LessOrEqual", 0)
    {
    }

    [JsonConstructor]
    public AttackComparisonQuery(string comparisonOperator, int attackValue)
    {
        ComparisonOperator = new ComponentPrimitive<string>(comparisonOperator);
        AttackValue = new ComponentPrimitive<int>(attackValue);
        Children = this.CreateReactiveProperties(
            (nameof(ComparisonOperator), ComparisonOperator),
            (nameof(AttackValue), AttackValue));
    }

    [DataMember] public ComponentPrimitive<string> ComparisonOperator { get; }
    [DataMember] public ComponentPrimitive<int> AttackValue { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class BehindSameLaneQuery : EntityQuery
{
}

[DataContract]
public class BlockMeterValueQuery : EntityQuery
{
    public BlockMeterValueQuery() : this("LessOrEqual", 0)
    {
    }

    [JsonConstructor]
    public BlockMeterValueQuery(string comparisonOperator, int blockMeterValue)
    {
        ComparisonOperator = new ComponentPrimitive<string>(comparisonOperator);
        BlockMeterValue = new ComponentPrimitive<int>(blockMeterValue);
        Children = this.CreateReactiveProperties(
            (nameof(ComparisonOperator), ComparisonOperator),
            (nameof(BlockMeterValue), BlockMeterValue));
    }

    [DataMember] public ComponentPrimitive<string> ComparisonOperator { get; }
    [DataMember] public ComponentPrimitive<int> BlockMeterValue { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class CardGuidQuery : EntityQuery
{
    public CardGuidQuery() : this(0)
    {
    }

    [JsonConstructor]
    public CardGuidQuery(int guid)
    {
        Guid = new ComponentPrimitive<int>(guid);
        Children = this.CreateReactiveProperties((nameof(Guid), Guid));
    }

    [DataMember] public ComponentPrimitive<int> Guid { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class CompositeAllQuery : CompositeQuery
{
    public CompositeAllQuery()
    {
    }

    [JsonConstructor]
    public CompositeAllQuery(ComponentList<ComponentWrapper<EntityQuery>> queries) : base(queries)
    {
    }
}

[DataContract]
public class CompositeAnyQuery : CompositeQuery
{
    public CompositeAnyQuery()
    {
    }

    [JsonConstructor]
    public CompositeAnyQuery(ComponentList<ComponentWrapper<EntityQuery>> queries) : base(queries)
    {
    }
}

[DataContract]
public class DamageTakenComparisonQuery : EntityQuery
{
    public DamageTakenComparisonQuery() : this("LessOrEqual", 0)
    {
    }

    [JsonConstructor]
    public DamageTakenComparisonQuery(string comparisonOperator, int damageTakenValue)
    {
        ComparisonOperator = new ComponentPrimitive<string>(comparisonOperator);
        DamageTakenValue = new ComponentPrimitive<int>(damageTakenValue);
        Children = this.CreateReactiveProperties(
            (nameof(ComparisonOperator), ComparisonOperator),
            (nameof(DamageTakenValue), DamageTakenValue));
    }

    [DataMember] public ComponentPrimitive<string> ComparisonOperator { get; }
    [DataMember] public ComponentPrimitive<int> DamageTakenValue { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class DrawnCardQuery : EntityQuery
{
}

[DataContract]
public class FighterQuery : EntityQuery
{
}

[DataContract]
public class HasComponentQuery : ComponentTypeQuery
{
    public HasComponentQuery()
    {
    }

    [JsonConstructor]
    public HasComponentQuery(ComponentTypeString componentType) : base(componentType)
    {
    }
}

[DataContract]
public class InAdjacentLaneQuery : LaneSideQuery
{
    public InAdjacentLaneQuery()
    {
    }

    [JsonConstructor]
    public InAdjacentLaneQuery(string side) : base(side)
    {
    }
}

[DataContract]
public class InEnvironmentQuery : EntityQuery
{
}

[DataContract]
public class InHandQuery : EntityQuery
{
}

[DataContract]
public class InLaneAdjacentToLaneQuery : LaneSideQuery
{
    public InLaneAdjacentToLaneQuery()
    {
    }

    [JsonConstructor]
    public InLaneAdjacentToLaneQuery(string side) : base(side)
    {
    }
}

[DataContract]
public class InLaneQuery : EntityQuery
{
}

[DataContract]
public class InLaneSameAsLaneQuery : EntityQuery
{
}

[DataContract]
public class InOneTimeEffectZoneQuery : EntityQuery
{
}

[DataContract]
public class InSameLaneQuery : EntityQuery
{
    public InSameLaneQuery() : this("Self")
    {
    }

    [JsonConstructor]
    public InSameLaneQuery(string originEntityType)
    {
        OriginEntityType = new ComponentPrimitive<string>(originEntityType);
        Children = this.CreateReactiveProperties((nameof(OriginEntityType), OriginEntityType));
    }

    [DataMember] public ComponentPrimitive<string> OriginEntityType { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class InUnopposedLaneQuery : EntityQuery
{
}

[DataContract]
public class IsActiveQuery : EntityQuery
{
}

[DataContract]
public class IsAliveQuery : EntityQuery
{
}

[DataContract]
public class KilledByQuery : NestedQuery
{
    public KilledByQuery()
    {
    }

    [JsonConstructor]
    public KilledByQuery(ComponentWrapper<EntityQuery> query) : base(query)
    {
    }
}

[DataContract]
public class LacksComponentQuery : ComponentTypeQuery
{
    public LacksComponentQuery()
    {
    }

    [JsonConstructor]
    public LacksComponentQuery(ComponentTypeString componentType) : base(componentType)
    {
    }
}

[DataContract]
public class LaneOfIndexQuery : EntityQuery
{
    public LaneOfIndexQuery() : this(0)
    {
    }

    [JsonConstructor]
    public LaneOfIndexQuery(int laneIndex)
    {
        LaneIndex = new ComponentPrimitive<int>(laneIndex);
        Children = this.CreateReactiveProperties((nameof(LaneIndex), LaneIndex));
    }

    [DataMember] public ComponentPrimitive<int> LaneIndex { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class LaneWithMatchingEnvironmentQuery : NestedQuery
{
    public LaneWithMatchingEnvironmentQuery()
    {
    }

    [JsonConstructor]
    public LaneWithMatchingEnvironmentQuery(ComponentWrapper<EntityQuery> query) : base(query)
    {
    }
}

[DataContract]
public class LaneWithMatchingFighterQuery : NestedQuery
{
    public LaneWithMatchingFighterQuery()
    {
    }

    [JsonConstructor]
    public LaneWithMatchingFighterQuery(ComponentWrapper<EntityQuery> query) : base(query)
    {
    }
}

[DataContract]
public class LastLaneOfSelfQuery : EntityQuery
{
}

[DataContract]
public class NotQuery : NestedQuery
{
    public NotQuery()
    {
    }

    [JsonConstructor]
    public NotQuery(ComponentWrapper<EntityQuery> query) : base(query)
    {
    }
}

[DataContract]
public class OnTerrainQuery : EntityQuery
{
    public OnTerrainQuery() : this(string.Empty)
    {
    }

    [JsonConstructor]
    public OnTerrainQuery(string terrainType)
    {
        TerrainType = new ComponentPrimitive<string>(terrainType);
        Children = this.CreateReactiveProperties((nameof(TerrainType), TerrainType));
    }

    [DataMember] public ComponentPrimitive<string> TerrainType { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class OpenLaneQuery : EntityQuery
{
    public OpenLaneQuery() : this(string.Empty, false)
    {
    }

    [JsonConstructor]
    public OpenLaneQuery(string playerFactionType, bool isForTeamupCard)
    {
        PlayerFactionType = new ComponentPrimitive<string>(playerFactionType);
        IsForTeamupCard = new ComponentPrimitive<bool>(isForTeamupCard);
        Children = this.CreateReactiveProperties(
            (nameof(PlayerFactionType), PlayerFactionType),
            (nameof(IsForTeamupCard), IsForTeamupCard));
    }

    [DataMember] public ComponentPrimitive<string> PlayerFactionType { get; }
    [DataMember] public ComponentPrimitive<bool> IsForTeamupCard { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class OriginalTargetCardGuidQuery : EntityQuery
{
}

[DataContract]
public class SameFactionQuery : EntityQuery
{
}

[DataContract]
public class SameLaneAsTargetQuery : EntityQuery
{
}

[DataContract]
public class SameLaneQuery : EntityQuery
{
}

[DataContract]
public class SelfQuery : EntityQuery
{
}

[DataContract]
public class SourceQuery : EntityQuery
{
}

[DataContract]
public class SpringboardedOnSelfQuery : EntityQuery
{
}

[DataContract]
public class SubsetQuery : EntityQuery
{
    public SubsetQuery() : this(string.Empty)
    {
    }

    [JsonConstructor]
    public SubsetQuery(string subset)
    {
        Subset = new ComponentPrimitive<string>(subset);
        Children = this.CreateReactiveProperties((nameof(Subset), Subset));
    }

    [DataMember] public ComponentPrimitive<string> Subset { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class SubtypeQuery : EntityQuery
{
    public SubtypeQuery() : this(0)
    {
    }

    [JsonConstructor]
    public SubtypeQuery(int subtype)
    {
        Subtype = new ComponentPrimitive<int>(subtype);
        Children = this.CreateReactiveProperties((nameof(Subtype), Subtype));
    }

    [DataMember] public ComponentPrimitive<int> Subtype { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class SunCostComparisonQuery : EntityQuery
{
    public SunCostComparisonQuery() : this("LessOrEqual", 0)
    {
    }

    [JsonConstructor]
    public SunCostComparisonQuery(string comparisonOperator, int sunCost)
    {
        ComparisonOperator = new ComponentPrimitive<string>(comparisonOperator);
        SunCost = new ComponentPrimitive<int>(sunCost);
        Children = this.CreateReactiveProperties(
            (nameof(ComparisonOperator), ComparisonOperator),
            (nameof(SunCost), SunCost));
    }

    [DataMember] public ComponentPrimitive<string> ComparisonOperator { get; }
    [DataMember] public ComponentPrimitive<int> SunCost { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class SunCostPlusNComparisonQuery : EntityQuery
{
    public SunCostPlusNComparisonQuery() : this("Equal", 0)
    {
    }

    [JsonConstructor]
    public SunCostPlusNComparisonQuery(string comparisonOperator, int additionalCost)
    {
        ComparisonOperator = new ComponentPrimitive<string>(comparisonOperator);
        AdditionalCost = new ComponentPrimitive<int>(additionalCost);
        Children = this.CreateReactiveProperties(
            (nameof(ComparisonOperator), ComparisonOperator),
            (nameof(AdditionalCost), AdditionalCost));
    }

    [DataMember] public ComponentPrimitive<string> ComparisonOperator { get; }
    [DataMember] public ComponentPrimitive<int> AdditionalCost { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class SunCounterComparisonQuery : EntityQuery
{
    public SunCounterComparisonQuery() : this("LessOrEqual", 0)
    {
    }

    [JsonConstructor]
    public SunCounterComparisonQuery(string comparisonOperator, int sunCounterValue)
    {
        ComparisonOperator = new ComponentPrimitive<string>(comparisonOperator);
        SunCounterValue = new ComponentPrimitive<int>(sunCounterValue);
        Children = this.CreateReactiveProperties(
            (nameof(ComparisonOperator), ComparisonOperator),
            (nameof(SunCounterValue), SunCounterValue));
    }

    [DataMember] public ComponentPrimitive<string> ComparisonOperator { get; }
    [DataMember] public ComponentPrimitive<int> SunCounterValue { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class TargetCardGuidQuery : EntityQuery
{
}

[DataContract]
public class TargetQuery : EntityQuery
{
}

[DataContract]
public class TargetableInPlayFighterQuery : EntityQuery
{
}

[DataContract]
public class TrickQuery : EntityQuery
{
}

[DataContract]
public class TurnCountQuery : EntityQuery
{
    public TurnCountQuery() : this(0, "LessOrEqual")
    {
    }

    [JsonConstructor]
    public TurnCountQuery(int turnCount, string comparisonOperator)
    {
        TurnCount = new ComponentPrimitive<int>(turnCount);
        ComparisonOperator = new ComponentPrimitive<string>(comparisonOperator);
        Children = this.CreateReactiveProperties(
            (nameof(TurnCount), TurnCount),
            (nameof(ComparisonOperator), ComparisonOperator));
    }

    [DataMember] public ComponentPrimitive<int> TurnCount { get; }
    [DataMember] public ComponentPrimitive<string> ComparisonOperator { get; }

    public override FullObservableCollection<ComponentProperty> Children { get; }
}

[DataContract]
public class WasInSameLaneAsSelfQuery : EntityQuery
{
}

[DataContract]
public class WillTriggerEffectsQuery : EntityQuery
{
}

[DataContract]
public class WillTriggerOnDeathEffectsQuery : EntityQuery
{
}