using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PvZHCardEditor.Models;

public abstract class CardComponent
{
    public virtual ObservableCollection<object> GetChildNodes() => new();

    public string GetFullTypeString()
    {
        var name = GetType().Name;
        string patchedName;
        if (name.EndsWith("Query"))
            patchedName = "Queries." + name;
        else if (name.EndsWith("Component"))
            patchedName = "Components." + name[..name.LastIndexOf("Component", StringComparison.Ordinal)];
        else
            patchedName = "Components." + name;
        return $"PvZCards.Engine.{patchedName}, EngineLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
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
        // TODO: Parse important part of obj["$type"], create instance and fill with data from obj["$data"]
        return null;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.IsSubclassOf(typeof(CardComponent)) || objectType == typeof(CardComponent);
    }
}