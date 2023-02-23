using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace PvZHCardEditor
{
    internal class FindCardViewModel : ViewModelBase
    {
        private string _name = "";
        private int? _cost;
        private int? _strength;
        private int? _health;
        private CardType _type;
        private CardFaction _faction;
        private IEnumerable<CardData> _results = Array.Empty<CardData>();

        public ICommand SearchCommand => new DelegateCommand(SearchCard);

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public int? Cost
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

        public IEnumerable<CardData> Results
        {
            get => _results;
            set => SetProperty(ref _results, value);
        }

        public IEnumerable<CardType> CardTypes => GameDataManager.CardTypes;

        private void SearchCard(object? parameter)
        {
            Results = GameDataManager.FindCards(Name, Cost, Strength, Health, Type, Faction);
        }
    }
}
