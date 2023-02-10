using System.Windows;

namespace PvZHCardEditor
{
    /// <summary>
    /// Interaction logic for EditValueDialog.xaml
    /// </summary>
    public partial class EditValueDialog : Window
    {
        internal EditValueViewModel Model { get; }

        public EditValueDialog(EditValueType? existingType)
        {
            InitializeComponent();
            
            Model = (EditValueViewModel)DataContext;
            if (existingType is not null)
                Model.Type = existingType.Value;
        }

        private void EditValueButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
