using System.Threading.Tasks;
using PvZHCardEditor.ViewModels;
using ReactiveUI;

namespace PvZHCardEditor.Models;

public abstract class ComponentValue : ReactiveObject
{
    public abstract string? Text { get; }
    public abstract FullObservableCollection<ComponentProperty> Children { get; }

    public virtual bool IsExpanded { get; set; }

    public abstract Task Edit(MainWindowViewModel model);
}