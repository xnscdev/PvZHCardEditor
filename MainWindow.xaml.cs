using System.Windows;

namespace PvZHCardEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static MainWindow()
        {
            GameDataManager.Init();
        }
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FindCardButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FindCardDialog();
            dialog.ShowDialog();
        }

        private void AddComponentButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
