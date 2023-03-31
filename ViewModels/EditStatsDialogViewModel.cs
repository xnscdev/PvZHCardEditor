using ReactiveUI;

namespace PvZHCardEditor.ViewModels;

public class EditStatsDialogViewModel : EditDialogViewModel
{
    private int _cost;
    private int _health;
    private bool _isFighter;
    private int _strength;

    public int Cost
    {
        get => _cost;
        set => this.RaiseAndSetIfChanged(ref _cost, value);
    }

    public int Strength
    {
        get => _strength;
        set => this.RaiseAndSetIfChanged(ref _strength, value);
    }

    public int Health
    {
        get => _health;
        set => this.RaiseAndSetIfChanged(ref _health, value);
    }

    public bool IsFighter
    {
        get => _isFighter;
        set => this.RaiseAndSetIfChanged(ref _isFighter, value);
    }
}