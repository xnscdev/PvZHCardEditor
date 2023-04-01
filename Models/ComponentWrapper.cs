using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PvZHCardEditor.ViewModels;
using ReactiveUI;

namespace PvZHCardEditor.Models;

[JsonConverter(typeof(ComponentWrapperConverter))]
public class ComponentWrapper<T> : ComponentValue where T : EntityComponentBase
{
    private bool _isExpanded;
    private T _value = null!;

    public ComponentWrapper() : this((T)Activator.CreateInstance(GameDataManager.GetComponentTypes<T>().First())!)
    {
    }

    public ComponentWrapper(T value)
    {
        Value = value;
    }

    public override string Text => Value.GetDisplayTypeString();
    public override FullObservableCollection<ComponentProperty> Children => Value.Children;

    public T Value
    {
        get => _value;
        set
        {
            this.RaiseAndSetIfChanged(ref _value, value);
            value.WhenAnyValue(x => x.Children).Subscribe(_ => this.RaisePropertyChanged(nameof(Children)));
        }
    }

    public override bool IsExpanded
    {
        get => Value.EditHandler?.IsExpanded ?? _isExpanded;
        set
        {
            if (Value.EditHandler != null)
                Value.EditHandler.IsExpanded = value;
            else
                _isExpanded = value;
        }
    }

    public override async Task<bool> Edit(MainWindowViewModel model, bool real)
    {
        if (!real && Value.EditHandler != null)
            return await Value.EditHandler.Edit(model, real);
        var editModel = new EditComponentDialogViewModel<T>();
        var result = await model.ShowEditComponentDialog.Handle(editModel);
        if (!result)
            return false;
        var type = EntityComponentBase.ParseDisplayTypeString(editModel.ComponentValue,
            typeof(T) == typeof(EntityComponent));
        if (type == null)
            return false;
        Value = (T)Activator.CreateInstance(type)!;
        return true;
    }
}

public class ComponentWrapperConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        dynamic wrapper = value;
        EntityComponentBase component = wrapper.Value;
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
        var obj = JObject.Load(reader);
        var type = EntityComponentBase.ParseFullTypeString((string)obj["$type"]!);
        if (type == null)
            return null;
        var component = obj["$data"]!.DefaultToObject(type, serializer);
        var elementType = objectType.GetGenericArguments()[0];
        return GetType().GetMethod(nameof(MakeComponent))?.MakeGenericMethod(elementType)
            .Invoke(null, new[] { component });
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.IsGenericTypeDefinition &&
               objectType.GetGenericTypeDefinition() == typeof(ComponentWrapper<>);
    }

    public static ComponentWrapper<T> MakeComponent<T>(T component) where T : EntityComponentBase
    {
        return new ComponentWrapper<T>(component);
    }
}