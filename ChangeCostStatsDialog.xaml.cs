using System.Windows;

namespace PvZHCardEditor
{
    /// <summary>
    /// Interaction logic for ChangeCostStatsDialog.xaml
    /// </summary>
    public partial class ChangeCostStatsDialog : Window
    {
        internal ChangeCostStatsViewModel Model { get; }

        public ChangeCostStatsDialog(int cost, bool isFighter, int? strength, int? health)
        {
            InitializeComponent();

            Model = (ChangeCostStatsViewModel)DataContext;
            Model.Cost = cost;
            Model.IsFighter = isFighter;
            Model.Strength = isFighter ? strength ?? 0 : 0;
            Model.Health = isFighter ? health ?? 0 : 0;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
