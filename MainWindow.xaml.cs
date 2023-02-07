using System.ComponentModel;
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

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (GameDataManager.PreventCloseUnsavedChanges())
                e.Cancel = true;
        }
    }
}
