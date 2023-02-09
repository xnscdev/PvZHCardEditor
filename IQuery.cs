using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PvZHCardEditor
{
    public interface IQuery
    {
        public TreeViewNode Node { get; }

        public static Type? ParseQueryType(string s)
        {
            var match = Regex.Match(s, "^PvZCards\\.Engine\\.Queries\\.([a-zA-Z0-9_]+),");
            return Type.GetType($"PvZHCardEditor.{match.Groups[1].Value}");
        }

        public static IQuery? ParseQuery(JToken token)
        {
            if (token.Type == JTokenType.Null)
                return null;

            var type = ParseQueryType((string)token["$type"]!);
            if (type is null)
                return null;

            var info = type.GetConstructor(new[] { typeof(JToken) });
            if (info is not null)
                return (IQuery)info.Invoke(new[] { token["$data"]! });

            info = type.GetConstructor(Array.Empty<Type>());
            return info is null ? null : (IQuery)info.Invoke(null);
        }
    }

    public abstract class SingletonQuery<T> : IQuery
    {
        private readonly string _name;
        private readonly T _value;

        public TreeViewNode Node => new($"{_name} = {_value}");

        public SingletonQuery(string name, T value)
        {
            _name = name;
            _value = value;
        }
    }

    #region Empty Queries

    public class SelfQuery : IQuery
    {
        public TreeViewNode Node => new("SelfQuery");
    }

    public class WillTriggerEffectsQuery : IQuery
    {
        public TreeViewNode Node => new("WillTriggerEffectsQuery");
    }

    public class WillTriggerOnDeathEffectsQuery : IQuery
    {
        public TreeViewNode Node => new("WillTriggerOnDeathEffectsQuery");
    }

    public class TargetableInPlayFighterQuery : IQuery
    {
        public TreeViewNode Node => new("TargetableInPlayFighterQuery");
    }

    public class IsAliveQuery : IQuery
    {
        public TreeViewNode Node => new("IsAliveQuery");
    }

    #endregion

    #region Singleton Queries

    public class SubtypeQuery : SingletonQuery<CardTribe>
    {
        public SubtypeQuery(JToken token) : base("SubtypeQuery", (CardTribe)(int)token["Subtype"]!) { }
    }

    public class InSameLaneQuery : SingletonQuery<string>
    {
        public InSameLaneQuery(JToken token) : base("InSameLaneQuery", (string)token["OriginEntityType"]!) { }
    }

    #endregion

    #region Other Queries

    public class HasComponentQuery : IQuery
    {
        private readonly Type _componentType;

        public TreeViewNode Node => new($"HasComponentQuery = {_componentType.Name.Replace("Component", "")}");

        public HasComponentQuery(JToken token)
        {
            _componentType = ICardComponent.ParseComponentType((string)token["ComponentType"]!)!;
        }
    }

    public class OnTerrainQuery : IQuery
    {
        private readonly Type _componentType;

        public TreeViewNode Node => new($"OnTerrainQuery = {_componentType.Name.Replace("Component", "")}");

        public OnTerrainQuery(JToken token)
        {
            _componentType = ICardComponent.ParseComponentType((string)token["TerrainType"]!)!;
        }
    }

    public class CompositeAllQuery : IQuery
    {
        private readonly List<IQuery> _queries = new();
        private readonly TreeViewNode _node;

        public TreeViewNode Node => _node;

        public CompositeAllQuery(JToken token)
        {
            var queries = (JArray)token["queries"]!;
            foreach (var data in queries)
            {
                var query = IQuery.ParseQuery(data);
                if (query is not null)
                    _queries.Add(query);
            }

            _node = new TreeViewCompoundNode("CompositeAllQuery", _queries.Select(q => q.Node));
        }
    }

    public class CompositeAnyQuery : IQuery
    {
        private readonly List<IQuery> _queries = new();
        private readonly TreeViewNode _node;

        public TreeViewNode Node => _node;

        public CompositeAnyQuery(JToken token)
        {
            var queries = (JArray)token["queries"]!;
            foreach (var data in queries)
            {
                var query = IQuery.ParseQuery(data);
                if (query is not null)
                    _queries.Add(query);
            }

            _node = new TreeViewCompoundNode("CompositeAnyQuery", _queries.Select(q => q.Node));
        }
    }

    #endregion
}
