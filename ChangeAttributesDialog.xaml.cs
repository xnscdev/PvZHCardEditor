using System.Linq;
using System.Windows;

namespace PvZHCardEditor
{
    /// <summary>
    /// Interaction logic for ChangeAttributesDialog.xaml
    /// </summary>
    public partial class ChangeAttributesDialog : Window
    {
        internal ChangeAttributesViewModel Model { get; }

        public ChangeAttributesDialog(CardTribe[] tribes, CardClass[] classes, CardSet set, CardRarity rarity)
        {
            InitializeComponent();

            Model = (ChangeAttributesViewModel)DataContext;
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
            Model.Set = set;
            Model.Rarity = rarity;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
