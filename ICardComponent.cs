using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PvZHCardEditor
{
    public interface ICardComponent
    {
        public TreeViewNode Node { get; }

        public static Type? ParseComponentType(string s)
        {
            var match = Regex.Match(s, "^PvZCards\\.Engine\\.Components\\.([a-zA-Z0-9_]+),");
            return Type.GetType($"PvZHCardEditor.{match.Groups[1].Value}Component");
        }

        public static ICardComponent? ParseComponent(JToken token)
        {
            if (token.Type == JTokenType.Null)
                return null;

            var type = ParseComponentType((string)token["$type"]!);
            if (type is null)
                return null;

            var info = type.GetConstructor(new[] { typeof(JToken) });
            if (info is not null)
                return (ICardComponent)info.Invoke(new[] { token["$data"]! });

            info = type.GetConstructor(Array.Empty<Type>());
            return info is null ? null : (ICardComponent)info.Invoke(null);
        }
    }

    public abstract class SingletonCardComponent<T> : ICardComponent
    {
        private readonly string _name;
        private readonly T _value;

        public TreeViewNode Node => new($"{_name} = {_value}");

        public SingletonCardComponent(string name, T value)
        {
            _name = name;
            _value = value;
        }
    }

    public abstract class TraitCardComponent : ICardComponent
    {
        private readonly TreeViewNode _node;

        public TreeViewNode Node => _node;

        public TraitCardComponent(string name, JToken token)
        {
            var counters = token["Counters"]!;
            var children = new List<TreeViewNode>();
            var persistent = new TreeViewNode($"IsPersistent = {counters["IsPersistent"]}");
            children.Add(persistent);
            var index = 0;
            foreach (var counter in counters["Counters"]!)
            {
                var sourceId = new TreeViewNode($"SourceId = {(int)counter["SourceId"]!}");
                var duration = new TreeViewNode($"Duration = {(int)counter["Duration"]!}");
                var value = new TreeViewNode($"Value = {(int)counter["Value"]!}");
                var node = new TreeViewCompoundNode($"[{index++}]", new[] { sourceId, duration, value });
                children.Add(node);
            }
            _node = new TreeViewCompoundNode(name, children);
        }
    }

    #region Empty Card Components

    public class UnusableComponent : ICardComponent
    {
        public TreeViewNode Node => new("Unusable");
    }

    public class BoardAbilityComponent : ICardComponent
    {
        public TreeViewNode Node => new("BoardAbility");
    }

    public class PlantsComponent : ICardComponent
    {
        public TreeViewNode Node => new("Plants");
    }

    public class ZombiesComponent : ICardComponent
    {
        public TreeViewNode Node => new("Zombies");
    }

    public class PlaysFaceDownComponent : ICardComponent
    {
        public TreeViewNode Node => new("PlaysFaceDown");
    }

    public class SurpriseComponent : ICardComponent
    {
        public TreeViewNode Node => new("Surprise");
    }

    public class SuperpowerComponent : ICardComponent
    {
        public TreeViewNode Node => new("Superpower");
    }

    public class PrimarySuperpowerComponent : ICardComponent
    {
        public TreeViewNode Node => new("PrimarySuperpower");
    }

    public class CreateInFrontComponent : ICardComponent
    {
        public TreeViewNode Node => new("CreateInFront");
    }

    public class EnvironmentComponent : ICardComponent
    {
        public TreeViewNode Node => new("Environment");
    }

    public class MultishotComponent : ICardComponent
    {
        public TreeViewNode Node => new("Multishot");
    }

    public class AttacksInAllLanesComponent : ICardComponent
    {
        public TreeViewNode Node => new("AttacksInAllLanes");
    }

    public class AttacksOnlyInAdjacentLanesComponent : ICardComponent
    {
        public TreeViewNode Node => new("AttacksOnlyInAdjacentLanes");
    }

    public class BurstComponent : ICardComponent
    {
        public TreeViewNode Node => new("Burst");
    }

    public class FromBurstComponent : ICardComponent
    {
        public TreeViewNode Node => new("FromBurst");
    }

    public class EvolvableComponent : ICardComponent
    {
        public TreeViewNode Node => new("Evolvable");
    }

    public class SpringboardComponent : ICardComponent
    {
        public TreeViewNode Node => new("Springboard");
    }

    public class MustacheComponent : ICardComponent
    {
        public TreeViewNode Node => new("Mustache");
    }

    public class DiscardFromPlayTriggerComponent : ICardComponent
    {
        public TreeViewNode Node => new("DiscardFromPlayTrigger");
    }

    public class HighgroundTerrainComponent : ICardComponent
    {
        public TreeViewNode Node => new("HighgroundTerrain");
    }

    #endregion

    #region Singleton Card Components

    public class CardComponent : SingletonCardComponent<int>
    {
        public CardComponent(JToken token) : base("Guid", (int)token["Guid"]!) { }
    }

    public class RarityComponent : SingletonCardComponent<string>
    {
        public RarityComponent(JToken token) : base("Rarity", (string)token["Value"]!) { }
    }

    public class AttackComponent : SingletonCardComponent<int>
    {
        public AttackComponent(JToken token) : base("Attack", (int)token["AttackValue"]!["BaseValue"]!) { }
    }

    public class HealthComponent : SingletonCardComponent<int>
    {
        public HealthComponent(JToken token) : base("Health", (int)token["MaxHealth"]!["BaseValue"]!) { }
    }

    public class SunCostComponent : SingletonCardComponent<int>
    {
        public SunCostComponent(JToken token) : base("SunCost", (int)token["SunCostValue"]!["BaseValue"]!) { }
    }

    public class EffectEntityGroupingComponent : SingletonCardComponent<int>
    {
        public EffectEntityGroupingComponent(JToken token) : base("EffectEntityGrouping", (int)token["AbilityGroupId"]!) { }
    }

    public class DamageEffectDescriptorComponent : SingletonCardComponent<int>
    {
        public DamageEffectDescriptorComponent(JToken token) : base("DamageEffectDescriptor", (int)token["DamageAmount"]!) { }
    }

    #endregion

    #region Trait Card Components

    public class AquaticComponent : TraitCardComponent
    {
        public AquaticComponent(JToken token) : base("Aquatic", token) { }
    }

    public class FrenzyComponent : TraitCardComponent
    {
        public FrenzyComponent(JToken token) : base("Frenzy", token) { }
    }

    public class TeamupComponent : TraitCardComponent
    {
        public TeamupComponent(JToken token) : base("Teamup", token) { }
    }

    public class TruestrikeComponent : TraitCardComponent
    {
        public TruestrikeComponent(JToken token) : base("Truestrike", token) { }
    }

    public class StrikethroughComponent : TraitCardComponent
    {
        public StrikethroughComponent(JToken token) : base("Strikethrough", token) { }
    }

    public class DeadlyComponent : TraitCardComponent
    {
        public DeadlyComponent(JToken token) : base("Deadly", token) { }
    }

    public class AttackOverrideComponent : TraitCardComponent
    {
        public AttackOverrideComponent(JToken token) : base("AttackOverride", token) { }
    }

    public class UntrickableComponent : TraitCardComponent
    {
        public UntrickableComponent(JToken token) : base("Untrickable", token) { }
    }

    #endregion

    #region Other Card Components

    public class SubtypesComponent : ICardComponent
    {
        private readonly CardTribe[] _tribes;
        private readonly TreeViewNode _node;

        public TreeViewNode Node => _node;

        public SubtypesComponent(JToken token)
        {
            var array = (JArray)token["subtypes"]!;
            _tribes = array.Select(t => (CardTribe)(int)t).ToArray();
            _node = new TreeViewCompoundNode("Subtypes", _tribes.Select(t => new TreeViewNode(t.ToString())));
        }
    }

    public class TagsComponent : ICardComponent
    {
        private readonly string[] _tags;
        private readonly TreeViewNode _node;

        public TreeViewNode Node => _node;

        public TagsComponent(JToken token)
        {
            var array = (JArray)token["tags"]!;
            _tags = array.Select(t => (string)t!).ToArray();
            _node = new TreeViewCompoundNode("Tags", _tags.Select(t => new TreeViewNode(t)));
        }
    }

    public class GrantedTriggeredAbilitiesComponent : ICardComponent
    {
        private readonly TreeViewNode _node;

        public TreeViewNode Node => _node;

        public GrantedTriggeredAbilitiesComponent(JToken token)
        {
            var a = token["a"]!;
            var children = new List<TreeViewNode>();
            var index = 0;
            foreach (var s in a)
            {
                var g = new TreeViewNode($"g = {(int)s["g"]!}");
                var vt = new TreeViewNode($"vt = {(int)s["vt"]!}");
                var va = new TreeViewNode($"va = {(int)s["va"]!}");
                var node = new TreeViewCompoundNode($"[{index++}]", new[] { g, vt, va });
                children.Add(node);
            }
            _node = new TreeViewCompoundNode("GrantedTriggeredAbilities", children);
        }
    }

    public class ShowTriggeredIconComponent : ICardComponent
    {
        private readonly List<CardAbilityIcon> _abilities = new();
        private readonly TreeViewNode _node;

        public TreeViewNode Node => _node;

        public ShowTriggeredIconComponent(JToken token)
        {
            var abilities = (JArray)token["abilities"]!;
            _abilities = abilities.Select(a => (CardAbilityIcon)(int)a).ToList();
            _node = new TreeViewCompoundNode("ShowTriggeredIcon", _abilities.Select(a => new TreeViewNode(a.ToString())));
        }
    }

    public class EvolutionRestrictionComponent : ICardComponent
    {
        private readonly IQuery _query;
        private readonly TreeViewNode _node;

        public TreeViewNode Node => _node;

        public EvolutionRestrictionComponent(JToken token)
        {
            _query = IQuery.ParseQuery(token["Query"]!)!;
            _node = new TreeViewCompoundNode("EvolutionRestriction", new[] { _query.Node });
        }
    }

    public class TriggerTargetFilterComponent : ICardComponent
    {
        private readonly IQuery _query;
        private readonly TreeViewNode _node;

        public TreeViewNode Node => _node;

        public TriggerTargetFilterComponent(JToken token)
        {
            _query = IQuery.ParseQuery(token["Query"]!)!;
            _node = new TreeViewCompoundNode("TriggerTargetFilter", new[] { _query.Node });
        }
    }

    public class PrimaryTargetFilterComponent : ICardComponent
    {
        private readonly string _selectionType;
        private readonly int _numTargets;
        private readonly string _targetScopeType;
        private readonly string _targetScopeSortValue;
        private readonly string _targetScopeSortMethod;
        private readonly string _additionalTargetType;
        private readonly IQuery? _additionalTargetQuery;
        private readonly bool _onlyApplyEffectsOnAdditionalTargets;
        private readonly IQuery _query;
        private readonly TreeViewNode _node;

        public TreeViewNode Node => _node;

        public PrimaryTargetFilterComponent(JToken token)
        {
            _selectionType = (string)token["SelectionType"]!;
            _numTargets = (int)token["NumTargets"]!;
            _targetScopeType = (string)token["TargetScopeType"]!;
            _targetScopeSortValue = (string)token["TargetScopeSortValue"]!;
            _targetScopeSortMethod = (string)token["TargetScopeSortMethod"]!;
            _additionalTargetType = (string)token["AdditionalTargetType"]!;
            _additionalTargetQuery = IQuery.ParseQuery(token["AdditionalTargetQuery"]!);
            _onlyApplyEffectsOnAdditionalTargets = (bool)token["OnlyApplyEffectsOnAdditionalTargets"]!;
            _query = IQuery.ParseQuery(token["Query"]!)!;
            
            _node = new TreeViewCompoundNode("PrimaryTargetFilter", new TreeViewNode[] {
                new($"SelectionType = {_selectionType}"),
                new($"NumTargets = {_numTargets}"),
                new($"TargetScopeType = {_targetScopeType}"),
                new($"TargetScopeSortValue = {_targetScopeSortValue}"),
                new($"TargetScopeSortMethod = {_targetScopeSortMethod}"),
                new($"AdditionalTargetType = {_additionalTargetType}"),
                new TreeViewCompoundNode("AdditionalTargetQuery", new[] { _additionalTargetQuery?.Node ?? new("null") }),
                new($"OnlyApplyEffectsOnAdditionalTargets = {_onlyApplyEffectsOnAdditionalTargets}"),
                new TreeViewCompoundNode("Query", new[] { _query.Node })
            });
        }
    }

    public class BuffEffectDescriptorComponent : ICardComponent
    {
        private readonly int _attackAmount;
        private readonly int _healthAmount;
        private readonly string _buffDuration;
        private readonly TreeViewNode _node;

        public TreeViewNode Node => _node;

        public BuffEffectDescriptorComponent(JToken token)
        {
            _attackAmount = (int)token["AttackAmount"]!;
            _healthAmount = (int)token["HealthAmount"]!;
            _buffDuration = (string)token["BuffDuration"]!;

            _node = new TreeViewCompoundNode("BuffEffectDescriptor", new TreeViewNode[]
            {
                new($"AttackAmount = {_attackAmount}"),
                new($"HealthAmount = {_healthAmount}"),
                new($"BuffDuration = {_buffDuration}")
            });
        }
    }

    public class EffectEntitiesDescriptorComponent : ICardComponent
    {
        private readonly List<List<ICardComponent>> _components = new();
        private readonly TreeViewNode _node;

        public TreeViewNode Node => _node;

        public EffectEntitiesDescriptorComponent(JToken token)
        {
            var entities = (JArray)token["entities"]!;
            var index = 0;
            var nodes = new List<TreeViewNode>();
            foreach (var entity in entities)
            {
                var list = new List<ICardComponent>();
                var components = (JArray)entity["components"]!;
                foreach (var data in components)
                {
                    var component = ICardComponent.ParseComponent(data);
                    if (component is not null)
                        list.Add(component);
                }
                var node = new TreeViewCompoundNode($"[{index++}]", list.Select(c => c.Node));
                _components.Add(list);
                nodes.Add(node);
            }

            _node = new TreeViewCompoundNode("EffectEntitiesDescriptor", nodes);
        }
    }

    #endregion
}
