using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PvZHCardEditor.ViewModels;

namespace PvZHCardEditor.Models;

[JsonConverter(typeof(ComponentTypeStringConverter))]
public class ComponentTypeString : ComponentPrimitive<string>
{
    public ComponentTypeString(string value) : base(value)
    {
    }

    public override string Text => EntityComponentBase.ParseHasComponentTypeString(Value);

    public override async Task<bool> Edit(MainWindowViewModel model, bool real)
    {
        var editModel = new EditPrimitiveDialogViewModel<string>
        {
            Value = Text
        };
        var result = await model.ShowEditPrimitiveDialog.Handle(editModel);
        if (!result)
            return false;
        Value = EntityComponentBase.GetHasComponentTypeString(editModel.Value);
        return true;
    }
}

public class ComponentTypeStringConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        var wrapper = (ComponentTypeString)value;
        var token = JToken.FromObject(wrapper.Value);
        token.WriteTo(writer);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        var token = JToken.Load(reader);
        return new ComponentTypeString((string)token!);
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(ComponentTypeString);
    }
}