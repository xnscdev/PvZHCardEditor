using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ReactiveUI;

namespace PvZHCardEditor.Models;

public class CardData : ReactiveObject
{
    private readonly FullObservableCollection<ComponentWrapper<EntityComponent>> _components;
    private readonly JObject _data;
    private CardClass[] _classes;
    private int _cost;
    private string _displayName;
    private CardFaction _faction;
    private string _flavorText;
    private int? _health;
    private string? _heraldFighterText;
    private string? _heraldTrickText;
    private string _id;
    private string _longText;
    private CardRarity _rarity;
    private CardSet _set;
    private string _shortText;
    private int? _strength;
    private string? _targetingText;
    private CardTribe[] _tribes;
    private CardType _type;

    public CardData(string id, JToken data)
    {
        _id = id;
        _data = (JObject)data;
        _type = ParseCardType(_data);
        _faction = Enum.Parse<CardFaction>((string)_data["faction"]!);
        _cost = (int)_data["displaySunCost"]!;
        _strength = _type == CardType.Fighter ? (int?)_data["displayAttack"] : null;
        _health = _type == CardType.Fighter ? (int?)_data["displayHealth"] : null;

        PrefabName = (string)_data["prefabName"]!;
        _displayName = GameDataManager.GetLocalizedString($"{PrefabName}_name");
        _shortText = GameDataManager.GetLocalizedString($"{PrefabName}_shortDesc");
        _longText = GameDataManager.GetLocalizedString($"{PrefabName}_longDesc");
        _flavorText = GameDataManager.GetLocalizedString($"{PrefabName}_flavorText");
        _targetingText = GameDataManager.TryGetLocalizedString($"{PrefabName}_Targeting");
        _heraldFighterText = GameDataManager.TryGetLocalizedString($"{PrefabName}_heraldFighter");
        _heraldTrickText = GameDataManager.TryGetLocalizedString($"{PrefabName}_heraldTrick");
        _rarity = (CardRarity)(int)_data["rarity"]!;
        _set = Enum.GetValues<CardSet>().Where(set => set.GetCardSetKey() == (string?)_data["set"])
            .DefaultIfEmpty(CardSet.Empty).First();
        _tribes = ((JArray)_data["subtypes"]!).Select(t => GameDataManager.GetEnumInternalKey<CardTribe>((string)t!))
            .ToArray();

        var classes = (string)_data["color"]!;
        _classes = classes == "0"
            ? Array.Empty<CardClass>()
            : classes.Split(new[] { ", " }, StringSplitOptions.TrimEntries)
                .Select(GameDataManager.GetEnumInternalKey<CardClass>).ToArray();

        _components = new FullObservableCollection<ComponentWrapper<EntityComponent>>(
            _data["entity"]!["components"]!.Select(t => t.ToObject<ComponentWrapper<EntityComponent>>()!));
    }

    public string PrefabName { get; }

    public string DisplayName
    {
        get => _displayName;
        set => this.RaiseAndSetIfChanged(ref _displayName, value);
    }

    public string ShortText
    {
        get => _shortText;
        set => this.RaiseAndSetIfChanged(ref _shortText, value);
    }

    public string LongText
    {
        get => _longText;
        set => this.RaiseAndSetIfChanged(ref _longText, value);
    }

    public string FlavorText
    {
        get => _flavorText;
        set => this.RaiseAndSetIfChanged(ref _flavorText, value);
    }

    public string? TargetingText
    {
        get => _targetingText;
        set => this.RaiseAndSetIfChanged(ref _targetingText, value);
    }

    public string? HeraldFighterText
    {
        get => _heraldFighterText;
        set => this.RaiseAndSetIfChanged(ref _heraldFighterText, value);
    }

    public string? HeraldTrickText
    {
        get => _heraldTrickText;
        set => this.RaiseAndSetIfChanged(ref _heraldTrickText, value);
    }

