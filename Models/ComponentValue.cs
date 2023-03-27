using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PvZHCardEditor.ViewModels;
using ReactiveUI;

namespace PvZHCardEditor.Models;

public abstract class ComponentValue : ReactiveObject
{
    public abstract string? Text { get; }
    public abstract FullObservableCollection<ComponentProperty> Children { get; }

    public string IsolatedText
    {
        get
        {
            if (Text != null)
                return Text;
            var name = Regex.Replace(GetType().Name, "^Component", "");
            return $"({name})";
        }
    }

    public virtual bool IsExpanded { get; set; }

    public abstract Task Edit(MainWindowViewModel model);
}