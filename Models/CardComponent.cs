using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactiveUI;

namespace PvZHCardEditor.Models;

public abstract class CardComponent : ReactiveObject
{
    public string GetFullTypeString()
    {
        var name = GetType().Name;
        string patchedName;
        if (GetType() == typeof(GrantTriggeredAbilityEffectDescriptor))
            patchedName = "Effects." + name;
        else if (name.EndsWith("Query"))
            patchedName = "Queries." + name;
        else if (name.EndsWith("Component"))
            patchedName = "Components." + name[..name.LastIndexOf("Component", StringComparison.Ordinal)];
        else
            patchedName = "Components." + name;
        return $"PvZCards.Engine.{patchedName}, EngineLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
    }

    public static Type? ParseFullTypeString(string s)
    {
        var match = Regex.Match(s, "^PvZCards\\.Engine\\.([a-zA-Z0-9_]+)\\.([a-zA-Z0-9_]+),");
        var ns = match.Groups[1].Value;
        var name = match.Groups[2].Value;
        var type = Type.GetType($"PvZHCardEditor.Models.{name}");
        if (type != null)
            return type;
        if (ns == "Components")
            type = Type.GetType($"PvZHCardEditor.Models.{name}Component");
        return type;
    }

    public static string GetHasComponentTypeString(string s)
    {
        return $"PvZCards.Engine.{s}, EngineLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
    }

    public static string ParseHasComponentTypeString(string s)
    {
        var match = Regex.Match(s, "^PvZCards\\.Engine\\.([a-zA-Z0-9_.]+),");
        return match.Groups[1].Value;
    }
}

public class CardComponentConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        var token = serializer.DefaultFromObject(value);
        var component = (CardComponent)value;
        var obj = new JObject
        {
            ["$type"] = component.GetFullTypeString(),
            ["$data"] = token
        };
        obj.WriteTo(writer);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        var obj = JObject.Load(reader);
        var type = CardComponent.ParseFullTypeString((string)obj["$type"]!);
        return type == null ? null : obj["$data"]!.DefaultToObject(type, serializer);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.IsSubclassOf(typeof(CardComponent)) || objectType == typeof(CardComponent);
    }
}

public class ArmorComponent : CardComponent
{
    public int ArmorAmount { get; set; }
}

public class GrantTriggeredAbilityEffectDescriptor : CardComponent
{
}

public class HasComponentQuery : CardComponent
{
    public CardComponent? Query { get; set; }
}