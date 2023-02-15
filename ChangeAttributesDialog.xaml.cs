using System.Windows;

namespace PvZHCardEditor
{
    /// <summary>
    /// Interaction logic for ChangeAttributesDialog.xaml
    /// </summary>
    public partial class ChangeAttributesDialog : Window
    {
        internal ChangeAttributesViewModel Model { get; }

        public ChangeAttributesDialog(CardExtraAttributes data)
        {
            InitializeComponent();

            Model = (ChangeAttributesViewModel)DataContext;
            Model.SetValues(data);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
