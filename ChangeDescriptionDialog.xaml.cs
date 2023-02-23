using System.Windows;

namespace PvZHCardEditor
{
    /// <summary>
    /// Interaction logic for ChangeDescriptionDialog.xaml
    /// </summary>
    public partial class ChangeDescriptionDialog : Window
    {
        internal ChangeDescriptionViewModel Model { get; }

        public ChangeDescriptionDialog(string name, string shortDescription, string longDescription, string flavorText, string? targeting, string? heraldFighter, string? heraldTrick)
        {
            InitializeComponent();

            Model = (ChangeDescriptionViewModel)DataContext;
            Model.Name = name;
            Model.ShortDescription = shortDescription;
            Model.LongDescription = longDescription;
            Model.FlavorText = flavorText;
            if (targeting is not null)
                Model.Targeting = targeting;
            if (heraldFighter is not null)
                Model.HeraldFighter = heraldFighter;
            if (heraldTrick is not null)
                Model.HeraldTrick = heraldTrick;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
