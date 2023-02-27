using Newtonsoft.Json.Linq;
using PvZHCardEditor.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PvZHCardEditor
{
    public class CardData : ViewModelBase
    {
        private readonly JObject _data;
        private readonly ComponentCollection<ComponentNode> _components;
        private string _displayName;
        private string _shortText;
        private string _longText;
        private string _flavorText;
        private string? _targetingText;
        private string? _heraldFighterText;
        private string? _heraldTrickText;
        private string _id;
        private int _cost;
        private int? _strength;
        private int? _health;
        private CardType _type;
        private CardFaction _faction;
        private CardRarity _rarity;
        private CardSet _set;
        private CardTribe[] _tribes;
        private readonly TreeViewCompoundNode _tribesNode;
        private CardClass[] _classes;

        public string PrefabName { get; }

        public string DisplayName
        {
            get => _displayName;
            set
            {
                GameDataManager.SetTranslatedString($"{PrefabName}_name", value);
                SetProperty(ref _displayName, value, null);
            }
        }

        public string ShortText
        {
            get => _shortText;
            set
            {
                GameDataManager.SetTranslatedString($"{PrefabName}_shortDesc", value);
                SetProperty(ref _shortText, value, null);
            }
        }

        public string LongText
        {
            get => _longText;
            set
            {
                GameDataManager.SetTranslatedString($"{PrefabName}_longDesc", value);
                SetProperty(ref _longText, value, null);
            }
        }

        public string FlavorText
        {
            get => _flavorText;
            set
            {
                GameDataManager.SetTranslatedString($"{PrefabName}_flavorText", value);
                SetProperty(ref _flavorText, value, null);
            }
        }

        public string? TargetingText
        {
            get => _targetingText;
            set
            {
                GameDataManager.SetTranslatedString($"{PrefabName}_Targeting", value);
                SetProperty(ref _targetingText, value, null);
            }
        }

        public string? HeraldFighterText
        {
            get => _heraldFighterText;
            set
            {
                GameDataManager.SetTranslatedString($"{PrefabName}_heraldFighter", value);
                SetProperty(ref _heraldFighterText, value, null);
            }
        }

        public string? HeraldTrickText
        {
            get => _heraldTrickText;
            set
            {
                GameDataManager.SetTranslatedString($"{PrefabName}_heraldTrick", value);
                SetProperty(ref _heraldTrickText, value, null);
            }
        }

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value, null);
        }

        public int Cost
        {
            get => _cost;
            set
            {
                _data["displaySunCost"] = value;
                FindOrInsertComponent(typeof(SunCost)).Edit(new ComponentInt(new JValue(value)));
                SetProperty(ref _cost, value, null);
            }
        }

        public int? Strength
        {
            get => _strength;
            set
            {
                if (value is not null)
                {
                    _data["displayAttack"] = value;
                    FindOrInsertComponent(typeof(Attack)).Edit(new ComponentInt(new JValue(value)));
                }
                
                SetProperty(ref _strength, value, null);
            }
        }

        public int? Health
        {
            get => _health;
            set
            {
                if (value is not null)
                {
                    _data["displayHealth"] = value;
                    FindOrInsertComponent(typeof(Health)).Children.Where(c => c.Key == "MaxHealth").First().Edit(new ComponentInt(new JValue(value)));
                }

                SetProperty(ref _health, value, null);
            }
        }

        public CardType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public CardFaction Faction
        {
            get => _faction;
            set => SetProperty(ref _faction, value);
        }

        public CardRarity Rarity
        {
            get => _rarity;
            set
            {
                _data["rarity"] = (int)value;
                var rarityKey = Set.GetAttribute<CardSetDataAttribute>()?.SetRarityKey;
                _data["setAndRarityKey"] = Set == CardSet.Token ? "Token" : rarityKey is null ? null : $"{rarityKey}_{value}";
                FindOrInsertComponent(typeof(Rarity)).Edit(new ComponentString(new JValue(value.GetInternalKey())));
                SetProperty(ref _rarity, value, null);
            }
        }

        public CardSet Set
        {
            get => _set;
            set
            {
                var attr = value.GetAttribute<CardSetDataAttribute>();
                _data["set"] = attr?.SetKey ?? null;
                _data["setAndRarityKey"] = value == CardSet.Token ? "Token" : attr?.SetRarityKey is null ? null : $"{attr.SetRarityKey}_{Rarity}";
                SetProperty(ref _set, value, null);
            }
        }

        public CardTribe[] Tribes
        {
            get => _tribes;
            set
            {
                _tribesNode.Children = value.Select(x => new TreeViewNode(x.ToString()));
                _data["subtypes"] = new JArray(value.Select(x => x.GetInternalKey()).ToArray());
                var array = new ComponentArray(new JArray());
                foreach (var x in value)
                    array.Add(null, new ComponentInt(new JValue((int)x)));
                FindOrInsertComponent(typeof(Subtypes)).Edit(array);
                SetProperty(ref _tribes, value, null);
            }
        }

        public CardClass[] Classes
        {
            get => _classes;
            set
            {
                _data["color"] = value.Any() ? string.Join(", ", value.Select(x => x.GetInternalKey())) : "0";
                SetProperty(ref _classes, value, null);
            }
        }

        public string ResultViewTitle => $"{Id}: {DisplayName}";

        public IEnumerable<TreeViewNode> ResultViewData
        {
            get
            {
                yield return new TreeViewNode($"DisplayName = {DisplayName}");
                yield return new TreeViewNode($"PrefabName = {PrefabName}");
                yield return new TreeViewNode($"Id = {Id}");
                yield return new TreeViewNode($"Cost = {Cost}");
                if (Strength is not null)
                    yield return new TreeViewNode($"Strength = {Strength}");
                if (Health is not null)
                    yield return new TreeViewNode($"Health = {Health}");
            }
        }

        public IEnumerable<TreeViewNode> CardInfoViewData
        {
            get
            {
                yield return new TreeViewNode($"DisplayName = {DisplayName}");
                yield return new TreeViewNode($"ShortText = {ShortText}");
                yield return new TreeViewNode($"LongText = {LongText}");
                yield return new TreeViewNode($"FlavorText = {FlavorText}");
                if (TargetingText is not null)
                    yield return new TreeViewNode($"TargetingText = {TargetingText}");
                if (HeraldFighterText is not null)
                    yield return new TreeViewNode($"HeraldFighterText = {HeraldFighterText}");
                if (HeraldTrickText is not null)
                    yield return new TreeViewNode($"HeraldTrickText = {HeraldTrickText}");
                yield return new TreeViewNode($"PrefabName = {PrefabName}");
                yield return new TreeViewNode($"Id = {Id}");
                yield return new TreeViewNode($"Cost = {Cost}");
                if (Strength is not null)
                    yield return new TreeViewNode($"Strength = {Strength}");
                if (Health is not null)
                    yield return new TreeViewNode($"Health = {Health}");
                yield return new TreeViewNode($"Rarity = {Rarity}");
                yield return new TreeViewNode($"Set = {Set}");
                yield return _tribesNode;
                yield return new TreeViewNode($"Classes = {string.Join(", ", Classes)}");
            }
        }

        public IEnumerable<ComponentNode> ComponentsViewData => _components;

        public CardData(string id, JToken data)
        {
            _id = id;
            _data = (JObject)data;
            _type = ParseType(_data);
            _faction = Enum.Parse<CardFaction>((string)_data["faction"]!);
            _cost = (int)_data["displaySunCost"]!;
            _strength = _type == CardType.Fighter ? (int?)_data["displayAttack"] : null;
            _health = _type == CardType.Fighter ? (int?)_data["displayHealth"] : null;

            PrefabName = (string)_data["prefabName"]!;
            _displayName = GameDataManager.GetTranslatedString($"{PrefabName}_name");
            _shortText = GameDataManager.GetTranslatedString($"{PrefabName}_shortDesc");
            _longText = GameDataManager.GetTranslatedString($"{PrefabName}_longDesc");
            _flavorText = GameDataManager.GetTranslatedString($"{PrefabName}_flavorText");
            _targetingText = GameDataManager.TryGetTranslatedString($"{PrefabName}_Targeting");
            _heraldFighterText = GameDataManager.TryGetTranslatedString($"{PrefabName}_heraldFighter");
            _heraldTrickText = GameDataManager.TryGetTranslatedString($"{PrefabName}_heraldTrick");
            _rarity = (CardRarity)(int)_data["rarity"]!;
            _set = Enum.GetValues<CardSet>().Where(set => set.GetCardSetKey() == (string?)_data["set"]).DefaultIfEmpty(CardSet.Empty).First();

            _tribes = ((JArray)_data["subtypes"]!).Select(t => GameDataManager.GetEnumInternalKey<CardTribe>((string)t!)).ToArray();
            _tribesNode = new TreeViewCompoundNode("Tribes", _tribes.Select(t => new TreeViewNode(t.ToString())));

            var classes = (string)_data["color"]!;
            _classes = classes == "0" ? Array.Empty<CardClass>() : classes.Split(new string[] { ", " }, StringSplitOptions.TrimEntries)
                .Select(c => GameDataManager.GetEnumInternalKey<CardClass>(c)).ToArray();

            _components = new ComponentCollection<ComponentNode>();
            foreach (var token in _data["entity"]!["components"]!)
            {
                var component = ComponentNode.ParseComponentNode(token);
                if (component is not null)
                    _components.Add(component);
            }
        }

        public void SetupNewCard(int id, CardFaction faction, CardType type)
        {
            FindOrInsertComponent(typeof(Card)).Edit(new ComponentInt(new JValue(id)));
            if (type == CardType.Fighter)
            {
                FindOrInsertComponent(typeof(Attack));
                FindOrInsertComponent(typeof(Health));
            }
            else if (type == CardType.BoardAbility)
            {
                FindOrInsertComponent(typeof(BoardAbility));
            }

            FindOrInsertComponent(typeof(SunCost));
            if (faction == CardFaction.Plants)
                FindOrInsertComponent(typeof(Plants));
            else if (faction == CardFaction.Zombies)
                FindOrInsertComponent(typeof(Zombies));

            Rarity = CardRarity.Common;
        }

        public void ActionPerformed()
        {
        }

        public int RemoveComponent(ComponentNode component)
        {
            var index = _components.IndexOf(component);
            _components.RemoveAt(index);
            var token = component.RootToken ?? component.Token;
            if (token.Parent is JProperty)
                token.Parent.Remove();
            else
                token.Remove();
            return index;
        }

        public void AddComponent(ComponentNode component, int index = -1)
        {
            if (index < 0)
                index = _components.Count;
            _components.Insert(index, component);
            var array = (JArray)_data["entity"]!["components"]!;
            array.Insert(index, component.RootToken!);
        }

        public ComponentNode? FindComponent(Type type)
        {
            var array = (JArray)_data["entity"]!["components"]!;
            var token = array.Where(c => ComponentNode.ParseComponentType((string)c["$type"]!) == type).FirstOrDefault();
            if (token is null)
                return null;
            return _components.Where(c => ReferenceEquals(c.RootToken, token)).FirstOrDefault();
        }

        public ComponentNode FindOrInsertComponent(Type type)
        {
            var array = (JArray)_data["entity"]!["components"]!;
            var query = from c in array where ComponentNode.ParseComponentType((string)c["$type"]!) == type select c;
            if (query.Any())
            {
                var token = query.First();
                return _components.Where(c => ReferenceEquals(c.RootToken, token)).First();
            }
            else
            {
                var component = (CardComponent?)Activator.CreateInstance(type);
                var name = type.Name;
                if (component is null)
                    throw new ArgumentException(name);
                var node = component.Value is null ? new AutoComponentNode(name, component.Token, component.AllowAdd, component.FullToken) : new AutoComponentNode(name, component.Value, component.AllowAdd, component.FullToken);
                AddComponent(node);
                return node;
            }
        }

        public void FindRemoveComponent(Type type)
        {
            var array = (JArray)_data["entity"]!["components"]!;
            var query = from c in array where ComponentNode.ParseComponentType((string)c["$type"]!) == type select c;
            if (query.Any())
            {
                var token = query.First();
                var node = _components.Where(c => ReferenceEquals(c.RootToken, token)).First();
                RemoveComponent(node);
            }
        }

        public CardExtraAttributes GetExtraAttributes()
        {
            var allowCrafting = _data.ContainsKey("craftingBuy");
            return new CardExtraAttributes()
            {
                Set = Set,
                Rarity = Rarity,
                IsPower = (bool)_data["isPower"]!,
                IsPrimaryPower = (bool)_data["isPrimaryPower"]!,
                IsFighter = (bool)_data["isFighter"]!,
                IsEnv = (bool)_data["isEnv"]!,
                IsAquatic = (bool)_data["isAquatic"]!,
                IsTeamup = (bool)_data["isTeamup"]!,
                IgnoreDeckLimit = (bool)_data["ignoreDeckLimit"]!,
                Usable = (bool)_data["usable"]!,
                BuyPrice = allowCrafting ? (int)_data["craftingBuy"]! : null,
                SellPrice = allowCrafting ? (int)_data["craftingSell"]! : null,
                Abilities = _data["special_abilities"]!.Select(a => Enum.GetValues<CardSpecialAbility>().Where(x => x.GetInternalKey() == (string)a!).Select(x => (CardSpecialAbility?)x).DefaultIfEmpty(null).First()).Where(a => a is not null).Select(a => a!.Value).ToArray(),
                Tags = _data["tags"]!.Select(t => (string)t!).ToArray()
            };
        }

        public void SetExtraAttributes(CardExtraAttributes data)
        {
            Set = data.Set;
            Rarity = data.Rarity;
            _data["isPower"] = data.IsPower;
            _data["isPrimaryPower"] = data.IsPrimaryPower;
            _data["isFighter"] = data.IsFighter;
            _data["isEnv"] = data.IsEnv;
            _data["isAquatic"] = data.IsAquatic;
            _data["isTeamup"] = data.IsTeamup;
            _data["ignoreDeckLimit"] = data.IgnoreDeckLimit;
            _data["usable"] = data.Usable;
            
            if (data.BuyPrice is not null)
            {
                _data["craftingBuy"] = data.BuyPrice.Value;
                _data["craftingSell"] = data.SellPrice!.Value;
            }
            else
            {
                _data.Remove("craftingBuy");
                _data.Remove("craftingSell");
            }

            var abilities = new JArray(data.Abilities.Select(x => (int)x).ToArray());
            _data["special_abilities"] = new JArray(data.Abilities.Select(x => x.GetInternalKey()).ToArray());
            if (data.Abilities.Length > 0)
                FindOrInsertComponent(typeof(ShowTriggeredIcon)).Edit(new ComponentArray(abilities, abilities.Select(a => new ComponentInt((int)a!))));
            else
                FindRemoveComponent(typeof(ShowTriggeredIcon));

            var tags = new JArray(data.Tags);
            _data["tags"] = tags.DeepClone();
            FindOrInsertComponent(typeof(Tags)).Edit(new ComponentArray(tags, tags.Select(tag => new ComponentString((string)tag!))));
        }

        public static CardType ParseType(JToken data)
        {
            if ((bool?)data["isFighter"] is true)
                return CardType.Fighter;
            if ((bool?)data["isEnv"] is true)
                return CardType.Environment;
            
            foreach (var token in data["entity"]!["components"]!)
            {
                var type = ComponentNode.ParseComponentType((string)token["$type"]!);
                if (type == typeof(BoardAbility))
                    return CardType.BoardAbility;
            }

            return CardType.Trick;
        }

        public static JObject CreateCardToken(string prefabName, CardFaction faction, CardType type)
        {
            return new JObject
            {
                ["entity"] = new JObject
                {
                    ["components"] = new JArray()
                },
                ["prefabName"] = prefabName,
                ["baseId"] = type switch
                {
                    CardType.Fighter => faction == CardFaction.Zombies ? "BaseZombie" : "Base",
                    CardType.Environment => faction == CardFaction.Zombies ? "BaseZombieEnvironment" : "BasePlantEnvironment",
                    _ => faction == CardFaction.Zombies ? "BaseZombieOneTimeEffect" : "BasePlantOneTimeEffect"
                },
                ["color"] = "0",
                ["set"] = "Silver",
                ["rarity"] = 4,
                ["setAndRarityKey"] = "Dawn_Common",
                ["displayHealth"] = 0,
                ["displayAttack"] = 0,
                ["displaySunCost"] = 0,
                ["faction"] = faction.ToString(),
                ["ignoreDeckLimit"] = false,
                ["isPower"] = false,
                ["isPrimaryPower"] = false,
                ["isFighter"] = type == CardType.Fighter,
                ["isEnv"] = type == CardType.Environment,
                ["isAquatic"] = false,
                ["isTeamup"] = false,
                ["subtypes"] = new JArray(),
                ["tags"] = new JArray(),
                ["subtype_affinities"] = new JArray(),
                ["subtype_affinity_weights"] = new JArray(),
                ["tag_affinities"] = new JArray(),
                ["tag_affinity_weights"] = new JArray(),
                ["card_affinities"] = new JArray(),
                ["card_affinity_weights"] = new JArray(),
                ["usable"] = true,
                ["special_abilities"] = new JArray()
            };
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
        [InternalKey("Peashooter"), FactionOnly(CardFaction.Plants)]
        Pea = 0,
        [FactionOnly(CardFaction.Plants)]
        Berry,
        [FactionOnly(CardFaction.Plants)]
        Bean,
        [FactionOnly(CardFaction.Plants)]
        Flower,
        [FactionOnly(CardFaction.Plants)]
        Mushroom,
        [FactionOnly(CardFaction.Plants)]
        Nut,
        [FactionOnly(CardFaction.Zombies)]
        Sports,
        [FactionOnly(CardFaction.Zombies)]
        Science,
        [FactionOnly(CardFaction.Zombies)]
        Dancing,
        [FactionOnly(CardFaction.Zombies)]
        Imp,
        [FactionOnly(CardFaction.Zombies)]
        Pet,
        [FactionOnly(CardFaction.Zombies)]
        Gargantuar,
        [FactionOnly(CardFaction.Zombies)]
        Pirate,
        [FactionOnly(CardFaction.Plants)]
        Pinecone,
        [FactionOnly(CardFaction.Zombies)]
        Mustache = 15,
        [FactionOnly(CardFaction.Zombies)]
        Party,
        [FactionOnly(CardFaction.Zombies)]
        Gourmet = 18,
        [FactionOnly(CardFaction.Zombies)]
        History,
        [FactionOnly(CardFaction.Zombies)]
        Barrel,
        [FactionOnly(CardFaction.Plants)]
        Seed,
        [FactionOnly(CardFaction.Plants)]
        Animal,
        [FactionOnly(CardFaction.Plants)]
        Cactus,
        [FactionOnly(CardFaction.Plants)]
        Corn,
        [FactionOnly(CardFaction.Plants)]
        Dragon,
        [FactionOnly(CardFaction.Plants)]
        Flytrap,
        [FactionOnly(CardFaction.Plants)]
        Fruit,
        [FactionOnly(CardFaction.Plants)]
        Leafy,
        [FactionOnly(CardFaction.Plants)]
        Moss,
        [FactionOnly(CardFaction.Plants)]
        Root = 31,
        [FactionOnly(CardFaction.Plants)]
        Squash,
        [FactionOnly(CardFaction.Plants)]
        Tree,
        [FactionOnly(CardFaction.Zombies)]
        Clock = 35,
        [FactionOnly(CardFaction.Zombies)]
        Professional = 37,
        [FactionOnly(CardFaction.Zombies)]
        Monster = 39,
        [FactionOnly(CardFaction.Plants)]
        Banana,
        [FactionOnly(CardFaction.All)]
        Mime
    }

    public enum CardClass
    {
        [InternalKey("MegaGro"), FactionOnly(CardFaction.Plants)]
        MegaGrow,
        [FactionOnly(CardFaction.Plants)]
        Smarty,
        [FactionOnly(CardFaction.Plants)]
        Kabloom,
        [FactionOnly(CardFaction.Plants)]
        Guardian,
        [FactionOnly(CardFaction.Plants)]
        Solar,
        [FactionOnly(CardFaction.Zombies)]
        Brainy,
        [FactionOnly(CardFaction.Zombies)]
        Hearty,
        [InternalKey("Hungry"), FactionOnly(CardFaction.Zombies)]
        Beastly,
        [InternalKey("Madcap"), FactionOnly(CardFaction.Zombies)]
        Crazy,
        [FactionOnly(CardFaction.Zombies)]
        Sneaky
    }

    public enum CardRarity
    {
        [InternalKey("R1")]
        Uncommon,
        [InternalKey("R2")]
        Rare,
        [InternalKey("R3")]
        SuperRare,
        [InternalKey("R4")]
        Legendary,
        [InternalKey("R0")]
        Common,
        Event
    }

    public enum CardSet
    {
        [CardSetData("Gold", "Bloom")]
        Premium,
        [CardSetData("Set2", "Galaxy")]
        Galactic,
        [CardSetData("Set3", "Colossal")]
        Colossal,
        [CardSetData("Set4", "Triassic")]
        Triassic,
        [CardSetData("Silver", "Dawn")]
        Basic,
        [CardSetData("Superpower", "Superpower")]
        Superpower,
        [CardSetData("Hero", "Bloom")]
        SignatureSuperpower,
        Token,
        [CardSetData("Board", null)]
        Board,
        [CardSetData("Cheats", null)]
        Cheats,
        [CardSetData("Blank", null)]
        Blank,
        Empty
    }

    public enum CardSpecialAbility
    {
        [InternalKey("Ambush")]
        AntiHero = 9,
        [InternalKey("Repeater")]
        DoubleStrike = 11,
        Overshoot,
        Unique
    }
}
