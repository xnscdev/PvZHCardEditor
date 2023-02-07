using System.Collections.Generic;

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

        public string PrefabName
        {
            get => _prefabName;
            set => SetProperty(ref _prefabName, value);
        }

        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }

        public string ShortText
        {
            get => _shortText;
            set => SetProperty(ref _shortText, value);
        }

        public string LongText
        {
            get => _longText;
            set => SetProperty(ref _longText, value);
        }

        public string FlavorText
        {
            get => _flavorText;
            set => SetProperty(ref _flavorText, value);
        }

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public int Cost
        {
            get => _cost;
            set => SetProperty(ref _cost, value);
        }

        public int? Strength
        {
            get => _strength;
            set => SetProperty(ref _strength, value);
        }

        public int? Health
        {
            get => _health;
            set => SetProperty(ref _health, value);
        }

        public CardData(string prefabName, string displayName, string shortText, string longText, string flavorText,
            string id, int cost, int? strength, int? health)
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
        }

        public string ResultViewTitle => $"{Id}: {DisplayName}";

        public IEnumerable<string> ResultViewData
        {
            get
            {
                yield return $"DisplayName = {DisplayName}";
                yield return $"PrefabName = {PrefabName}";
                yield return $"Id = {Id}";
                yield return $"Cost = {Cost}";
                if (Strength is not null)
                    yield return $"Strength = {Strength}";
                if (Health is not null)
                    yield return $"Health = {Health}";
            }
        }
    }
}
