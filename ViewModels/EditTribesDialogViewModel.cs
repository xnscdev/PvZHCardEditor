using System;
using System.Collections.Generic;
using System.Linq;
using PvZHCardEditor.Models;

namespace PvZHCardEditor.ViewModels;

public class EditTribesDialogViewModel : EditDialogViewModel
{
    private readonly CheckBoxWrapper<CardTribe>[] _allTribes = Enum.GetValues<CardTribe>()
        .Where(x => x.GetAttribute<FactionOnlyAttribute>()?.Faction == CardFaction.All)
        .Select(x => new CheckBoxWrapper<CardTribe>(x)).ToArray();

    private readonly CheckBoxWrapper<CardClass>[] _plantClasses = Enum.GetValues<CardClass>()
        .Where(x => x.GetAttribute<FactionOnlyAttribute>()?.Faction == CardFaction.Plants)
        .Select(x => new CheckBoxWrapper<CardClass>(x)).ToArray();

    private readonly CheckBoxWrapper<CardTribe>[] _plantTribes = Enum.GetValues<CardTribe>()
        .Where(x => x.GetAttribute<FactionOnlyAttribute>()?.Faction == CardFaction.Plants)
        .Select(x => new CheckBoxWrapper<CardTribe>(x)).ToArray();

    private readonly CheckBoxWrapper<CardClass>[] _zombieClasses = Enum.GetValues<CardClass>()
        .Where(x => x.GetAttribute<FactionOnlyAttribute>()?.Faction == CardFaction.Zombies)
        .Select(x => new CheckBoxWrapper<CardClass>(x)).ToArray();

    private readonly CheckBoxWrapper<CardTribe>[] _zombieTribes = Enum.GetValues<CardTribe>()
        .Where(x => x.GetAttribute<FactionOnlyAttribute>()?.Faction == CardFaction.Zombies)
        .Select(x => new CheckBoxWrapper<CardTribe>(x)).ToArray();

    public IEnumerable<CheckBoxWrapper<CardClass>> PlantClasses => _plantClasses;
    public IEnumerable<CheckBoxWrapper<CardClass>> ZombieClasses => _zombieClasses;
    public IEnumerable<CheckBoxWrapper<CardTribe>> PlantTribes => _plantTribes;
    public IEnumerable<CheckBoxWrapper<CardTribe>> ZombieTribes => _zombieTribes;
    public IEnumerable<CheckBoxWrapper<CardTribe>> AllTribes => _allTribes;
    public IEnumerable<CheckBoxWrapper<CardClass>> ClassCheckBoxes => PlantClasses.Concat(ZombieClasses);

    public IEnumerable<CheckBoxWrapper<CardTribe>> TribeCheckBoxes =>
        PlantTribes.Concat(ZombieTribes).Concat(AllTribes);

    public CardClass[] SelectedClasses => ClassCheckBoxes.Where(x => x.IsSelected).Select(x => x.Value).ToArray();
    public CardTribe[] SelectedTribes => TribeCheckBoxes.Where(x => x.IsSelected).Select(x => x.Value).ToArray();
}