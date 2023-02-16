using System.Collections.Generic;
using System;
using System.Linq;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace PvZHCardEditor
{
    internal class ChangeAttributesViewModel : ViewModelBase
    {
        private CardSet _set;
        private CardRarity _rarity;
        private bool _isPower;
        private bool _isPrimaryPower;
        private bool _isFighter;
        private bool _isEnv;
        private bool _isAquatic;
        private bool _isTeamup;
        private bool _ignoreDeckLimit;
        private bool _usable;
        private bool _allowCrafting;
        private int _buyPrice;
        private int _sellPrice;
        private ObservableCollection<TextboxEntry> _tagEntries = new();

        public ICommand AddTagCommand => new DelegateCommand(DoAddTag);
        public ICommand ClearTagCommand => new DelegateCommand(DoClearTag);
        public ICommand RemoveTagCommand => new DelegateCommand(DoRemoveTag);

        public IEnumerable<CardSet> SetTypes => Enum.GetValues<CardSet>();
        public IEnumerable<CardRarity> RarityTypes => Enum.GetValues<CardRarity>();

        public CardSet Set
        {
            get => _set;
            set => SetProperty(ref _set, value);
        }

        public CardRarity Rarity
        {
            get => _rarity;
            set => SetProperty(ref _rarity, value);
        }

        public bool IsPower
        {
            get => _isPower;
            set => SetProperty(ref _isPower, value);
        }

        public bool IsPrimaryPower
        {
            get => _isPrimaryPower;
            set => SetProperty(ref _isPrimaryPower, value);
        }

        public bool IsFighter
        {
            get => _isFighter;
            set => SetProperty(ref _isFighter, value);
        }

        public bool IsEnv
        {
            get => _isEnv;
            set => SetProperty(ref _isEnv, value);
        }

        public bool IsAquatic
        {
            get => _isAquatic;
            set => SetProperty(ref _isAquatic, value);
        }

        public bool IsTeamup
        {
            get => _isTeamup;
            set => SetProperty(ref _isTeamup, value);
        }

        public bool IgnoreDeckLimit
        {
            get => _ignoreDeckLimit;
            set => SetProperty(ref _ignoreDeckLimit, value);
        }

        public bool Usable
        {
            get => _usable;
            set => SetProperty(ref _usable, value);
        }

        public bool AllowCrafting
        {
            get => _allowCrafting;
            set => SetProperty(ref _allowCrafting, value);
        }

        public int BuyPrice
        {
            get => _buyPrice;
            set => SetProperty(ref _buyPrice, value);
        }

        public int SellPrice
        {
            get => _sellPrice;
            set => SetProperty(ref _sellPrice, value);
        }

        public ObservableCollection<TextboxEntry> TagEntries
        {
            get => _tagEntries;
            set => SetProperty(ref _tagEntries, value);
        }

        public CardExtraAttributes GetExtraAttributes()
        {
            return new CardExtraAttributes()
            {
                Set = Set,
                Rarity = Rarity,
                IsPower = IsPower,
                IsPrimaryPower = IsPrimaryPower,
                IsFighter = IsFighter,
                IsEnv = IsEnv,
                IsAquatic = IsAquatic,
                IsTeamup = IsTeamup,
                IgnoreDeckLimit = IgnoreDeckLimit,
                Usable = Usable,
                BuyPrice = AllowCrafting ? BuyPrice : null,
                SellPrice = AllowCrafting ? SellPrice : null,
                Tags = TagEntries.Select(t => t.Text).ToArray()
            };
        }

        public void SetValues(CardExtraAttributes data)
        {
            Set = data.Set;
            Rarity = data.Rarity;
            IsPower = data.IsPower;
            IsPrimaryPower = data.IsPrimaryPower;
            IsFighter = data.IsFighter;
            IsEnv = data.IsEnv;
            IsAquatic = data.IsAquatic;
            IsTeamup = data.IsTeamup;
            IgnoreDeckLimit = data.IgnoreDeckLimit;
            Usable = data.Usable;
            AllowCrafting = data.BuyPrice is not null;
            if (AllowCrafting)
            {
                BuyPrice = data.BuyPrice!.Value;
                SellPrice = data.SellPrice!.Value;
            }
            TagEntries = new ObservableCollection<TextboxEntry>(data.Tags.Select(t => new TextboxEntry(t)));
        }

        private void DoAddTag(object? parameter)
        {
            TagEntries.Add(new TextboxEntry());
        }

        private void DoClearTag(object? parameter)
        {
            TagEntries.Clear();
        }

        private void DoRemoveTag(object? parameter)
        {
            var entry = (TextboxEntry)parameter!;
            TagEntries.Remove(entry);
        }
    }

    public struct CardExtraAttributes
    {
        public CardSet Set;
        public CardRarity Rarity;
        public bool IsPower;
        public bool IsPrimaryPower;
        public bool IsFighter;
        public bool IsEnv;
        public bool IsAquatic;
        public bool IsTeamup;
        public bool IgnoreDeckLimit;
        public bool Usable;
        public int? BuyPrice;
        public int? SellPrice;
        public string[] Tags;
    }
}
