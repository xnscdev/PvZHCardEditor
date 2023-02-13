using System.Windows;

namespace PvZHCardEditor
{
    /// <summary>
    /// Interaction logic for ChangeCostStatsDialog.xaml
    /// </summary>
    public partial class ChangeCostStatsDialog : Window
    {
        internal ChangeCostStatsViewModel Model { get; }

        public ChangeCostStatsDialog(bool isFighter)
        {
            InitializeComponent();

            Model = (ChangeCostStatsViewModel)DataContext;
            Model.IsFighter = isFighter;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
