using Newtonsoft.Json.Linq;
using PvZHCardEditor.Components;
using System.Linq;

namespace PvZHCardEditor.Queries
{
    public class AdjacentLaneQuery : CardComponent
    {
        public AdjacentLaneQuery() { }
        public AdjacentLaneQuery(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["Side"] = "Either",
            ["OriginEntityType"] = "Self"
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("Side", new ComponentString(token["Side"]!)),
            new ComponentNode("OriginEntityType", new ComponentString(token["OriginEntityType"]!))
        }));
    }

    public class AlwaysMatchesQuery : CardComponent
    {
        public AlwaysMatchesQuery() { }
        public AlwaysMatchesQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class AttackComparisonQuery : CardComponent
    {
        public AttackComparisonQuery() { }
        public AttackComparisonQuery(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["ComparisonOperator"] = "LessOrEqual",
            ["AttackValue"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("ComparisonOperator", new ComponentString(token["ComparisonOperator"]!)),
            new ComponentNode("AttackValue", new ComponentInt(token["AttackValue"]!))
        }));
    }

    public class BehindSameLaneQuery : CardComponent
    {
        public BehindSameLaneQuery() { }
        public BehindSameLaneQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class BlockMeterValueQuery : CardComponent
    {
        public BlockMeterValueQuery() { }
        public BlockMeterValueQuery(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["ComparisonOperator"] = "LessOrEqual",
            ["BlockMeterValue"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("ComparisonOperator", new ComponentString(token["ComparisonOperator"]!)),
            new ComponentNode("BlockMeterValue", new ComponentInt(token["BlockMeterValue"]!))
        }));
    }

    public class CardGuidQuery : Card
    {
        public CardGuidQuery() { }
        public CardGuidQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class CompositeAllQuery : CardComponent
    {
        public CompositeAllQuery() : base() { }
        public CompositeAllQuery(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override bool AllowAdd => true;

        protected override JToken DefaultToken => new JObject
        {
            ["queries"] = new JArray()
        };

        protected override ComponentValue? DefaultValue(JToken token)
        {
            var queries = (JArray)token["queries"]!;
            return new ComponentArray(queries, queries.Select(q => ComponentNode.ParseComponent(q)).Where(v => v is not null).Select(v => v!));
        }
    }

    public class CompositeAnyQuery : CompositeAllQuery
    {
        public CompositeAnyQuery() { }
        public CompositeAnyQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class DamageTakenComparisonQuery : CardComponent
    {
        public DamageTakenComparisonQuery() { }
        public DamageTakenComparisonQuery(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["ComparisonOperator"] = "LessOrEqual",
            ["DamageTakenValue"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("ComparisonOperator", new ComponentString(token["ComparisonOperator"]!)),
            new ComponentNode("DamageTakenValue", new ComponentInt(token["DamageTakenValue"]!))
        }));
    }

    public class DrawnCardQuery : CardComponent
    {
        public DrawnCardQuery() { }
        public DrawnCardQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class FighterQuery : CardComponent
    {
        public FighterQuery() { }
        public FighterQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class HasComponentQuery : CardComponent
    {
        public HasComponentQuery() { }
        public HasComponentQuery(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("ComponentType", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["ComponentType"] = ""
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentString(token["ComponentType"]!);
    }

    public class InAdjacentLaneQuery : CardComponent
    {
        public InAdjacentLaneQuery() { }
        public InAdjacentLaneQuery(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("Side", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["Side"] = "Either"
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentString(token["Side"]!);
    }

    public class InEnvironmentQuery : CardComponent
    {
        public InEnvironmentQuery() { }
        public InEnvironmentQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class InHandQuery : CardComponent
    {
        public InHandQuery() { }
        public InHandQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class InLaneAdjacentToLaneQuery : InAdjacentLaneQuery
    {
        public InLaneAdjacentToLaneQuery() { }
        public InLaneAdjacentToLaneQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class InLaneQuery : CardComponent
    {
        public InLaneQuery() { }
        public InLaneQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class InLaneSameAsLaneQuery : CardComponent
    {
        public InLaneSameAsLaneQuery() { }
        public InLaneSameAsLaneQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class InOneTimeEffectZoneQuery : CardComponent
    {
        public InOneTimeEffectZoneQuery() { }
        public InOneTimeEffectZoneQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class InSameLaneQuery : CardComponent
    {
        public InSameLaneQuery() { }
        public InSameLaneQuery(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("OriginEntityType", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["OriginEntityType"] = "Self"
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentString(token["OriginEntityType"]!);
    }

    public class InUnopposedLaneQuery : CardComponent
    {
        public InUnopposedLaneQuery() { }
        public InUnopposedLaneQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class IsActiveQuery : CardComponent
    {
        public IsActiveQuery() { }
        public IsActiveQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class IsAliveQuery : CardComponent
    {
        public IsAliveQuery() { }
        public IsAliveQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class KilledByQuery : SingleQueryComponent
    {
        public KilledByQuery() { }
        public KilledByQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class LacksComponentQuery : HasComponentQuery
    {
        public LacksComponentQuery() { }
        public LacksComponentQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class LaneOfIndexQuery : CardComponent
    {
        public LaneOfIndexQuery() { }
        public LaneOfIndexQuery(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("LaneIndex", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["LaneIndex"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentInt(token["LaneIndex"]!);
    }

    public class LaneWithMatchingEnvironmentQuery : SingleQueryComponent
    {
        public LaneWithMatchingEnvironmentQuery() { }
        public LaneWithMatchingEnvironmentQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class LaneWithMatchingFighterQuery : SingleQueryComponent
    {
        public LaneWithMatchingFighterQuery() { }
        public LaneWithMatchingFighterQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class LastLaneOfSelfQuery : CardComponent
    {
        public LastLaneOfSelfQuery() { }
        public LastLaneOfSelfQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class NotQuery : SingleQueryComponent
    {
        public NotQuery() { }
        public NotQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class OnTerrainQuery : CardComponent
    {
        public OnTerrainQuery() { }
        public OnTerrainQuery(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("TerrainType", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["TerrainType"] = ""
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentString(token["TerrainType"]!);
    }

    public class OpenLaneQuery : CardComponent
    {
        public OpenLaneQuery() { }
        public OpenLaneQuery(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["PlayerFactionType"] = "",
            ["IsForTeamupCard"] = false
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("PlayerFactionType", new ComponentString(token["PlayerFactionType"]!)),
            new ComponentNode("IsForTeamupCard", new ComponentBool(token["IsForTeamupCard"]!))
        }));
    }

    public class OriginalTargetCardGuidQuery : CardComponent
    {
        public OriginalTargetCardGuidQuery() { }
        public OriginalTargetCardGuidQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class SameFactionQuery : CardComponent
    {
        public SameFactionQuery() { }
        public SameFactionQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class SameLaneAsTargetQuery : CardComponent
    {
        public SameLaneAsTargetQuery() { }
        public SameLaneAsTargetQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class SameLaneQuery : CardComponent
    {
        public SameLaneQuery() { }
        public SameLaneQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class SelfQuery : CardComponent
    {
        public SelfQuery() : base() { }
        public SelfQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class SourceQuery : CardComponent
    {
        public SourceQuery() { }
        public SourceQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class SpringboardedOnSelfQuery : CardComponent
    {
        public SpringboardedOnSelfQuery() { }
        public SpringboardedOnSelfQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class SubsetQuery : CardComponent
    {
        public SubsetQuery() { }
        public SubsetQuery(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("Subset", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["Subset"] = ""
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentString(token["Subset"]!);
    }

    public class SubtypeQuery : CardComponent
    {
        public SubtypeQuery() { }
        public SubtypeQuery(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("Subtype", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["Subtype"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentInt(token["Subtype"]!);
    }

    public class SunCostComparisonQuery : CardComponent
    {
        public SunCostComparisonQuery() { }
        public SunCostComparisonQuery(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["ComparisonOperator"] = "LessOrEqual",
            ["SunCost"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("ComparisonOperator", new ComponentString(token["ComparisonOperator"]!)),
            new ComponentNode("SunCost", new ComponentInt(token["SunCost"]!))
        }));
    }

    public class SunCostPlusNComparisonQuery : CardComponent
    {
        public SunCostPlusNComparisonQuery() { }
        public SunCostPlusNComparisonQuery(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["ComparisonOperator"] = "Equal",
            ["AdditionalCost"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("ComparisonOperator", new ComponentString(token["ComparisonOperator"]!)),
            new ComponentNode("AdditionalCost", new ComponentInt(token["AdditionalCost"]!))
        }));
    }

    public class SunCounterComparisonQuery : CardComponent
    {
        public SunCounterComparisonQuery() { }
        public SunCounterComparisonQuery(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["ComparisonOperator"] = "LessOrEqual",
            ["SunCounterValue"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("ComparisonOperator", new ComponentString(token["ComparisonOperator"]!)),
            new ComponentNode("SunCounterValue", new ComponentInt(token["SunCounterValue"]!))
        }));
    }

    public class TargetCardGuidQuery : CardComponent
    {
        public TargetCardGuidQuery() { }
        public TargetCardGuidQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class TargetQuery : CardComponent
    {
        public TargetQuery() { }
        public TargetQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class TargetableInPlayFighterQuery : CardComponent
    {
        public TargetableInPlayFighterQuery() { }
        public TargetableInPlayFighterQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class TrickQuery : CardComponent
    {
        public TrickQuery() { }
        public TrickQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class TurnCountQuery : CardComponent
    {
        public TurnCountQuery() { }
        public TurnCountQuery(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["TurnCount"] = 0,
            ["ComparisonOperator"] = "LessOrEqual"
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("TurnCount", new ComponentInt(token["TurnCount"]!)),
            new ComponentNode("ComparisonOperator", new ComponentString(token["ComparisonOperator"]!))
        }));
    }

    public class WasInSameLaneAsSelfQuery : CardComponent
    {
        public WasInSameLaneAsSelfQuery() { }
        public WasInSameLaneAsSelfQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class WillTriggerEffectsQuery : CardComponent
    {
        public WillTriggerEffectsQuery() { }
        public WillTriggerEffectsQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class WillTriggerOnDeathEffectsQuery : CardComponent
    {
        public WillTriggerOnDeathEffectsQuery() { }
        public WillTriggerOnDeathEffectsQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }
}
