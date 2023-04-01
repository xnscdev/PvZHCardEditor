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

[JsonConverter(typeof(ComponentListConverter))]
public class ComponentList<T> : ComponentValue where T : ComponentValue, new()
{
    public ComponentList() : this(new FullObservableCollection<T>())
    {
    }

    public ComponentList(FullObservableCollection<T> elements)
    {
        Elements = elements;
        Properties = MakeProperties();
        Elements.CollectionChanged += OnCollectionChanged;
    }

    public FullObservableCollection<T> Elements { get; private set; }
    private FullObservableCollection<ComponentProperty> Properties { get; set; }

    public override string? Text => null;
    public override FullObservableCollection<ComponentProperty> Children => Properties;

    public override async Task<bool> Edit(MainWindowViewModel model, bool real)
    {
        var editModel = new EditListDialogViewModel<T>
        {
            Elements = new AvaloniaList<T>(Elements)
        };
        var result = await model.ShowEditListDialog.Handle(editModel);
        if (!result)
            return false;
        SetElements(new FullObservableCollection<T>(editModel.Elements));
        return true;
    }

    public void SetElements(FullObservableCollection<T> elements)
    {
        Elements = elements;
        Elements.CollectionChanged += OnCollectionChanged;
        Properties = MakeProperties();
        this.RaisePropertyChanged(nameof(Children));
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
    {
        this.RaisePropertyChanged(nameof(Children));
    }

    private FullObservableCollection<ComponentProperty> MakeProperties()
    {
        return new FullObservableCollection<ComponentProperty>(Elements.Select((e, i) =>
            new ComponentProperty($"[{i}]", e)));
    }
}

public class ComponentListConverter : JsonConverter
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
        var array = new JArray(children.Select(p => JToken.FromObject(p.Value)).Cast<object>().ToArray());
        array.WriteTo(writer);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        var array = JArray.Load(reader);
        var elementType = objectType.GetGenericArguments()[0];
        return GetType().GetMethod(nameof(MakeList))?.MakeGenericMethod(elementType)
            .Invoke(null, new object[] { array });
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.IsGenericTypeDefinition &&
               objectType.GetGenericTypeDefinition() == typeof(ComponentList<>);
    }

    public static ComponentList<T> MakeList<T>(JArray array) where T : ComponentValue, new()
    {
        return new ComponentList<T>(new FullObservableCollection<T>(array.Select(token => token.ToObject<T>()!)));
    }
}