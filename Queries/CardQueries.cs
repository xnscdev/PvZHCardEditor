using Newtonsoft.Json.Linq;
using System.Linq;

namespace PvZHCardEditor.Queries
{
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

    public class SelfQuery : CardComponent
    {
        public SelfQuery() : base() { }
        public SelfQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
    }

    public class SubtypeQuery : CardComponent
    {
        public SubtypeQuery() { }
        public SubtypeQuery(JToken token, JToken fullToken) : base(token, fullToken) { }

        public override ComponentValue IsolatedObject => new ComponentObject(Token, new ComponentCollection<ComponentNode>(new[] {
            new ComponentNode("Subtype", Value!)
        }));

        protected override JToken DefaultToken => new JObject
        {
            ["Subtype"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentInt(token["Subtype"]!);
    }
}
