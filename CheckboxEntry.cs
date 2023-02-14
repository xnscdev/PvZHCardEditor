namespace PvZHCardEditor
{
    internal class CheckboxEntry<T> : ViewModelBase
    {
        private T _value;
        private bool _isSelected;

        public T Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public CheckboxEntry(T value)
        {
            _value = value;
        }
    }
}
