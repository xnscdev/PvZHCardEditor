using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Avalonia.Collections;
using PvZHCardEditor.Models;
using ReactiveUI;

namespace PvZHCardEditor.ViewModels;

public class EditAttributesDialogViewModel : EditDialogViewModel
{
    private readonly CheckBoxWrapper<CardSpecialAbility>[] _specialAbilities = Enum.GetValues<CardSpecialAbility>()
        .Select(x => new CheckBoxWrapper<CardSpecialAbility>(x)).ToArray();

    private bool _allowCrafting;
    private int _buyPrice;
    private bool _ignoreDeckLimit;
    private bool _isAquatic;
    private bool _isEnv;
    private bool _isFighter;
    private bool _isPower;
    private bool _isPrimaryPower;
    private bool _isTeamup;
    private CardRarity _rarity;
    private int _sellPrice;
    private CardSet _set;
    private AvaloniaList<TextBoxWrapper> _tagEntries = new();
    private bool _usable;

    public EditAttributesDialogViewModel()
    {
        AddTagCommand = ReactiveCommand.Create(DoAddTag);
        ClearTagCommand = ReactiveCommand.Create(DoClearTag);
        RemoveTagCommand = ReactiveCommand.Create<TextBoxWrapper>(DoRemoveTag);
    }

    public ReactiveCommand<Unit, Unit> AddTagCommand { get; }
    public ReactiveCommand<Unit, Unit> ClearTagCommand { get; }
    public ReactiveCommand<TextBoxWrapper, Unit> RemoveTagCommand { get; }

    public IEnumerable<CardSet> SetTypes => Enum.GetValues<CardSet>();
    public IEnumerable<CardRarity> RarityTypes => Enum.GetValues<CardRarity>();
    public IEnumerable<CheckBoxWrapper<CardSpecialAbility>> SpecialAbilities => _specialAbilities;

    public CardSpecialAbility[] SelectedAbilities =>
        SpecialAbilities.Where(x => x.IsSelected).Select(x => x.Value).ToArray();

    public CardSet Set
    {
        get => _set;
        set => this.RaiseAndSetIfChanged(ref _set, value);
    }

    public CardRarity Rarity
    {
        get => _rarity;
        set => this.RaiseAndSetIfChanged(ref _rarity, value);
    }

    public bool IsPower
    {
        get => _isPower;
        set => this.RaiseAndSetIfChanged(ref _isPower, value);
    }

    public bool IsPrimaryPower
    {
        get => _isPrimaryPower;
        set => this.RaiseAndSetIfChanged(ref _isPrimaryPower, value);
    }

    public bool IsFighter
    {
        get => _isFighter;
        set => this.RaiseAndSetIfChanged(ref _isFighter, value);
    }

    public bool IsEnv
    {
        get => _isEnv;
        set => this.RaiseAndSetIfChanged(ref _isEnv, value);
    }

    public bool IsAquatic
    {
        get => _isAquatic;
        set => this.RaiseAndSetIfChanged(ref _isAquatic, value);
    }

    public bool IsTeamup
    {
        get => _isTeamup;
        set => this.RaiseAndSetIfChanged(ref _isTeamup, value);
    }

    public bool IgnoreDeckLimit
    {
        get => _ignoreDeckLimit;
        set => this.RaiseAndSetIfChanged(ref _ignoreDeckLimit, value);
    }

    public bool Usable
    {
        get => _usable;
        set => this.RaiseAndSetIfChanged(ref _usable, value);
    }

    public bool AllowCrafting
    {
        get => _allowCrafting;
        set => this.RaiseAndSetIfChanged(ref _allowCrafting, value);
    }

    public int BuyPrice
    {
        get => _buyPrice;
        set => this.RaiseAndSetIfChanged(ref _buyPrice, value);
    }

    public int SellPrice
    {
        get => _sellPrice;
        set => this.RaiseAndSetIfChanged(ref _sellPrice, value);
    }

    public AvaloniaList<TextBoxWrapper> TagEntries
    {
        get => _tagEntries;
        set => this.RaiseAndSetIfChanged(ref _tagEntries, value);
    }

    private void DoAddTag()
    {
        TagEntries.Add(new TextBoxWrapper(string.Empty));
    }

    private void DoClearTag()
    {
        TagEntries.Clear();
    }

    private void DoRemoveTag(TextBoxWrapper entry)
    {
        TagEntries.Remove(entry);
    }
}