using System.Collections.Generic;

namespace PvZHCardEditor
{
    internal class CreateCardViewModel : ViewModelBase
    {
        private string _prefabName = "";
        private CardType _type;
        private CardFaction _faction;

        public string PrefabName
        {
            get => _prefabName;
            set => SetProperty(ref _prefabName, value);
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

        public IEnumerable<CardType> CardTypes => GameDataManager.CardTypes;
    }
}
