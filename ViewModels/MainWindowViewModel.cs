using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
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
    }

    public ReactiveCommand<Unit, Unit> OpenCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveAsCommand { get; }
    public ReactiveCommand<Unit, Unit> LoadCardCommand { get; }
    public ReactiveCommand<bool, Unit> EditValueCommand { get; }
    public ReactiveCommand<Unit, Unit> EditDescriptionCommand { get; }

    public Interaction<MainWindowViewModel, string?> ShowSelectFolderDialog { get; } = new();
    public Interaction<string, bool> ShowYesNoDialog { get; } = new();
    public Interaction<EditDialogViewModel, bool> ShowEditPrimitiveDialog { get; } = new();
    public Interaction<EditDialogViewModel, bool> ShowEditListDialog { get; } = new();
    public Interaction<EditDialogViewModel, bool> ShowEditObjectDialog { get; } = new();
    public Interaction<EditDialogViewModel, bool> ShowEditComponentDialog { get; } = new();
    public Interaction<EditDialogViewModel, bool> ShowEditOptionalComponentDialog { get; } = new();
    public Interaction<EditDialogViewModel, bool> ShowEditDescriptionDialog { get; } = new();

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
            _ => throw new ArgumentException("Attempted to edit item with no value", nameof(SelectedItem))
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
}