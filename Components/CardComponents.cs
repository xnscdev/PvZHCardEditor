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

    public class BoardAbility : CardComponent
    {
        public BoardAbility() { }
        public BoardAbility(JToken token, JToken fullToken) : base(token, fullToken) { }
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
