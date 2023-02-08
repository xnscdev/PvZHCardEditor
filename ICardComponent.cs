using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PvZHCardEditor
{
    public interface ICardComponent
    {
        public TreeViewNode Node { get; }

        private static readonly Dictionary<string, Type> _componentTypes = new()
        {
            { "Card", typeof(CardComponent) },
            { "Unusable", typeof(UnusableComponent) },
            { "BoardAbility", typeof(BoardAbilityComponent) }
        };

        public static Type? ParseComponentType(JToken token)
        {
            var match = Regex.Match((string)token["$type"]!, "^PvZCards\\.Engine\\.Components\\.([a-zA-Z0-9_]+),");
            return _componentTypes.TryGetValue(match.Groups[1].Value, out var type) ? type : null;
        }

        public static ICardComponent? ParseComponent(JToken token)
        {
            var type = ParseComponentType(token);
            if (type is null)
                return null;

            var info = type.GetConstructor(new[] { typeof(JToken) });
            if (info is not null)
                return (ICardComponent)info.Invoke(new[] { token["$data"]! });

            info = type.GetConstructor(Array.Empty<Type>());
            return info is null ? null : (ICardComponent)info.Invoke(null);
        }
    }

    public class CardComponent : ICardComponent
    {
        private readonly TreeViewCompoundNode _node;
        private readonly int _id;

        public TreeViewNode Node => _node;

        public CardComponent(JToken token)
        {
            _id = (int)token["Guid"]!;
            _node = new TreeViewCompoundNode("Card", new TreeViewNode[] { new($"Guid = {_id}") });
        }
    }

    public class UnusableComponent : ICardComponent
    {
        public TreeViewNode Node => new("Unusable");
    }

    public class BoardAbilityComponent : ICardComponent
    {
        public TreeViewNode Node => new("BoardAbility");
    }
}
