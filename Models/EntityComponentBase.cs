using System;
using System.Text.RegularExpressions;
using ReactiveUI;

namespace PvZHCardEditor.Models;

public abstract class EntityComponentBase : ReactiveObject
{
    public virtual FullObservableCollection<ComponentProperty> Children { get; } = new();
    public virtual ComponentValue? EditHandler => null;

    public string GetDisplayTypeString()
    {
        return GetDisplayTypeString(GetType());
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

    public static string GetDisplayTypeString(Type type)
    {
        return type.Name.EndsWith("Component")
            ? type.Name[..type.Name.LastIndexOf("Component", StringComparison.Ordinal)]
            : type.Name;
    }

    public static Type? ParseDisplayTypeString(string s, bool tryComponent)
    {
        var ns = typeof(EntityComponentBase).Namespace;
        var type = Type.GetType($"{ns}.{s}");
        if (tryComponent)
            type ??= Type.GetType($"{ns}.{s}Component");
        return type;
    }

    public static Type? ParseFullTypeString(string s)
    {
        var match = Regex.Match(s, "^PvZCards\\.Engine\\.([a-zA-Z0-9_]+)\\.([a-zA-Z0-9_]+),");
        var ns = match.Groups[1].Value;
        var name = match.Groups[2].Value;
        return ParseDisplayTypeString(name, ns == "Components");
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
}