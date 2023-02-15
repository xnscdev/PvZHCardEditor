using System.Linq;
using System.Windows;

namespace PvZHCardEditor
{
    /// <summary>
    /// Interaction logic for ChangeAttributesDialog.xaml
    /// </summary>
    public partial class ChangeTribesDialog : Window
    {
        internal ChangeTribesViewModel Model { get; }

        public ChangeTribesDialog(CardTribe[] tribes, CardClass[] classes)
        {
            InitializeComponent();

            Model = (ChangeTribesViewModel)DataContext;
            foreach (var x in Model.TribeCheckboxes)
            {
                if (tribes.Contains(x.Value))
                    x.IsSelected = true;
            }
            foreach (var x in Model.ClassCheckboxes)
            {
                if (classes.Contains(x.Value))
                    x.IsSelected = true;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
