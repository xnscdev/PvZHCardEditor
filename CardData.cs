using Newtonsoft.Json.Linq;
using PvZHCardEditor.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PvZHCardEditor
{
    public class CardData : ViewModelBase
    {
        private readonly JToken _data;
        private readonly ComponentCollection<ComponentNode> _components;
        private string _prefabName;
        private string _displayName;
        private string _shortText;
        private string _longText;
        private string _flavorText;
        private string _id;
        private int _cost;
        private int? _strength;
        private int? _health;
        private CardType _type;
        private CardFaction _faction;
        private CardTribe[] _tribes;
        private readonly TreeViewCompoundNode _tribesNode;
        private CardClass[] _classes;

        public string PrefabName
        {
            get => _prefabName;
            set => SetProperty(ref _prefabName, value, null);
        }

        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value, null);
        }

        public string ShortText
        {
            get => _shortText;
            set => SetProperty(ref _shortText, value, null);
        }

        public string LongText
        {
            get => _longText;
            set => SetProperty(ref _longText, value, null);
        }

        public string FlavorText
        {
            get => _flavorText;
            set => SetProperty(ref _flavorText, value, null);
        }

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value, null);
        }

        public int Cost
        {
            get => _cost;
            set => SetProperty(ref _cost, value, null);
        }

        public int? Strength
        {
            get => _strength;
            set => SetProperty(ref _strength, value, null);
        }

        public int? Health
        {
            get => _health;
            set => SetProperty(ref _health, value, null);
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

        public CardTribe[] Tribes
        {
            get => _tribes;
            set
            {
                SetProperty(ref _tribes, value, null);
                _tribesNode.Children = _tribes.Select(t => new TreeViewNode(t.ToString()));
            }
        }

        public CardClass[] Classes
        {
            get => _classes;
            set => SetProperty(ref _classes, value, null);
        }

        public CardData(string id, JToken data)
        {
            _id = id;
            _data = data;
            _type = ParseType(_data);
            _faction = Enum.Parse<CardFaction>((string)_data["faction"]!);
            _cost = (int)_data["displaySunCost"]!;
            _strength = _type == CardType.Fighter ? (int?)_data["displayAttack"] : null;
            _health = _type == CardType.Fighter ? (int?)_data["displayHealth"] : null;
            _prefabName = (string)_data["prefabName"]!;
            _displayName = GameDataManager.GetTranslatedString($"{_prefabName}_name");
            _shortText = GameDataManager.GetTranslatedString($"{_prefabName}_shortDesc");
            _longText = GameDataManager.GetTranslatedString($"{_prefabName}_longDesc");
            _flavorText = GameDataManager.GetTranslatedString($"{_prefabName}_flavorText");
            
            _tribes = ((JArray)_data["subtypes"]!).Select(t => GameDataManager.GetEnumInternalKey<CardTribe>((string)t!)).ToArray();
            _tribesNode = new TreeViewCompoundNode("Tribes", _tribes.Select(t => new TreeViewNode(t.ToString())));
            
            var classes = (string)_data["color"]!;
            _classes = classes == "0" ? Array.Empty<CardClass>() : classes.Split(new string[] { ", " }, StringSplitOptions.TrimEntries)
                .Select(c => GameDataManager.GetEnumInternalKey<CardClass>(c)).ToArray();
            
            _components = new ComponentCollection<ComponentNode>();
            foreach (var token in _data["entity"]!["components"]!)
            {
                var component = ComponentNode.ParseComponent(token);
                if (component is not null)
                    _components.Add(component);
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
                yield return new TreeViewNode($"PrefabName = {PrefabName}");
                yield return new TreeViewNode($"Id = {Id}");
                yield return new TreeViewNode($"Cost = {Cost}");
                if (Strength is not null)
                    yield return new TreeViewNode($"Strength = {Strength}");
                if (Health is not null)
                    yield return new TreeViewNode($"Health = {Health}");
                yield return _tribesNode;
                yield return new TreeViewNode($"Classes = {string.Join(", ", Classes)}");
            }
        }

        public IEnumerable<ComponentNode> ComponentsViewData => _components;

        public void UpdateComponentsView()
        {
            System.Diagnostics.Debug.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(_data, Newtonsoft.Json.Formatting.Indented));
            UpdateProperty(nameof(ComponentsViewData));
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
        [InternalKey("Peashooter")]
        Pea = 0,
        Berry,
        Bean,
        Flower,
        Mushroom,
        Nut,
        Sports,
        Science,
        Dancing,
        Imp,
        Pet,
        Gargantuar,
        Pirate,
        Pinecone,
        Mustache = 15,
        Party,
        Gourmet = 18,
        History,
        Barrel,
        Seed,
        Animal,
        Cactus,
        Corn,
        Dragon,
        Flytrap,
        Fruit,
        Leafy,
        Moss,
        Root = 31,
        Squash,
        Tree,
        Clock = 35,
        Professional = 37,
        Monster = 39,
        Banana,
        Mime
    }

    public enum CardClass
    {
        [InternalKey("MegaGro")]
        MegaGrow,
        Smarty,
        Kabloom,
        Guardian,
        Solar,
        Brainy,
        Hearty,
        [InternalKey("Hungry")]
        Beastly,
        [InternalKey("Madcap")]
        Crazy,
        Sneaky
    }

    public enum CardAbilityIcon
    {
        AntiHero = 9,
        DoubleStrike = 11,
        Overshoot,
        Star
    }
}
