using Newtonsoft.Json.Linq;
using System.Linq;

namespace PvZHCardEditor.Components
{
    public class Card : CardComponent
    {
        public Card() : base() { }
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

    public class BoardAbility : CardComponent
    {
        public BoardAbility() : base() { }
        public BoardAbility(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class Tags : CardComponent
    {
        public Tags() : base() { }
        public Tags(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override bool AllowAdd => true;

        protected override JToken DefaultToken => new JObject
        {
            ["tags"] = new JArray()
        };

        protected override ComponentValue? DefaultValue(JToken token)
        {
            var tags = (JArray)token["tags"]!;
            return new ComponentArray(tags, tags.Select(tag => new ComponentString(tag!)));
        }
    }
}
