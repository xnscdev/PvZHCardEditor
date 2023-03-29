using ReactiveUI;

namespace PvZHCardEditor.ViewModels;

public class EditDescriptionDialogViewModel : EditDialogViewModel
{
    private string _displayName = string.Empty;
    private string _flavorText = string.Empty;
    private string _heraldFighterText = string.Empty;
    private string _heraldTrickText = string.Empty;
    private string _longText = string.Empty;
    private string _shortText = string.Empty;
    private string _targetingText = string.Empty;

    public string DisplayName
    {
        get => _displayName;
        set => this.RaiseAndSetIfChanged(ref _displayName, value);
    }

    public string ShortText
    {
        get => _shortText;
        set => this.RaiseAndSetIfChanged(ref _shortText, value);
    }

    public string LongText
    {
        get => _longText;
        set => this.RaiseAndSetIfChanged(ref _longText, value);
    }

    public string FlavorText
    {
        get => _flavorText;
        set => this.RaiseAndSetIfChanged(ref _flavorText, value);
    }

    public string TargetingText
    {
        get => _targetingText;
        set => this.RaiseAndSetIfChanged(ref _targetingText, value);
    }

    public string HeraldFighterText
    {
        get => _heraldFighterText;
        set => this.RaiseAndSetIfChanged(ref _heraldFighterText, value);
    }

    public string HeraldTrickText
    {
        get => _heraldTrickText;
        set => this.RaiseAndSetIfChanged(ref _heraldTrickText, value);
    }
}