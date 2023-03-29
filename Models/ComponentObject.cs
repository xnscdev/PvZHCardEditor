using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PvZHCardEditor.ViewModels;
using ReactiveUI;

namespace PvZHCardEditor.Models;

[JsonConverter(typeof(ComponentObjectConverter))]
public class ComponentObject<T> : ComponentValue where T : ComponentValue, new()
{
    public ComponentObject() : this(new FullObservableCollection<ComponentProperty>())
    {
    }

    public ComponentObject(FullObservableCollection<ComponentProperty> properties)
    {
        Properties = properties;
        Properties.CollectionChanged += OnCollectionChanged;
    }

    public FullObservableCollection<ComponentProperty> Properties { get; private set; }

    public override string? Text => null;
    public override FullObservableCollection<ComponentProperty> Children => Properties;

    public override async Task Edit(MainWindowViewModel model, bool real)
    {
        var editModel = new EditObjectDialogViewModel<T>
        {
            Properties = new AvaloniaList<ComponentProperty>(Properties)
        };
        var result = await model.ShowEditObjectDialog.Handle(editModel);
        if (!result)
            return;
        Properties = new FullObservableCollection<ComponentProperty>(editModel.Properties);
        Properties.CollectionChanged += OnCollectionChanged;
        this.RaisePropertyChanged(nameof(Children));
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        this.RaisePropertyChanged(nameof(Children));
    }
}

public class ComponentObjectConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }

        dynamic wrapper = value;
        FullObservableCollection<ComponentProperty> children = wrapper.Children;
        var obj = new JObject(children.Select(p => new JProperty(p.Key, JToken.FromObject(p.Value))).Cast<object>()
            .ToArray());
        obj.WriteTo(writer);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        var obj = JObject.Load(reader);
        var elementType = objectType.GetGenericArguments()[0];
        return GetType().GetMethod(nameof(MakeObject))?.MakeGenericMethod(elementType)
            .Invoke(null, new object[] { obj });
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.IsGenericTypeDefinition &&
               objectType.GetGenericTypeDefinition() == typeof(ComponentObject<>);
    }

    public static ComponentObject<T> MakeObject<T>(JObject obj) where T : ComponentValue, new()
    {
        return new ComponentObject<T>(new FullObservableCollection<ComponentProperty>(obj.Properties()
            .Select(p => new ComponentProperty(p.Name, p.Value.ToObject<T>()!))));
    }
}