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

    public override async Task Edit(MainWindowViewModel model)
    {
        if (Value.EditHandler != null)
        {
            await Value.EditHandler.Edit(model);
            return;
        }

        EditComponentDialogViewModel editModel = Value.IsQuery
            ? new EditComponentDialogViewModel<EntityQuery>()
            : new EditComponentDialogViewModel<EntityComponent>();
        var result = await model.ShowEditComponentDialog.Handle(editModel);
        if (!result)
            return;
        var type = EntityComponentBase.ParseDisplayTypeString(editModel.ComponentValue, !Value.IsQuery);
        if (type == null)
            return;
        Value = (T)Activator.CreateInstance(type)!;
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