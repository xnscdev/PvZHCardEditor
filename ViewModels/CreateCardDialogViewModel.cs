using System;
using System.Collections.Generic;
using PvZHCardEditor.Models;
using ReactiveUI;

namespace PvZHCardEditor.ViewModels;

public class CreateCardDialogViewModel : EditDialogViewModel
{
    private CardFaction _faction;
    private string _prefabName = string.Empty;
    private CardType _type;

    public static IEnumerable<CardType> CardTypes => Enum.GetValues<CardType>();
    public static IEnumerable<CardFaction> CardFactions => Enum.GetValues<CardFaction>();

    public string PrefabName
    {
        get => _prefabName;
        set => this.RaiseAndSetIfChanged(ref _prefabName, value);
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
}