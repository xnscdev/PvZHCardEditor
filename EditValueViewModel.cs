using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace PvZHCardEditor
{
    internal class EditValueViewModel : ViewModelBase
    {
        private EditValueType _type;
        private int _integerValue;
        private string _stringValue = "";
        private bool _boolValue;
        private string _componentValue;

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

        public string ComponentValue
        {
            get => _componentValue;
            set => SetProperty(ref _componentValue, value);
        }

        public IEnumerable<EditValueType> EditValueTypes => Enum.GetValues(typeof(EditValueType)).Cast<EditValueType>();
        public IEnumerable<string> ComponentTypes => GetType().Assembly.GetTypes()
            .Where(t => t.Namespace == "PvZHCardEditor.Components" && !t.GetTypeInfo().IsDefined(typeof(CompilerGeneratedAttribute), true)).Select(t => t.Name);

        public EditValueViewModel()
        {
            _componentValue = ComponentTypes.First();
        }
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
