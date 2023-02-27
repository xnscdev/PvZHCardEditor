﻿using Newtonsoft.Json.Linq;
using System.Linq;

namespace PvZHCardEditor.Components
{
    public class ActiveTargets : CardComponent
    {
        public ActiveTargets() { }
        public ActiveTargets(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Aquatic : TraitCardComponent
    {
        public Aquatic() { }
        public Aquatic(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Armor : CardComponent
    {
        public Armor() { }
        public Armor(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("ArmorAmount", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["ArmorAmount"] = new JObject
            {
                ["BaseValue"] = 0
            }
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentInt(token["ArmorAmount"]!["BaseValue"]!);
    }

    public class Attack : CardComponent
    {
        public Attack() { }
        public Attack(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("AttackValue", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["AttackValue"] = new JObject
            {
                ["BaseValue"] = 0
            }
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentInt(token["AttackValue"]!["BaseValue"]!);
    }

    public class AttackInLaneEffectDescriptor : CardComponent
    {
        public AttackInLaneEffectDescriptor() { }
        public AttackInLaneEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("DamageAmount", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["DamageAmount"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentInt(token["DamageAmount"]!);
    }

    public class AttackOverride : TraitCardComponent
    {
        public AttackOverride() { }
        public AttackOverride(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class AttacksInAllLanes : CardComponent
    {
        public AttacksInAllLanes() { }
        public AttacksInAllLanes(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class AttacksOnlyInAdjacentLanes : CardComponent
    {
        public AttacksOnlyInAdjacentLanes() { }
        public AttacksOnlyInAdjacentLanes(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class BoardAbility : CardComponent
    {
        public BoardAbility() { }
        public BoardAbility(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class BuffEffectDescriptor : CardComponent
    {
        public BuffEffectDescriptor() { }
        public BuffEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["AttackAmount"] = 0,
            ["HealthAmount"] = 0,
            ["BuffDuration"] = "Permanent"
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("AttackAmount", new ComponentInt(token["AttackAmount"]!)),
            new ComponentNode("HealthAmount", new ComponentInt(token["HealthAmount"]!)),
            new ComponentNode("BuffDuration", new ComponentString(token["BuffDuration"]!))
        }));
    }

    public class BuffTrigger : CardComponent
    {
        public BuffTrigger() { }
        public BuffTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Burst : CardComponent
    {
        public Burst() { }
        public Burst(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Card : CardComponent
    {
        public Card() { }
        public Card(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] { 
            new ComponentNode("Guid", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["Guid"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentInt(token["Guid"]!);
    }

    public class ChargeBlockMeterEffectDescriptor : CardComponent
    {
        public ChargeBlockMeterEffectDescriptor() { }
        public ChargeBlockMeterEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("ChargeAmount", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["ChargeAmount"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentInt(token["ChargeAmount"]!);
    }

    public class CombatEndTrigger : CardComponent
    {
        public CombatEndTrigger() { }
        public CombatEndTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Continuous : CardComponent
    {
        public Continuous() { }
        public Continuous(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class CopyCardEffectDescriptor : CardComponent
    {
        public CopyCardEffectDescriptor() { }
        public CopyCardEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["GrantTeamup"] = true,
            ["ForceFaceDown"] = false,
            ["CreateInFront"] = true
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("GrantTeamup", new ComponentBool(token["GrantTeamup"]!)),
            new ComponentNode("ForceFaceDown", new ComponentBool(token["ForceFaceDown"]!)),
            new ComponentNode("CreateInFront", new ComponentBool(token["CreateInFront"]!))
        }));
    }

    public class CopyStatsEffectDescriptor : CardComponent
    {
        public CopyStatsEffectDescriptor() { }
        public CopyStatsEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class CreateCardEffectDescriptor : CardComponent
    {
        public CreateCardEffectDescriptor() { }
        public CreateCardEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["CardGuid"] = 0,
            ["ForceFaceDown"] = false
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("CardGuid", new ComponentInt(token["CardGuid"]!)),
            new ComponentNode("ForceFaceDown", new ComponentBool(token["ForceFaceDown"]!))
        }));
    }

    public class CreateCardFromSubsetEffectDescriptor : CardComponent
    {
        public CreateCardFromSubsetEffectDescriptor() { }
        public CreateCardFromSubsetEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["ForceFaceDown"] = false,
            ["SubsetQuery"] = null
        };

        protected override ComponentValue? DefaultValue(JToken token)
        {
            var component = ComponentNode.ParseComponent(token["SubsetQuery"]!);
            return new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
            {
                new ComponentNode("ForceFaceDown", new ComponentBool(token["ForceFaceDown"]!)),
                component is null ? new ComponentNode("SubsetQuery", new ComponentNull(token["SubsetQuery"]!)) : new ComponentNode("SubsetQuery", component.IsolatedObject, component.AllowAdd, component.FullToken) 
                { 
                    ComponentName = component.GetType().Name 
                }
            }));
        }
    }

    public class CreateCardInDeckEffectDescriptor : CardComponent
    {
        public CreateCardInDeckEffectDescriptor() { }
        public CreateCardInDeckEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["CardGuid"] = 0,
            ["AmountToCreate"] = 0,
            ["DeckPosition"] = "Random"
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("CardGuid", new ComponentInt(token["CardGuid"]!)),
            new ComponentNode("AmountToCreate", new ComponentInt(token["AmountToCreate"]!)),
            new ComponentNode("DeckPosition", new ComponentString(token["DeckPosition"]!))
        }));
    }

    public class CreateInFront : CardComponent
    {
        public CreateInFront() { }
        public CreateInFront(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class DamageEffectDescriptor : CardComponent
    {
        public DamageEffectDescriptor() { }
        public DamageEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("DamageAmount", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["DamageAmount"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentInt(token["DamageAmount"]!);
    }

    public class DamageEffectRedirector : CardComponent
    {
        public DamageEffectRedirector() { }
        public DamageEffectRedirector(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class DamageEffectRedirectorDescriptor : CardComponent
    {
        public DamageEffectRedirectorDescriptor() { }
        public DamageEffectRedirectorDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class DamageTrigger : CardComponent
    {
        public DamageTrigger() { }
        public DamageTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Deadly : TraitCardComponent
    {
        public Deadly() { }
        public Deadly(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class DestroyCardEffectDescriptor : CardComponent
    {
        public DestroyCardEffectDescriptor() { }
        public DestroyCardEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class DestroyCardTrigger : CardComponent
    {
        public DestroyCardTrigger() { }
        public DestroyCardTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class DiscardFromPlayTrigger : CardComponent
    {
        public DiscardFromPlayTrigger() { }
        public DiscardFromPlayTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class DrawCardEffectDescriptor : CardComponent
    {
        public DrawCardEffectDescriptor() { }
        public DrawCardEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("DrawAmount", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["DrawAmount"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentInt(token["DrawAmount"]!);
    }

    public class DrawCardFromSubsetEffectDescriptor : CardComponent
    {
        public DrawCardFromSubsetEffectDescriptor() { }
        public DrawCardFromSubsetEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["SubsetQuery"] = null,
            ["DrawAmount"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token)
        {
            var component = ComponentNode.ParseComponent(token["SubsetQuery"]!);
            return new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
            {
                component is null ? new ComponentNode("SubsetQuery", new ComponentNull(token["SubsetQuery"]!)) : new ComponentNode("SubsetQuery", component.IsolatedObject, component.AllowAdd, component.FullToken) 
                {
                    ComponentName = component.GetType().Name 
                },
                new ComponentNode("DrawAmount", new ComponentInt(token["DrawAmount"]!))
            }));
        }
    }

    public class DrawCardFromSubsetTrigger : CardComponent
    {
        public DrawCardFromSubsetTrigger() { }
        public DrawCardFromSubsetTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class DrawCardTrigger : CardComponent
    {
        public DrawCardTrigger() { }
        public DrawCardTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class DrawnCardCostMultiplier : CardComponent
    {
        public DrawnCardCostMultiplier() { }
        public DrawnCardCostMultiplier(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("Divider", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["Divider"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentInt(token["Divider"]!);
    }

    public class EffectEntitiesDescriptor : CardComponent
    {
        public EffectEntitiesDescriptor() : base() { }
        public EffectEntitiesDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override bool AllowAdd => true;

        protected override JToken DefaultToken => new JObject
        {
            ["entities"] = new JArray
            {
                new JObject
                {
                    ["components"] = new JArray()
                }
            }
        };

        protected override ComponentValue? DefaultValue(JToken token)
        {
            var entities = (JArray)token["entities"]!;
            return new ComponentArray(entities, entities.Select(entity =>
            {
                var components = (JArray)entity["components"]!;
                return new ComponentArray(components, components.Select(c => ComponentNode.ParseComponent(c)).Where(v => v is not null).Select(v => v!));
            }));
        }
    }

    public class EffectEntityGrouping : CardComponent
    {
        public EffectEntityGrouping() { }
        public EffectEntityGrouping(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("AbilityGroupId", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["AbilityGroupId"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentInt(token["AbilityGroupId"]!);
    }

    public class EffectValueCondition : CardComponent
    {
        public EffectValueCondition() { }
        public EffectValueCondition(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["EffectValue"] = "TotalBuffAmount",
            ["ComparisonOperator"] = "GreaterOrEqual",
            ["ValueAmount"] = 1
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("EffectValue", new ComponentString(token["EffectValue"]!)),
            new ComponentNode("ComparisonOperator", new ComponentString(token["ComparisonOperator"]!)),
            new ComponentNode("ValueAmount", new ComponentInt(token["ValueAmount"]!))
        }));
    }

    public class EffectValueDescriptor : CardComponent
    {
        public EffectValueDescriptor() { }
        public EffectValueDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["DestToSourceMap"] = new JObject()
        };

        protected override ComponentValue? DefaultValue(JToken token)
        {
            var map = (JObject)token["DestToSourceMap"]!;
            return new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
            {
                new ComponentNode("DestToSourceMap", new ComponentObject(map, new FullObservableCollection<ComponentNode>(map.Properties().Select(p => new ComponentNode(p.Name, new ComponentString(p.Value))))))
            }));
        }
    }

    public class EnterBoardTrigger : CardComponent
    {
        public EnterBoardTrigger() { }
        public EnterBoardTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Environment : CardComponent
    {
        public Environment() { }
        public Environment(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class EvolutionRestriction : SingleQueryComponent
    {
        public EvolutionRestriction() { }
        public EvolutionRestriction(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Evolvable : CardComponent
    {
        public Evolvable() { }
        public Evolvable(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class ExtraAttackEffectDescriptor : CardComponent
    {
        public ExtraAttackEffectDescriptor() { }
        public ExtraAttackEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class ExtraAttackTrigger : CardComponent
    {
        public ExtraAttackTrigger() { }
        public ExtraAttackTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Frenzy : TraitCardComponent
    {
        public Frenzy() { }
        public Frenzy(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class FromBurst : CardComponent
    {
        public FromBurst() { }
        public FromBurst(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class GainSunEffectDescriptor : CardComponent
    {
        public GainSunEffectDescriptor() { }
        public GainSunEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["Amount"] = 0,
            ["Duration"] = "EndOfTurn",
            ["ActivationTime"] = "Immediate"
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("Amount", new ComponentInt(token["Amount"]!)),
            new ComponentNode("Duration", new ComponentString(token["Duration"]!)),
            new ComponentNode("ActivationTime", new ComponentString(token["ActivationTime"]!))
        }));
    }

    public class GrantAbilityEffectDescriptor : CardComponent
    {
        public GrantAbilityEffectDescriptor() { }
        public GrantAbilityEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["GrantableAbilityType"] = "",
            ["Duration"] = "Permanent",
            ["AbilityValue"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("GrantableAbilityType", new ComponentString(token["GrantableAbilityType"]!)),
            new ComponentNode("Duration", new ComponentString(token["Duration"]!)),
            new ComponentNode("AbilityValue", new ComponentInt(token["AbilityValue"]!))
        }));
    }

    public class GrantedTriggeredAbilities : CardComponent
    {
        public GrantedTriggeredAbilities() { }
        public GrantedTriggeredAbilities(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["a"] = new JArray
            {
                new JObject
                {
                    ["g"] = 0,
                    ["vt"] = 0,
                    ["va"] = 0
                }
            }
        };

        protected override ComponentValue? DefaultValue(JToken token)
        {
            var a = (JArray)token["a"]!;
            return new ComponentArray(a, a.Select(v => new ComponentObject(v, new FullObservableCollection<ComponentNode>(new[]
            {
                new ComponentNode("g", new ComponentInt(v["g"]!)),
                new ComponentNode("vt", new ComponentInt(v["vt"]!)),
                new ComponentNode("va", new ComponentInt(v["va"]!)),
            }))));
        }
    }

    public class HealEffectDescriptor : CardComponent
    {
        public HealEffectDescriptor() { }
        public HealEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("HealAmount", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["HealAmount"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentInt(token["HealAmount"]!);
    }

    public class HealTrigger : CardComponent
    {
        public HealTrigger() { }
        public HealTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Health : CardComponent
    {
        public Health() { }
        public Health(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["MaxHealth"] = new JObject
            {
                ["BaseValue"] = 0
            },
            ["CurrentDamage"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[] 
        {
            new ComponentNode("MaxHealth", new ComponentInt(token["MaxHealth"]!["BaseValue"]!)),
            new ComponentNode("CurrentDamage", new ComponentInt(token["CurrentDamage"]!))
        }));
    }

    public class HeraldEntities : CardComponent
    {
        public HeraldEntities() { }
        public HeraldEntities(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("ids", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["ids"] = new JArray { 0 }
        };

        protected override ComponentValue? DefaultValue(JToken token)
        {
            var ids = token["ids"]!;
            return new ComponentArray(ids, ids.Select(id => new ComponentInt(id)));
        }
    }

    public class HeroHealthMultiplier : CardComponent
    {
        public HeroHealthMultiplier() { }
        public HeroHealthMultiplier(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["Faction"] = "Zombies",
            ["Divider"] = 1
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("Faction", new ComponentString(token["Faction"]!)),
            new ComponentNode("Divider", new ComponentInt(token["Divider"]!))
        }));
    }

    public class LaneCombatEndTrigger : CardComponent
    {
        public LaneCombatEndTrigger() { }
        public LaneCombatEndTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class LaneCombatStartTrigger : CardComponent
    {
        public LaneCombatStartTrigger() { }
        public LaneCombatStartTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class MixedUpGravediggerEffectDescriptor : CardComponent
    {
        public MixedUpGravediggerEffectDescriptor() { }
        public MixedUpGravediggerEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class ModifySunCostEffectDescriptor : CardComponent
    {
        public ModifySunCostEffectDescriptor() { }
        public ModifySunCostEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["SunCostAmount"] = 0,
            ["BuffDuration"] = "Permanent"
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("SunCostAmount", new ComponentInt(token["SunCostAmount"]!)),
            new ComponentNode("BuffDuration", new ComponentString(token["BuffDuration"]!))
        }));
    }

    public class MoveCardToLanesEffectDescriptor : CardComponent
    {
        public MoveCardToLanesEffectDescriptor() { }
        public MoveCardToLanesEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class MoveTrigger : CardComponent
    {
        public MoveTrigger() { }
        public MoveTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Multishot : CardComponent
    {
        public Multishot() { }
        public Multishot(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class OncePerGameCondition : CardComponent
    {
        public OncePerGameCondition() { }
        public OncePerGameCondition(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class OncePerTurnCondition : CardComponent
    {
        public OncePerTurnCondition() { }
        public OncePerTurnCondition(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class PersistsAfterTransform : CardComponent
    {
        public PersistsAfterTransform() { }
        public PersistsAfterTransform(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Plants : CardComponent
    {
        public Plants() { }
        public Plants(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class PlayTrigger : CardComponent
    {
        public PlayTrigger() { }
        public PlayTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class PlayerInfoCondition : CardComponent
    {
        public PlayerInfoCondition() { }
        public PlayerInfoCondition(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["Faction"] = "Plants",
            ["Query"] = null
        };

        protected override ComponentValue? DefaultValue(JToken token)
        {
            var component = ComponentNode.ParseComponent(token["Query"]!);
            return new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
            {
                new ComponentNode("Faction", new ComponentString(token["Faction"]!)),
                component is null ? new ComponentNode("Query", new ComponentNull(token["Query"]!)) : new ComponentNode("Query", component.IsolatedObject, component.AllowAdd, component.FullToken) 
                { 
                    ComponentName = component.GetType().Name 
                }
            }));
        }
    }

    public class PlaysFaceDown : CardComponent
    {
        public PlaysFaceDown() { }
        public PlaysFaceDown(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class PrimarySuperpower : CardComponent
    {
        public PrimarySuperpower() { }
        public PrimarySuperpower(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class PrimaryTargetFilter : CardComponent
    {
        public PrimaryTargetFilter() { }
        public PrimaryTargetFilter(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["SelectionType"] = "All",
            ["NumTargets"] = 0,
            ["TargetScopeType"] = "All",
            ["TargetScopeSortValue"] = "None",
            ["TargetScopeSortMethod"] = "None",
            ["AdditionalTargetType"] = "None",
            ["AdditionalTargetQuery"] = null,
            ["OnlyApplyEffectsOnAdditionalTargets"] = false,
            ["Query"] = null
        };

        protected override ComponentValue? DefaultValue(JToken token)
        {
            var additionalQuery = ComponentNode.ParseComponent(token["AdditionalTargetQuery"]!);
            var query = ComponentNode.ParseComponent(token["Query"]!);
            return new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
            {
                new ComponentNode("SelectionType", new ComponentString(token["SelectionType"]!)),
                new ComponentNode("NumTargets", new ComponentInt(token["NumTargets"]!)),
                new ComponentNode("TargetScopeType", new ComponentString(token["TargetScopeType"]!)),
                new ComponentNode("TargetScopeSortValue", new ComponentString(token["TargetScopeSortValue"]!)),
                new ComponentNode("TargetScopeSortMethod", new ComponentString(token["TargetScopeSortMethod"]!)),
                new ComponentNode("AdditionalTargetType", new ComponentString(token["AdditionalTargetType"]!)),
                additionalQuery is null ? new ComponentNode("AdditionalTargetQuery", new ComponentNull(token["AdditionalTargetQuery"]!)) : new ComponentNode("AdditionalTargetQuery", additionalQuery.IsolatedObject, additionalQuery.AllowAdd, additionalQuery.FullToken) 
                {
                    ComponentName = additionalQuery.GetType().Name 
                },
                new ComponentNode("OnlyApplyEffectsOnAdditionalTargets", new ComponentBool(token["OnlyApplyEffectsOnAdditionalTargets"]!)),
                query is null ? new ComponentNode("Query", new ComponentNull(token["Query"]!)) : new ComponentNode("Query", query.IsolatedObject, query.AllowAdd, query.FullToken) 
                {
                    ComponentName = query.GetType().Name 
                }
            }));
        }
    }

    public class QueryEntityCondition : CardComponent
    {
        public QueryEntityCondition() { }
        public QueryEntityCondition(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["Finder"] = null,
            ["ConditionEvaluationType"] = "All",
            ["Query"] = null
        };

        protected override ComponentValue? DefaultValue(JToken token)
        {
            var finder = ComponentNode.ParseComponent(token["Finder"]!);
            var query = ComponentNode.ParseComponent(token["Query"]!);
            return new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
            {
                finder is null ? new ComponentNode("Finder", new ComponentNull(token["Finder"]!)) : new ComponentNode("Finder", finder.IsolatedObject, finder.AllowAdd, finder.FullToken) 
                {
                    ComponentName = finder.GetType().Name
                },
                new ComponentNode("ConditionEvaluationType", new ComponentString(token["ConditionEvaluationType"]!)),
                query is null ? new ComponentNode("Query", new ComponentNull(token["Query"]!)) : new ComponentNode("Query", query.IsolatedObject, query.AllowAdd, query.FullToken) 
                {
                    ComponentName = query.GetType().Name
                }
            }));
        }
    }

    public class QueryMultiplier : CardComponent
    {
        public QueryMultiplier() { }
        public QueryMultiplier(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["Query"] = null,
            ["Divider"] = 1
        };

        protected override ComponentValue? DefaultValue(JToken token)
        {
            var component = ComponentNode.ParseComponent(token["Query"]!);
            return new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
            {
                component is null ? new ComponentNode("Query", new ComponentNull(token["Query"]!)) : new ComponentNode("Query", component.IsolatedObject, component.AllowAdd, component.FullToken) 
                {
                    ComponentName = component.GetType().Name
                },
                new ComponentNode("Divider", new ComponentInt(token["Divider"]!))
            }));
        }
    }

    public class Rarity : CardComponent
    {
        public Rarity() { }
        public Rarity(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("Value", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["Value"] = default(CardRarity).GetInternalKey()
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentString(token["Value"]!);
    }

    public class ReturnToHandFromPlayEffectDescriptor : CardComponent
    {
        public ReturnToHandFromPlayEffectDescriptor() { }
        public ReturnToHandFromPlayEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class ReturnToHandTrigger : CardComponent
    {
        public ReturnToHandTrigger() { }
        public ReturnToHandTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class RevealPhaseEndTrigger : CardComponent
    {
        public RevealPhaseEndTrigger() { }
        public RevealPhaseEndTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class RevealTrigger : CardComponent
    {
        public RevealTrigger() { }
        public RevealTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class SecondaryTargetFilter : PrimaryTargetFilter
    {
        public SecondaryTargetFilter() { }
        public SecondaryTargetFilter(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class SelfEntityFilter : SingleQueryComponent
    {
        public SelfEntityFilter() { }
        public SelfEntityFilter(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class SelfLaneEntityFilter : SingleQueryComponent
    {
        public SelfLaneEntityFilter() { }
        public SelfLaneEntityFilter(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class SetStatEffectDescriptor : CardComponent
    {
        public SetStatEffectDescriptor() { }
        public SetStatEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["StatType"] = "Attack",
            ["Value"] = 0,
            ["ModifyOperation"] = "Set",
            ["StripNoncontinousModifiers"] = true
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("StatType", new ComponentString(token["StatType"]!)),
            new ComponentNode("Value", new ComponentInt(token["Value"]!)),
            new ComponentNode("ModifyOperation", new ComponentString(token["ModifyOperation"]!)),
            new ComponentNode("StripNoncontinousModifiers", new ComponentBool(token["StripNoncontinousModifiers"]!))
        }));
    }

    public class ShowTriggeredIcon : CardComponent
    {
        public ShowTriggeredIcon() { }
        public ShowTriggeredIcon(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("abilities", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["abilities"] = new JArray()
        };

        protected override ComponentValue? DefaultValue(JToken token)
        {
            var abilities = (JArray)token["abilities"]!;
            return new ComponentArray(abilities, abilities.Select(a => new ComponentInt(a)));
        }
    }

    public class SlowEffectDescriptor : CardComponent
    {
        public SlowEffectDescriptor() { }
        public SlowEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class SlowedTrigger : CardComponent
    {
        public SlowedTrigger() { }
        public SlowedTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class SplashDamage : DamageEffectDescriptor
    {
        public SplashDamage() { }
        public SplashDamage(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Springboard : CardComponent
    {
        public Springboard() { }
        public Springboard(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Strikethrough : TraitCardComponent
    {
        public Strikethrough() { }
        public Strikethrough(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Subtypes : CardComponent
    {
        public Subtypes() { }
        public Subtypes(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override bool AllowAdd => true;

        protected override JToken DefaultToken => new JObject
        {
            ["subtypes"] = new JArray()
        };

        protected override ComponentValue? DefaultValue(JToken token)
        {
            var subtypes = (JArray)token["subtypes"]!;
            return new ComponentArray(subtypes, subtypes.Select(s => new ComponentInt(s)));
        }
    }

    public class SunCost : CardComponent
    {
        public SunCost() { }
        public SunCost(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("SunCostValue", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["SunCostValue"] = new JObject
            {
                ["BaseValue"] = 0
            }
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentInt(token["SunCostValue"]!["BaseValue"]!);
    }

    public class SunGainedMultiplier : CardComponent
    {
        public SunGainedMultiplier() { }
        public SunGainedMultiplier(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["Faction"] = "Plants",
            ["Divider"] = 0,
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new FullObservableCollection<ComponentNode>(new[]
        {
            new ComponentNode("Faction", new ComponentString(token["Faction"]!)),
            new ComponentNode("Divider", new ComponentInt(token["Divider"]!))
        }));
    }

    public class Superpower : CardComponent
    {
        public Superpower() { }
        public Superpower(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Surprise : CardComponent
    {
        public Surprise() { }
        public Surprise(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class SurprisePhaseStartTrigger : CardComponent
    {
        public SurprisePhaseStartTrigger() { }
        public SurprisePhaseStartTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Tags : CardComponent
    {
        public Tags() { }
        public Tags(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override bool AllowAdd => true;

        protected override JToken DefaultToken => new JObject
        {
            ["tags"] = new JArray()
        };

        protected override ComponentValue? DefaultValue(JToken token)
        {
            var tags = (JArray)token["tags"]!;
            return new ComponentArray(tags, tags.Select(tag => new ComponentString(tag)));
        }
    }

    public class TargetAttackMultiplier : CardComponent
    {
        public TargetAttackMultiplier() { }
        public TargetAttackMultiplier(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("Divider", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["Divider"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentInt(token["Divider"]!);
    }

    public class TargetAttackOrHealthMultiplier : TargetAttackMultiplier
    {
        public TargetAttackOrHealthMultiplier() { }
        public TargetAttackOrHealthMultiplier(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class TargetHealthMultiplier : TargetAttackMultiplier
    {
        public TargetHealthMultiplier() { }
        public TargetHealthMultiplier(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Teamup : TraitCardComponent
    {
        public Teamup() { }
        public Teamup(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class TransformIntoCardFromSubsetEffectDescriptor : CardComponent
    {
        public TransformIntoCardFromSubsetEffectDescriptor() { }
        public TransformIntoCardFromSubsetEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["SubsetQuery"] = null
        };

        protected override ComponentValue? DefaultValue(JToken token)
        {
            var component = ComponentNode.ParseComponent(token["SubsetQuery"]!);
            return new ComponentObject(token["SubsetQuery"]!, new FullObservableCollection<ComponentNode>(new[]
            {
                component is null ? new ComponentNode("SubsetQuery", new ComponentNull(token["SubsetQuery"]!)) : new ComponentNode("SubsetQuery", component.IsolatedObject, component.AllowAdd, component.FullToken)
                {
                    ComponentName = component.GetType().Name
                }
            }));
        }
    }

    public class TransformWithCreationSource : CardComponent
    {
        public TransformWithCreationSource() { }
        public TransformWithCreationSource(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new FullObservableCollection<ComponentNode>(new[] {
            new ComponentNode("SourceGuid", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["SourceGuid"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentInt(token["SourceGuid"]!);
    }

    public class TriggerSourceFilter : SingleQueryComponent
    {
        public TriggerSourceFilter() { }
        public TriggerSourceFilter(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class TriggerTargetFilter : SingleQueryComponent
    {
        public TriggerTargetFilter() { }
        public TriggerTargetFilter(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Truestrike : TraitCardComponent
    {
        public Truestrike() { }
        public Truestrike(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class TurnIntoGravestoneEffectDescriptor : CardComponent
    {
        public TurnIntoGravestoneEffectDescriptor() { }
        public TurnIntoGravestoneEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class TurnStartTrigger : CardComponent
    {
        public TurnStartTrigger() { }
        public TurnStartTrigger(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Untrickable : TraitCardComponent
    {
        public Untrickable() { }
        public Untrickable(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Unusable : CardComponent
    {
        public Unusable() { }
        public Unusable(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Zombies : CardComponent
    {
        public Zombies() { }
        public Zombies(JToken token, JToken fullToken) : base(token, fullToken) { }
    }
}
