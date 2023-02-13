namespace PvZHCardEditor
{
    internal class ChangeCostStatsViewModel : ViewModelBase
    {
        private int _cost;
        private int _strength;
        private int _health;
        private bool _isFighter;

        public int Cost
        {
            get => _cost;
            set => SetProperty(ref _cost, value);
        }

        public int Strength
        {
            get => _strength;
            set => SetProperty(ref _strength, value);
        }

        public int Health
        {
            get => _health;
            set => SetProperty(ref _health, value);
        }

        public bool IsFighter
        {
            get => _isFighter;
            set => SetProperty(ref _isFighter, value);
        }
    }
}
