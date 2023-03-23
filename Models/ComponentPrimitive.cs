using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactiveUI;

namespace PvZHCardEditor.Models;

public class ComponentPrimitive<T> : ComponentRenderable
{
    private T _value;

    public T Value
    {
        get => _value;
        set => this.RaiseAndSetIfChanged(ref _value, value);
    }

    public override string? Text => Value?.ToString();
    public override FullObservableCollection<ComponentRenderable> Children => new();

    public ComponentPrimitive(T value)
    {
        _value = value;
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
        
        var wrapper = (ComponentPrimitive<object>)value;
        var token = JToken.FromObject(wrapper.Value);
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
            _ => throw new NotImplementedException()
        };
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(ComponentPrimitive<>);
    }
}