    public string Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    public int Cost
    {
        get => _cost;
        set => this.RaiseAndSetIfChanged(ref _cost, value);
    }

    public int? Strength
    {
        get => _strength;
        set => this.RaiseAndSetIfChanged(ref _strength, value);
    }

    public int? Health
    {
        get => _health;
        set => this.RaiseAndSetIfChanged(ref _health, value);
    }

    public CardType Type
    {
        get => _type;
        set => this.RaiseAndSetIfChanged(ref _type, value);
    }

    public CardFaction Faction
    {
        get => _faction;
        set => this.RaiseAndSetIfChanged(ref _faction, value);
    }

    public CardRarity Rarity
    {
        get => _rarity;
        set => this.RaiseAndSetIfChanged(ref _rarity, value);
    }

    public CardSet Set
    {
        get => _set;
        set => this.RaiseAndSetIfChanged(ref _set, value);
    }

    public CardTribe[] Tribes
    {
        get => _tribes;
        set => this.RaiseAndSetIfChanged(ref _tribes, value);
    }

    public CardClass[] Classes
    {
        get => _classes;
        set => this.RaiseAndSetIfChanged(ref _classes, value);
    }

    public string FindResultText => $"{Id}: {DisplayName}";

    public IEnumerable<string> CardInfoData
    {
        get
        {
            yield return $"DisplayName = {DisplayName}";
            yield return $"ShortText = {ShortText}";
            yield return $"LongText = {LongText}";
            yield return $"FlavorText = {FlavorText}";
            if (TargetingText != null)
                yield return $"TargetingText = {TargetingText}";
            if (HeraldFighterText != null)
                yield return $"HeraldFighterText = {HeraldFighterText}";
            if (HeraldTrickText != null)
                yield return $"HeraldTrickText = {HeraldTrickText}";
            yield return $"PrefabName = {PrefabName}";
            yield return $"Id = {Id}";
            yield return $"Cost = {Cost}";
            if (Strength != null)
                yield return $"Strength = {Strength}";
            if (Health != null)
                yield return $"Health = {Health}";
            yield return $"Rarity = {Rarity}";
            yield return $"Set = {Set}";
            yield return $"Tribes = {string.Join(", ", Tribes)}";
            yield return $"Classes = {string.Join(", ", Classes)}";
        }
    }

    public IEnumerable<ComponentWrapper<EntityComponent>> ComponentsData => _components;

    public void Save()
    {
        var components = new JArray(_components.Select(JToken.FromObject).Cast<object>().ToArray());
        _data["entity"]!["components"] = components;
    }

    public static CardType ParseCardType(JToken data)
    {
        if ((bool?)data["isFighter"] == true)
            return CardType.Fighter;
        if ((bool?)data["isEnv"] == true)
            return CardType.Environment;

        return data["entity"]!["components"]!
            .Select(token => EntityComponentBase.ParseFullTypeString((string)token["$type"]!))
            .Any(type => type == typeof(BoardAbilityComponent))
            ? CardType.BoardAbility
            : CardType.Trick;
    }
}

public enum CardType
{
    Fighter,
    Trick,
    Environment,
    BoardAbility
}

public enum CardFaction
{
    Plants,
    Zombies,
    All
}

