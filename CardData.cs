using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PvZHCardEditor
{
    public class CardData : ViewModelBase
    {
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
        private TreeViewCompoundNode _tribesNode;

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
            set => SetProperty(ref _type, value, null);
        }

        public CardFaction Faction
        {
            get => _faction;
            set => SetProperty(ref _faction, value, null);
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

        public CardData(string prefabName, string displayName, string shortText, string longText, string flavorText,
            string id, int cost, int? strength, int? health, CardType type, CardFaction faction, CardTribe[] tribes)
        {
            _prefabName = prefabName;
            _displayName = displayName;
            _shortText = shortText;
            _longText = longText;
            _flavorText = flavorText;
            _id = id;
            _cost = cost;
            _strength = strength;
            _health = health;
            _type = type;
            _faction = faction;
            _tribes = tribes;
            _tribesNode = new TreeViewCompoundNode("Tribes", tribes.Select(t => new TreeViewNode(t.ToString())));
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
            }
        }
    }

    public enum CardType
    {
        Fighter,
        Trick,
        Environment
    }

    public enum CardFaction
    {
        Plants,
        Zombies
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
}
