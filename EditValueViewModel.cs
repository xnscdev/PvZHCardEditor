using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace PvZHCardEditor
{
    internal class EditValueViewModel : ViewModelBase
    {
        private EditValueType _type;
        private int _integerValue;
        private string _stringValue = "";
        private bool _boolValue;

        public EditValueType Type
        {
            get => _type;
            set => SetProperty(ref _type, value, null);
        }

        public int IntegerValue
        {
            get => _integerValue;
            set => SetProperty(ref _integerValue, value);
        }

        public string StringValue
        {
            get => _stringValue;
            set => SetProperty(ref _stringValue, value);
        }

        public bool BoolValue
        {
            get => _boolValue;
            set => SetProperty(ref _boolValue, value);
        }

        public IEnumerable<EditValueType> EditValueTypes => Enum.GetValues(typeof(EditValueType)).Cast<EditValueType>();
    }

    public enum EditValueType
    {
        Integer,
        String,
        Boolean,
        Object,
        Array,
        Component,
        Null
    }
}
