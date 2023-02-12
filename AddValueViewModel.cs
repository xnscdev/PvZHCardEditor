using System.Collections.Generic;
using System.Linq;

namespace PvZHCardEditor
{
    internal class AddValueViewModel : EditValueViewModel
    {
        private ValueTargetType _targetType;
        private string _key = "";
        private int? _index;
        private IEnumerable<string>? _existingKeys;

        public ValueTargetType TargetType
        {
            get => _targetType;
            set
            {
                SetProperty(ref _targetType, value);
                UpdateProperty(nameof(ExistingKey));
            }
        }

        public string Key
        {
            get => _key;
            set
            {
                SetProperty(ref _key, value);
                UpdateProperty(nameof(ExistingKey));
            }
        }

        public int? Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }

        public IEnumerable<string>? ExistingKeys
        {
            get => _existingKeys;
            set => SetProperty(ref _existingKeys, value);
        }

        public bool ExistingKey
        {
            get
            {
                if (TargetType != ValueTargetType.Key)
                    return false;
                if (Key.Length == 0)
                    return true;
                return ExistingKeys?.Where(k => Key == k).Any() is true;
            }
        }
    }

    public enum ValueTargetType
    {
        None,
        Key,
        Index
    }
}
