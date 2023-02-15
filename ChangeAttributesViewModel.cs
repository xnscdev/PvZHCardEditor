using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PvZHCardEditor
{
    internal class ChangeAttributesViewModel : ViewModelBase
    {
        private readonly CheckboxEntry<CardClass>[] _plantClasses = Enum.GetValues<CardClass>()
            .Where(x => x.GetAttribute<FactionOnlyAttribute>()?.Faction == CardFaction.Plants).Select(x => new CheckboxEntry<CardClass>(x)).ToArray();
        private readonly CheckboxEntry<CardClass>[] _zombieClasses = Enum.GetValues<CardClass>()
            .Where(x => x.GetAttribute<FactionOnlyAttribute>()?.Faction == CardFaction.Zombies).Select(x => new CheckboxEntry<CardClass>(x)).ToArray();
        private readonly CheckboxEntry<CardTribe>[] _plantTribes = Enum.GetValues<CardTribe>()
            .Where(x => x.GetAttribute<FactionOnlyAttribute>()?.Faction == CardFaction.Plants).Select(x => new CheckboxEntry<CardTribe>(x)).ToArray();
        private readonly CheckboxEntry<CardTribe>[] _zombieTribes = Enum.GetValues<CardTribe>()
            .Where(x => x.GetAttribute<FactionOnlyAttribute>()?.Faction == CardFaction.Zombies).Select(x => new CheckboxEntry<CardTribe>(x)).ToArray();
        private readonly CheckboxEntry<CardTribe>[] _allTribes = Enum.GetValues<CardTribe>()
            .Where(x => x.GetAttribute<FactionOnlyAttribute>()?.Faction == CardFaction.All).Select(x => new CheckboxEntry<CardTribe>(x)).ToArray();
        private CardSet _set;
        private CardRarity _rarity;

        public IEnumerable<CheckboxEntry<CardClass>> PlantClasses => _plantClasses;
        public IEnumerable<CheckboxEntry<CardClass>> ZombieClasses => _zombieClasses;
        public IEnumerable<CheckboxEntry<CardTribe>> PlantTribes => _plantTribes;
        public IEnumerable<CheckboxEntry<CardTribe>> ZombieTribes => _zombieTribes;
        public IEnumerable<CheckboxEntry<CardTribe>> AllTribes => _allTribes;
        public IEnumerable<CheckboxEntry<CardClass>> ClassCheckboxes => PlantClasses.Concat(ZombieClasses);
        public IEnumerable<CheckboxEntry<CardTribe>> TribeCheckboxes => PlantTribes.Concat(ZombieTribes).Concat(AllTribes);
        public CardClass[] SelectedClasses => ClassCheckboxes.Where(x => x.IsSelected).Select(x => x.Value).ToArray();
        public CardTribe[] SelectedTribes => TribeCheckboxes.Where(x => x.IsSelected).Select(x => x.Value).ToArray();
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

        public ChangeAttributesViewModel()
        {
            foreach (var entry in ClassCheckboxes)
                entry.PropertyChanged += Entry_PropertyChanged;
            foreach (var entry in TribeCheckboxes)
                entry.PropertyChanged += Entry_PropertyChanged;
        }

        private void Entry_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateProperty(null);
        }
    }
}
