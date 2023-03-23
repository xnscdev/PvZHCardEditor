using System;

namespace PvZHCardEditor.Models;

[AttributeUsage(AttributeTargets.Field)]
public class InternalKeyAttribute : Attribute
{
    public InternalKeyAttribute(string key)
    {
        Key = key;
    }

    public string Key { get; }
}

[AttributeUsage(AttributeTargets.Field)]
public class FactionOnlyAttribute : Attribute
{
    public FactionOnlyAttribute(CardFaction faction)
    {
        Faction = faction;
    }

    public CardFaction Faction { get; }
}

[AttributeUsage(AttributeTargets.Field)]
public class CardSetDataAttribute : Attribute
{
    public CardSetDataAttribute(string setKey, string? setRarityKey)
    {
        SetKey = setKey;
        SetRarityKey = setRarityKey;
    }

    public string SetKey { get; }
    public string? SetRarityKey { get; }
}