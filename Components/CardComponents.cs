using Newtonsoft.Json.Linq;
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

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new ComponentCollection<ComponentNode>(new[] {
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

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new ComponentCollection<ComponentNode>(new[] {
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

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new ComponentCollection<ComponentNode>(new[] {
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

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new ComponentCollection<ComponentNode>(new[]
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

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new ComponentCollection<ComponentNode>(new[] { 
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

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new ComponentCollection<ComponentNode>(new[] {
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

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new ComponentCollection<ComponentNode>(new[]
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

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new ComponentCollection<ComponentNode>(new[]
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
            return new ComponentObject(token, new ComponentCollection<ComponentNode>(new[]
            {
                new ComponentNode("ForceFaceDown", new ComponentBool(token["ForceFaceDown"]!)),
                component is null ? new ComponentNode("SubsetQuery", new ComponentNull(token["SubsetQuery"]!)) : new ComponentNode("SubsetQuery", component.IsolatedObject)
                {
                    ComponentName = component.GetType().Name,
                    AllowAdd = component.AllowAdd
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

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new ComponentCollection<ComponentNode>(new[]
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

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new ComponentCollection<ComponentNode>(new[] {
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

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new ComponentCollection<ComponentNode>(new[] {
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
            return new ComponentObject(token, new ComponentCollection<ComponentNode>(new[]
            {
                component is null ? new ComponentNode("SubsetQuery", new ComponentNull(token["SubsetQuery"]!)) : new ComponentNode("SubsetQuery", component.IsolatedObject)
                {
                    ComponentName = component.GetType().Name,
                    AllowAdd = component.AllowAdd
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

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new ComponentCollection<ComponentNode>(new[] {
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

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new ComponentCollection<ComponentNode>(new[] 
        {
            new ComponentNode("MaxHealth", new ComponentInt(token["MaxHealth"]!["BaseValue"]!)),
            new ComponentNode("CurrentDamage", new ComponentInt(token["CurrentDamage"]!))
        }));
    }

    public class Rarity : CardComponent
    {
        public Rarity() { }
        public Rarity(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new ComponentCollection<ComponentNode>(new[] {
            new ComponentNode("Value", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["Value"] = default(CardRarity).GetInternalKey()
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentString(token["Value"]!);
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

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new ComponentCollection<ComponentNode>(new[] {
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
}
