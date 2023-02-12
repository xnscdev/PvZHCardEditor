using Newtonsoft.Json.Linq;
using System.Linq;

namespace PvZHCardEditor.Queries
{
    public class SelfQuery : CardComponent
    {
        public SelfQuery() : base() { }
        public SelfQuery(JToken token, JToken fullToken) : base(token, fullToken) { }
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
            return new ComponentArray(queries, queries.Select(query => ComponentNode.ParseComponent(query)!.IsolatedObject));
        }
    }
}
