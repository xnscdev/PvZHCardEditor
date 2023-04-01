using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Collections;
using Newtonsoft.Json.Linq;
using PvZHCardEditor.Models;
using ReactiveUI;

namespace PvZHCardEditor.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string? _cacheDir;
    private bool _dataLoaded;
    private string _id = string.Empty;
    private CardData? _loadedCard;
    private ReactiveObject? _selectedItem;
    private bool _statusShown = true;
    private string _statusText = "Open a folder to begin editing";

    public MainWindowViewModel()
    {
        var dataLoaded = this.WhenAnyValue(x => x.DataLoaded);
        OpenCommand = ReactiveCommand.CreateFromTask(DoOpenAsync);
        SaveCommand = ReactiveCommand.Create(DoSave, dataLoaded);
        SaveAsCommand = ReactiveCommand.CreateFromTask(DoSaveAsAsync, dataLoaded);
        LoadCardCommand = ReactiveCommand.Create(DoLoadCard, dataLoaded);
        EditValueCommand = ReactiveCommand.CreateFromTask<bool>(DoEditValueAsync, dataLoaded);
        EditDescriptionCommand = ReactiveCommand.CreateFromTask(DoEditDescriptionAsync, dataLoaded);
        EditStatsCommand = ReactiveCommand.CreateFromTask(DoEditStatsAsync, dataLoaded);
        EditTribesCommand = ReactiveCommand.CreateFromTask(DoEditTribesAsync, dataLoaded);
        EditAttributesCommand = ReactiveCommand.CreateFromTask(DoEditAttributesAsync, dataLoaded);
    }

    public ReactiveCommand<Unit, Unit> OpenCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveAsCommand { get; }
    public ReactiveCommand<Unit, Unit> LoadCardCommand { get; }
    public ReactiveCommand<bool, Unit> EditValueCommand { get; }
    public ReactiveCommand<Unit, Unit> EditDescriptionCommand { get; }
    public ReactiveCommand<Unit, Unit> EditStatsCommand { get; }
    public ReactiveCommand<Unit, Unit> EditTribesCommand { get; }
    public ReactiveCommand<Unit, Unit> EditAttributesCommand { get; }

    public Interaction<MainWindowViewModel, string?> ShowSelectFolderDialog { get; } = new();
    public Interaction<string, bool> ShowYesNoDialog { get; } = new();
    public Interaction<EditDialogViewModel, bool> ShowEditPrimitiveDialog { get; } = new();
    public Interaction<EditDialogViewModel, bool> ShowEditListDialog { get; } = new();
    public Interaction<EditDialogViewModel, bool> ShowEditObjectDialog { get; } = new();
    public Interaction<EditDialogViewModel, bool> ShowEditComponentDialog { get; } = new();
    public Interaction<EditDialogViewModel, bool> ShowEditOptionalComponentDialog { get; } = new();
    public Interaction<EditDialogViewModel, bool> ShowEditDescriptionDialog { get; } = new();
    public Interaction<EditDialogViewModel, bool> ShowEditStatsDialog { get; } = new();
    public Interaction<EditDialogViewModel, bool> ShowEditTribesDialog { get; } = new();
    public Interaction<EditDialogViewModel, bool> ShowEditAttributesDialog { get; } = new();

    public string Id
    {
        get => _id;
        set => this.RaiseAndSetIfChanged(ref _id, value);
    }

    public string StatusText
    {
        get => _statusText;
        set
        {
            this.RaiseAndSetIfChanged(ref _statusText, value);
            // I couldn't find a better way to replay the status fade animation
            StatusShown = false;
            StatusShown = true;
        }
    }

    public bool StatusShown
    {
        get => _statusShown;
        set => this.RaiseAndSetIfChanged(ref _statusShown, value);
    }

    public bool DataLoaded
    {
        get => _dataLoaded;
        set => this.RaiseAndSetIfChanged(ref _dataLoaded, value);
    }

    public CardData? LoadedCard
    {
        get => _loadedCard;
        set => this.RaiseAndSetIfChanged(ref _loadedCard, value);
    }

    public ReactiveObject? SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    private async Task DoOpenAsync()
    {
        if (GameDataManager.Modified &&
            await ShowYesNoDialog.Handle("Save modifications to current workspace before opening?") &&
            !GameDataManager.SaveData(_cacheDir!))
        {
            StatusText = "Failed to save current workspace, open cancelled";
            return;
        }

        var result = await ShowSelectFolderDialog.Handle(this);
        if (result == null)
            return;

        if (GameDataManager.OpenData(result))
        {
            StatusText = "Opened folder " + result;
            DataLoaded = true;
            _cacheDir = result;
        }
        else
        {
            StatusText = "Folder does not contain valid game data";
            DataLoaded = false;
            _cacheDir = null;
        }
    }

    private void DoSave()
    {
        if (_cacheDir == null)
        {
            StatusText = "No workspace open to save";
            return;
        }

        LoadedCard?.Save();
        if (GameDataManager.SaveData(_cacheDir))
            StatusText = "Saved workspace to " + _cacheDir;
        else
            StatusText = "Failed to save workspace to " + _cacheDir;
    }

    private async Task DoSaveAsAsync()
    {
        if (_cacheDir == null)
        {
            StatusText = "No workspace open to save";
            return;
        }

        var result = await ShowSelectFolderDialog.Handle(this);
        if (result == null)
            return;

        LoadedCard?.Save();
        if (GameDataManager.SaveData(result))
        {
            StatusText = "Saved workspace to " + result;
            _cacheDir = result;
        }
        else
        {
            StatusText = "Failed to save workspace to " + result;
        }
    }

    private void DoLoadCard()
    {
        if (Id.Length == 0)
            return;
        LoadedCard?.Save();
        LoadedCard = GameDataManager.LoadCard(Id);
        if (LoadedCard != null)
        {
            StatusText = $"Loaded card with ID {Id}";
            Id = string.Empty;
        }
        else
        {
            StatusText = $"No card exists with ID {Id}";
        }
    }

    private async Task DoEditValueAsync(bool real)
    {
        if (SelectedItem == null)
            return;
        var value = SelectedItem switch
        {
            ComponentProperty p => p.Value,
            ComponentWrapper<EntityComponent> c => c,
            ComponentWrapper<EntityQuery> c => c,
            _ => throw new ArgumentException("Attempted to edit item with no value")
        };
        await value.Edit(this, real);
    }

    private async Task DoEditDescriptionAsync()
    {
        if (LoadedCard == null)
            return;
        var editModel = new EditDescriptionDialogViewModel
        {
            DisplayName = LoadedCard.DisplayName,
            ShortText = LoadedCard.ShortText,
            LongText = LoadedCard.LongText.Replace("\\n", "\n"),
            FlavorText = LoadedCard.FlavorText,
            TargetingText = LoadedCard.TargetingText ?? string.Empty,
            HeraldFighterText = LoadedCard.HeraldFighterText ?? string.Empty,
            HeraldTrickText = LoadedCard.HeraldTrickText ?? string.Empty
        };
        var result = await ShowEditDescriptionDialog.Handle(editModel);
        if (!result)
            return;
        LoadedCard.DisplayName = editModel.DisplayName;
        LoadedCard.ShortText = editModel.ShortText;
        LoadedCard.LongText = editModel.LongText.Replace("\n", "\\n");
        LoadedCard.FlavorText = editModel.FlavorText;
        LoadedCard.TargetingText = editModel.TargetingText.Length > 0 ? editModel.TargetingText : null;
        LoadedCard.HeraldFighterText = editModel.HeraldFighterText.Length > 0 ? editModel.HeraldFighterText : null;
        LoadedCard.HeraldTrickText = editModel.HeraldTrickText.Length > 0 ? editModel.HeraldTrickText : null;
        LoadedCard.UpdateCardInfo();
    }

    private async Task DoEditStatsAsync()
    {
        if (LoadedCard == null)
            return;
        var editModel = new EditStatsDialogViewModel
        {
            Cost = LoadedCard.Cost,
            Strength = LoadedCard.Strength ?? 0,
            Health = LoadedCard.Health ?? 0,
            IsFighter = LoadedCard.Type == CardType.Fighter
        };
        var result = await ShowEditStatsDialog.Handle(editModel);
        if (!result)
            return;
        LoadedCard.Cost = editModel.Cost;
        if (editModel.IsFighter)
        {
            LoadedCard.Strength = editModel.Strength;
            LoadedCard.Health = editModel.Health;
        }

        LoadedCard.UpdateCardInfo();
    }

    private async Task DoEditTribesAsync()
    {
        if (LoadedCard == null)
            return;
        var editModel = new EditTribesDialogViewModel();
        foreach (var x in editModel.TribeCheckBoxes)
            if (LoadedCard.Tribes.Contains(x.Value))
                x.IsSelected = true;
        foreach (var x in editModel.ClassCheckBoxes)
            if (LoadedCard.Classes.Contains(x.Value))
                x.IsSelected = true;
        var result = await ShowEditTribesDialog.Handle(editModel);
        if (!result)
            return;
        LoadedCard.Tribes = editModel.SelectedTribes;
        LoadedCard.Classes = editModel.SelectedClasses;
        LoadedCard.UpdateCardInfo();
    }

    private async Task DoEditAttributesAsync()
    {
        if (LoadedCard == null)
            return;
        var allowCrafting = LoadedCard.Token.ContainsKey("craftingBuy");
        var editModel = new EditAttributesDialogViewModel
        {
            Set = LoadedCard.Set,
            Rarity = LoadedCard.Rarity,
            IsPower = (bool)LoadedCard.Token["isPower"]!,
            IsPrimaryPower = (bool)LoadedCard.Token["isPrimaryPower"]!,
            IsFighter = (bool)LoadedCard.Token["isFighter"]!,
            IsEnv = (bool)LoadedCard.Token["isEnv"]!,
            IsAquatic = (bool)LoadedCard.Token["isAquatic"]!,
            IsTeamup = (bool)LoadedCard.Token["isTeamup"]!,
            IgnoreDeckLimit = (bool)LoadedCard.Token["ignoreDeckLimit"]!,
            Usable = (bool)LoadedCard.Token["usable"]!,
            AllowCrafting = LoadedCard.Token.ContainsKey("craftingBuy"),
            BuyPrice = allowCrafting ? (int)LoadedCard.Token["craftingBuy"]! : 0,
            SellPrice = allowCrafting ? (int?)LoadedCard.Token["craftingSell"] ?? 0 : 0,
            TagEntries =
                new AvaloniaList<TextBoxWrapper>(LoadedCard.Token["tags"]!.Select(t => new TextBoxWrapper((string)t!)))
        };
        var abilities = LoadedCard.Token["special_abilities"]!
            .Select(a =>
                Enum.GetValues<CardSpecialAbility>().Where(x => x.GetInternalKey() == (string)a!)
                    .Select(x => (CardSpecialAbility?)x).DefaultIfEmpty(null).First()).Where(a => a != null)
            .Select(a => a!.Value).ToArray();
        foreach (var x in editModel.SpecialAbilities)
            if (abilities.Contains(x.Value))
                x.IsSelected = true;
        var result = await ShowEditAttributesDialog.Handle(editModel);
        if (!result)
            return;
        LoadedCard.Set = editModel.Set;
        LoadedCard.Rarity = editModel.Rarity;
        LoadedCard.Token["isPower"] = editModel.IsPower;
        LoadedCard.Token["isPrimaryPower"] = editModel.IsPrimaryPower;
        LoadedCard.Token["isFighter"] = editModel.IsFighter;
        LoadedCard.Token["isEnv"] = editModel.IsEnv;
        LoadedCard.Token["isAquatic"] = editModel.IsAquatic;
        LoadedCard.Token["isTeamup"] = editModel.IsTeamup;
        LoadedCard.Token["ignoreDeckLimit"] = editModel.IgnoreDeckLimit;
        LoadedCard.Token["usable"] = editModel.Usable;
        if (editModel.BuyPrice != 0)
            LoadedCard.Token["craftingBuy"] = editModel.BuyPrice;
        else
            LoadedCard.Token.Remove("craftingBuy");
        if (editModel.SellPrice != 0)
            LoadedCard.Token["craftingSell"] = editModel.SellPrice;
        else
            LoadedCard.Token.Remove("craftingSell");
        LoadedCard.Token["special_abilities"] = new JArray(editModel.SelectedAbilities.Select(x => x.GetInternalKey()));
        if (editModel.SelectedAbilities.Length > 0)
            LoadedCard.FindInsertComponent<ShowTriggeredIconComponent>().Abilities.SetElements(
                new FullObservableCollection<ComponentPrimitive<int>>(
                    editModel.SelectedAbilities.Select(x => new ComponentPrimitive<int>((int)x))));
        else
            LoadedCard.RemoveComponent<ShowTriggeredIconComponent>();
        LoadedCard.Token["tags"] = new JArray(editModel.TagEntries.Select(x => x.Text));
        LoadedCard.FindInsertComponent<TagsComponent>().Tags.SetElements(
            new FullObservableCollection<ComponentPrimitive<string>>(
                editModel.TagEntries.Select(x => new ComponentPrimitive<string>(x.Text))));
        LoadedCard.UpdateCardInfo();
    }
}