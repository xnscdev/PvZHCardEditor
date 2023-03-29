using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PvZHCardEditor.ViewModels;
using ReactiveUI;

namespace PvZHCardEditor.Models;

[JsonConverter(typeof(ComponentPrimitiveConverter))]
public class ComponentPrimitive<T> : ComponentValue
{
    private T _value;

    public ComponentPrimitive()
    {
        _value = typeof(T) == typeof(string) ? (T)(object)string.Empty : default!;
    }

    public ComponentPrimitive(T value)
    {
        _value = value;
    }

    public T Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }

    public override string? Text => Value?.ToString();
    public override FullObservableCollection<ComponentProperty> Children => new();

    public override async Task Edit(MainWindowViewModel model, bool real)
    {
        var editModel = new EditPrimitiveDialogViewModel<T>
        {
            Value = Value
        };
        var result = await model.ShowEditPrimitiveDialog.Handle(editModel);
        if (!result)
            return;
        Value = editModel.Value;
    }
}

public class ComponentPrimitiveConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        dynamic wrapper = value;
        var token = JToken.FromObject((object)wrapper.Value);
        token.WriteTo(writer);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        var token = JToken.Load(reader);
        return token.Type switch
        {
            JTokenType.Integer => new ComponentPrimitive<int>((int)token),
            JTokenType.Float => new ComponentPrimitive<double>((double)token),
            JTokenType.String => new ComponentPrimitive<string>((string)token!),
            JTokenType.Boolean => new ComponentPrimitive<bool>((bool)token),
            _ => throw new ArgumentException("Cannot create primitive value for token of type " + token.Type)
        };
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.IsGenericTypeDefinition &&
               objectType.GetGenericTypeDefinition() == typeof(ComponentPrimitive<>);
    }
}