using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PvZHCardEditor.ViewModels;
using ReactiveUI;

namespace PvZHCardEditor.Models;

[JsonConverter(typeof(OptionalComponentWrapperConverter))]
public class OptionalComponentWrapper<T> : ComponentValue where T : EntityComponentBase
{
    private bool _isExpanded;
    private T? _value;

    public OptionalComponentWrapper() : this(null)
    {
    }

    public OptionalComponentWrapper(T? value)
    {
        Value = value;
    }

    public override string Text => Value?.GetDisplayTypeString() ?? "(null)";

    public override FullObservableCollection<ComponentProperty> Children =>
        Value?.Children ?? new FullObservableCollection<ComponentProperty>();

    public T? Value
    {
        get => _value;
        set
        {
            this.RaiseAndSetIfChanged(ref _value, value);
            value?.WhenAnyValue(x => x.Children).Subscribe(_ => this.RaisePropertyChanged(nameof(Children)));
        }
    }

    public override bool IsExpanded
    {
        get => Value?.EditHandler?.IsExpanded ?? _isExpanded;
        set
        {
            if (Value?.EditHandler != null)
                Value.EditHandler.IsExpanded = value;
            else
                _isExpanded = value;
        }
    }

    public override async Task Edit(MainWindowViewModel model)
    {
        if (Value?.EditHandler != null)
        {
            await Value.EditHandler.Edit(model);
            return;
        }

        var editModel = new EditOptionalComponentDialogViewModel<T>();
        var result = await model.ShowEditOptionalComponentDialog.Handle(editModel);
        if (!result)
            return;

        if (editModel.IsNull)
        {
            Value = null;
        }
        else
        {
            var type = EntityComponentBase.ParseDisplayTypeString(editModel.ComponentValue,
                typeof(T) == typeof(EntityComponent));
            if (type == null)
                return;
            Value = (T)Activator.CreateInstance(type)!;
        }
    }
}

public class OptionalComponentWrapperConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        dynamic wrapper = value;
        EntityComponentBase? component = wrapper.Value;
        if (component == null)
        {
            writer.WriteNull();
            return;
        }

        var token = serializer.DefaultFromObject(component);
        var obj = new JObject
        {
            ["$type"] = component.GetFullTypeString(),
            ["$data"] = token
        };
        obj.WriteTo(writer);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        var elementType = objectType.GetGenericArguments()[0];
        var token = JToken.Load(reader);
        if (token.Type == JTokenType.Null)
            return GetType().GetMethod(nameof(MakeComponent))?.MakeGenericMethod(elementType)
                .Invoke(null, new object?[] { null });
        var obj = JObject.Load(reader);
        var type = EntityComponentBase.ParseFullTypeString((string)obj["$type"]!);
        if (type == null)
            return null;
        var component = obj["$data"]!.DefaultToObject(type, serializer);
        return GetType().GetMethod(nameof(MakeComponent))?.MakeGenericMethod(elementType)
            .Invoke(null, new[] { component });
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.IsGenericTypeDefinition &&
               objectType.GetGenericTypeDefinition() == typeof(OptionalComponentWrapper<>);
    }

    public static OptionalComponentWrapper<T> MakeComponent<T>(T? component) where T : EntityComponentBase
    {
        return new OptionalComponentWrapper<T>(component);
    }
}