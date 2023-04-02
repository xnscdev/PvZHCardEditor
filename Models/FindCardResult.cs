using System.Collections.Generic;

namespace PvZHCardEditor.Models;

public record struct FindCardResult
{
    public int Cost;
    public string DisplayName;
    public CardFaction Faction;
    public int? Health;
    public string Id;
    public string PrefabName;
    public int? Strength;
    public CardType Type;

    public string Title => $"{Id}: {DisplayName}";

    public IEnumerable<FindCardResultLine> Body
    {
        get
        {
            yield return new FindCardResultLine($"PrefabName = {PrefabName}");
            yield return new FindCardResultLine($"Type = {Type}");
            yield return new FindCardResultLine($"Faction = {Faction}");
            yield return new FindCardResultLine($"Cost = {Cost}");
            if (Strength != null)
                yield return new FindCardResultLine($"Strength = {Strength}");
            if (Health != null)
                yield return new FindCardResultLine($"Health = {Health}");
        }
    }
}

public class FindCardResultLine
{
    public FindCardResultLine(string text)
    {
        Text = text;
    }

    public string Text { get; }
}