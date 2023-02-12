using System.Collections.Generic;
using System.Linq;

namespace PvZHCardEditor
{
    internal class AddComponentViewModel : ViewModelBase
    {
        private string _componentType;

        public string ComponentType
        {
            get => _componentType;
            set => SetProperty(ref _componentType, value);
        }

        public IEnumerable<string> ComponentTypes => GameDataManager.ComponentTypes;

        public AddComponentViewModel()
        {
            _componentType = ComponentTypes.First();
        }
    }
}
