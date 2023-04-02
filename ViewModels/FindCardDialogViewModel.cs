using System;
using System.Collections.Generic;
using System.Reactive;
using PvZHCardEditor.Models;
using ReactiveUI;

namespace PvZHCardEditor.ViewModels;

public class FindCardDialogViewModel : ViewModelBase
{
    private int? _cost;
    private CardFaction _faction = CardFaction.All;
    private bool _filterType;
    private int? _health;
    private string _name = string.Empty;
    private IEnumerable<FindCardResult> _results = Array.Empty<FindCardResult>();
    private int? _strength;
    private CardType _type;

    public FindCardDialogViewModel()
    {
        SearchCommand = ReactiveCommand.Create(DoSearch);
        CloseCommand = ReactiveCommand.Create(() => true);
    }

    public ReactiveCommand<Unit, Unit> SearchCommand { get; }
    public ReactiveCommand<Unit, bool> CloseCommand { get; }

    public static IEnumerable<CardType> CardTypes => Enum.GetValues<CardType>();
    public static IEnumerable<CardFaction> CardFactions => Enum.GetValues<CardFaction>();

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public int? Cost
    {
        get => _cost;
        set => this.RaiseAndSetIfChanged(ref _cost, value);
    }

    public int? Strength
    {
        get => _strength;
        set => this.RaiseAndSetIfChanged(ref _strength, value);
    }

    public int? Health
    {
        get => _health;
        set => this.RaiseAndSetIfChanged(ref _health, value);
    }

    public bool FilterType
    {
        get => _filterType;
        set => this.RaiseAndSetIfChanged(ref _filterType, value);
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

    public IEnumerable<FindCardResult> Results
    {
        get => _results;
        set => this.RaiseAndSetIfChanged(ref _results, value);
    }

    private void DoSearch()
    {
        Results = GameDataManager.FindCards(this);
    }
}