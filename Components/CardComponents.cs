using Newtonsoft.Json.Linq;

namespace PvZHCardEditor.Components
{
    public class Card : ComponentNode
    {
        public Card(JToken token) : base(nameof(Card), new ComponentInt(token["Guid"]!)) { }
    }

    public class BoardAbility : ComponentNode
    {
        public BoardAbility(JToken token) : base(nameof(BoardAbility), token) { }
    }
}
