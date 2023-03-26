using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PvZHCardEditor.ViewModels;
using ReactiveUI;

namespace PvZHCardEditor.Models;

[JsonConverter(typeof(EntityComponentBaseConverter))]
public abstract class EntityComponentBase : ComponentValue
{
    public override string Text => GetDisplayTypeString();
    public override FullObservableCollection<ComponentProperty> Children { get; } = new();
    protected virtual ComponentValue? EditHandler => null;

    protected abstract bool IsQuery { get; }

    public override async Task Edit(MainWindowViewModel model)
    {
        if (EditHandler != null)
        {
            await EditHandler.Edit(model);
            return;
        }

        Console.WriteLine($"Implement GUI for selecting component type. IsQuery = {IsQuery}");
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
        return $"PvZCards.Engine.Components.{s}, EngineLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
    }

    public static string ParseHasComponentTypeString(string s)
    {
        var match = Regex.Match(s, "^PvZCards\\.Engine\\.Components\\.([a-zA-Z0-9_]+),");
        return match.Groups[1].Value;
    }

    protected FullObservableCollection<ComponentProperty> CreateProperties(
        params (string PropertyName, ComponentValue Value)[] properties)
    {
        return new FullObservableCollection<ComponentProperty>(properties.Select(t =>
            CreateProperty(t.PropertyName, t.Value)));
    }

    private ComponentProperty CreateProperty(string propertyName, ComponentValue value)
    {
        var property = new ComponentProperty(propertyName, value);
        property.PropertyChanged += (_, _) => this.RaisePropertyChanged(propertyName);
        return property;
    }
}

public class EntityComponentBaseConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        var token = serializer.DefaultFromObject(value);
        var component = (EntityComponentBase)value;
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
        var type = EntityComponentBase.ParseFullTypeString((string)obj["$type"]!);
        return type == null ? null : obj["$data"]!.DefaultToObject(type, serializer);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.IsSubclassOf(typeof(EntityComponentBase)) || objectType == typeof(EntityComponentBase);
    }
}