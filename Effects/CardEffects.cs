using Newtonsoft.Json.Linq;

namespace PvZHCardEditor.Effects
{
    public class GrantTriggeredAbilityEffectDescriptor : CardComponent
    {
        public GrantTriggeredAbilityEffectDescriptor() { }
        public GrantTriggeredAbilityEffectDescriptor(JToken token, JToken fullToken) : base(token, fullToken) { }

        protected override JToken DefaultToken => new JObject
        {
            ["AbilityGuid"] = 0,
            ["AbilityValueType"] = "None",
            ["AbilityValueAmount"] = 0
        };

        protected override ComponentValue? DefaultValue(JToken token) => new ComponentObject(token, new ComponentCollection<ComponentNode>(new[]
        {
            new ComponentNode("AbilityGuid", new ComponentInt(token["AbilityGuid"]!)),
            new ComponentNode("AbilityValueType", new ComponentString(token["AbilityValueType"]!)),
            new ComponentNode("AbilityValueAmount", new ComponentInt(token["AbilityValueAmount"]!))
        }));
    }
}
