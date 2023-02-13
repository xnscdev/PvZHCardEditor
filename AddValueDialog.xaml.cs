using System.Collections.Generic;
using System.Windows;

namespace PvZHCardEditor
{
    /// <summary>
    /// Interaction logic for AddValueDialog.xaml
    /// </summary>
    public partial class AddValueDialog : Window
    {
        internal AddValueViewModel Model { get; }

        public AddValueDialog(ValueTargetType targetType, IEnumerable<string> ExistingKeys)
        {
            InitializeComponent();

            Model = (AddValueViewModel)DataContext;
            Model.TargetType = targetType;
            Model.ExistingKeys = ExistingKeys;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
