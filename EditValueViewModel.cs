﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace PvZHCardEditor
{
    internal class EditValueViewModel : ViewModelBase
    {
        private EditValueType _type;
        private int _integerValue;
        private string _stringValue = "";
        private bool _boolValue;
        private string _componentValue;
        private string _queryValue;

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

        public string QueryValue
        {
            get => _queryValue;
            set => SetProperty(ref _queryValue, value);
        }

        public IEnumerable<EditValueType> EditValueTypes => Enum.GetValues(typeof(EditValueType)).Cast<EditValueType>();
        public IEnumerable<string> ComponentTypes => GameDataManager.ComponentTypes;
        public IEnumerable<string> QueryTypes => GameDataManager.QueryTypes;

        public EditValueViewModel()
        {
            _componentValue = ComponentTypes.First();
            _queryValue = QueryTypes.First();
        }
    }

    public enum EditValueType
    {
        Component,
        Query,
        Integer,
        String,
        Boolean,
        Object,
        Array,
        Null
    }
}
