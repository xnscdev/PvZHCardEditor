namespace PvZHCardEditor
{
    internal class ChangeDescriptionViewModel : ViewModelBase
    {
        private string _name = "";
        private string _shortDescription = "";
        private string _longDescription = "";
        private string _flavorText = "";
        private string _targeting = "";
        private string _heraldFighter = "";
        private string _heraldTrick = "";

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string ShortDescription
        {
            get => _shortDescription;
            set => SetProperty(ref _shortDescription, value);
        }

        public string LongDescription
        {
            get => _longDescription;
            set => SetProperty(ref _longDescription, value);
        }

        public string FlavorText
        {
            get => _flavorText;
            set => SetProperty(ref _flavorText, value);
        }

        public string Targeting
        {
            get => _targeting;
            set => SetProperty(ref _targeting, value);
        }

        public string HeraldFighter
        {
            get => _heraldFighter;
            set => SetProperty(ref _heraldFighter, value);
        }
        
        public string HeraldTrick
        {
            get => _heraldTrick;
            set => SetProperty(ref _heraldTrick, value);
        }
    }
}