public enum CardTribe
{
    [InternalKey("Peashooter")] [FactionOnly(CardFaction.Plants)]
    Pea = 0,
    [FactionOnly(CardFaction.Plants)] Berry,
    [FactionOnly(CardFaction.Plants)] Bean,
    [FactionOnly(CardFaction.Plants)] Flower,
    [FactionOnly(CardFaction.Plants)] Mushroom,
    [FactionOnly(CardFaction.Plants)] Nut,
    [FactionOnly(CardFaction.Zombies)] Sports,
    [FactionOnly(CardFaction.Zombies)] Science,
    [FactionOnly(CardFaction.Zombies)] Dancing,
    [FactionOnly(CardFaction.Zombies)] Imp,
    [FactionOnly(CardFaction.Zombies)] Pet,
    [FactionOnly(CardFaction.Zombies)] Gargantuar,
    [FactionOnly(CardFaction.Zombies)] Pirate,
    [FactionOnly(CardFaction.Plants)] Pinecone,
    [FactionOnly(CardFaction.All)] Knight,
    [FactionOnly(CardFaction.Zombies)] Mustache,
    [FactionOnly(CardFaction.Zombies)] Party,
    [FactionOnly(CardFaction.All)] Ghost,
    [FactionOnly(CardFaction.Zombies)] Gourmet,
    [FactionOnly(CardFaction.Zombies)] History,
    [FactionOnly(CardFaction.Zombies)] Barrel,
    [FactionOnly(CardFaction.Plants)] Seed,
    [FactionOnly(CardFaction.Plants)] Animal,
    [FactionOnly(CardFaction.Plants)] Cactus,
    [FactionOnly(CardFaction.Plants)] Corn,
    [FactionOnly(CardFaction.Plants)] Dragon,
    [FactionOnly(CardFaction.Plants)] Flytrap,
    [FactionOnly(CardFaction.Plants)] Fruit,
    [FactionOnly(CardFaction.Plants)] Leafy,
    [FactionOnly(CardFaction.Plants)] Moss,
    [FactionOnly(CardFaction.All)] Pepper,
    [FactionOnly(CardFaction.Plants)] Root,
    [FactionOnly(CardFaction.Plants)] Squash,
    [FactionOnly(CardFaction.Plants)] Tree,
    [FactionOnly(CardFaction.All)] Vine,
    [FactionOnly(CardFaction.Zombies)] Clock,
    [FactionOnly(CardFaction.All)] Garbage,
    [FactionOnly(CardFaction.Zombies)] Professional,
    [FactionOnly(CardFaction.All)] Onion,
    [FactionOnly(CardFaction.Zombies)] Monster,
    [FactionOnly(CardFaction.Plants)] Banana,
    [FactionOnly(CardFaction.All)] Mime
}

public enum CardClass
{
    [InternalKey("MegaGro")] [FactionOnly(CardFaction.Plants)]
    MegaGrow,
    [FactionOnly(CardFaction.Plants)] Smarty,
    [FactionOnly(CardFaction.Plants)] Kabloom,
    [FactionOnly(CardFaction.Plants)] Guardian,
    [FactionOnly(CardFaction.Plants)] Solar,
    [FactionOnly(CardFaction.Zombies)] Brainy,
    [FactionOnly(CardFaction.Zombies)] Hearty,

    [InternalKey("Hungry")] [FactionOnly(CardFaction.Zombies)]
    Beastly,

    [InternalKey("Madcap")] [FactionOnly(CardFaction.Zombies)]
    Crazy,
    [FactionOnly(CardFaction.Zombies)] Sneaky
}

public enum CardRarity
{
    [InternalKey("R1")] Uncommon,
    [InternalKey("R2")] Rare,
    [InternalKey("R3")] SuperRare,
    [InternalKey("R4")] Legendary,
    [InternalKey("R0")] Common,
    Event
}

public enum CardSet
{
    [CardSetData("Gold", "Bloom")] Premium,
    [CardSetData("Set2", "Galaxy")] Galactic,
    [CardSetData("Set3", "Colossal")] Colossal,
    [CardSetData("Set4", "Triassic")] Triassic,
    [CardSetData("Silver", "Dawn")] Basic,

    [CardSetData("Superpower", "Superpower")]
    Superpower,
    [CardSetData("Hero", "Bloom")] SignatureSuperpower,
    Token,
    [CardSetData("Board", null)] Board,
    [CardSetData("Cheats", null)] Cheats,
    [CardSetData("Blank", null)] Blank,
    Empty
}

public enum CardSpecialAbility
{
    [InternalKey("Ambush")] AntiHero = 9,
    [InternalKey("Repeater")] DoubleStrike = 11,
    Overshoot,
    Unique
}