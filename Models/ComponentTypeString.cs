using System.Reactive.Linq;
using System.Threading.Tasks;
using PvZHCardEditor.ViewModels;

namespace PvZHCardEditor.Models;

public class ComponentTypeString : ComponentPrimitive<string>
{
    public ComponentTypeString(string value) : base(value)
    {
    }

    public override string Text => EntityComponentBase.ParseHasComponentTypeString(Value);

    public override async Task Edit(MainWindowViewModel model)
    {
        var editModel = new EditPrimitiveDialogViewModel<string>
        {
            Value = Text
        };
        var result = await model.ShowEditPrimitiveDialog.Handle(editModel);
        if (!result)
            return;
        Value = EntityComponentBase.GetHasComponentTypeString(editModel.Value);
    }
}