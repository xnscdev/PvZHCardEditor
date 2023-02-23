using System.Windows;

namespace PvZHCardEditor
{
    /// <summary>
    /// Interaction logic for CreateCardDialog.xaml
    /// </summary>
    public partial class CreateCardDialog : Window
    {
        internal CreateCardViewModel Model { get; }

        public CreateCardDialog()
        {
            InitializeComponent();

            Model = (CreateCardViewModel)DataContext;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
