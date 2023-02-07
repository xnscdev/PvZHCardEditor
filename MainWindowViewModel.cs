using System.Windows;
using System.Windows.Input;

namespace PvZHCardEditor
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private string _loadId = "";
        private CardData? _loadedCard;

        public ICommand LoadCardCommand => new DelegateCommand(LoadCard);
        public ICommand ChangeCostStatsCommand => new DelegateCommand(ChangeCostStats);

        public string LoadId
        {
            get => _loadId;
            set => SetProperty(ref _loadId, value);
        }

        public CardData? LoadedCard
        {
            get => _loadedCard;
            set => SetProperty(ref _loadedCard, value);
        }

        private void LoadCard(object? parameter)
        {
            LoadedCard = GameDataManager.LoadCard(LoadId);
            if (LoadedCard is null)
                MessageBox.Show($"No card exists with ID {LoadId}", "Load Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ChangeCostStats(object? parameter)
        {
            if (LoadedCard is null)
                return;
            LoadedCard.Cost = 69;
        }
    }
}
