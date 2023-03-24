using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PvZHCardEditor.Models;

[JsonConverter(typeof(EntityComponentConverter))]
public abstract class EntityComponent : ComponentValue
{
    [JsonIgnore] public override string Text => GetDisplayTypeString();
    [JsonIgnore] public override FullObservableCollection<ComponentProperty> Children { get; } = new();

    public override Task Edit()
    {
        Console.WriteLine("Implement GUI for selecting component type");
        throw new NotImplementedException();
    }

    public string GetDisplayTypeString()
    {
        var name = GetType().Name;
        return name.EndsWith("Component") ? name[..name.LastIndexOf("Component", StringComparison.Ordinal)] : name;
    }

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

public class EntityComponentConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        var token = serializer.DefaultFromObject(value);
        var component = (EntityComponent)value;
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
        var type = EntityComponent.ParseFullTypeString((string)obj["$type"]!);
        return type == null ? null : obj["$data"]!.DefaultToObject(type, serializer);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.IsSubclassOf(typeof(EntityComponent)) || objectType == typeof(EntityComponent);
    }
}

[DataContract]
public class AquaticComponent : EntityComponent
{
}

[DataContract]
public class BoardAbilityComponent : EntityComponent
{
}

[DataContract]
public class CardComponent : EntityComponent
{
    [DataMember] public ComponentPrimitive<int> Guid { get; set; } = null!;

    public override FullObservableCollection<ComponentProperty> Children =>
        new(new[] { new ComponentProperty(nameof(Guid), Guid) });
}

[DataContract]
public class GrantTriggeredAbilityEffectDescriptor : EntityComponent
{
}

[DataContract]
public class HealthComponent : EntityComponent
{
    [DataMember] public BaseValueWrapper<int> MaxHealth;
    [DataMember] public ComponentPrimitive<int> CurrentDamage { get; set; } = null!;

    public override FullObservableCollection<ComponentProperty> Children =>
        new(new[]
        {
            new ComponentProperty(nameof(MaxHealth), MaxHealth.BaseValue),
            new ComponentProperty(nameof(CurrentDamage), CurrentDamage)
        });
}

public struct BaseValueWrapper<T>
{
    public ComponentPrimitive<T> BaseValue;
}