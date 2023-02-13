using System.Windows;

namespace PvZHCardEditor
{
    /// <summary>
    /// Interaction logic for AddComponentDialog.xaml
    /// </summary>
    public partial class AddComponentDialog : Window
    {
        internal AddComponentViewModel Model { get; }

        public AddComponentDialog()
        {
            InitializeComponent();

            Model = (AddComponentViewModel)DataContext;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
