using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PvZHCardEditor.ViewModels;
using ReactiveUI;

namespace PvZHCardEditor.Models;

[JsonConverter(typeof(ComponentListConverter))]
public class ComponentList<T> : ComponentValue where T : ComponentValue
{
    public ComponentList(FullObservableCollection<T> elements)
    {
        Elements = elements;
        Properties = MakeProperties();
        Elements.CollectionChanged += OnCollectionChanged;
    }

    public FullObservableCollection<T> Elements { get; }
    private FullObservableCollection<ComponentProperty> Properties { get; set; }

    public override string? Text => null;

    public override FullObservableCollection<ComponentProperty> Children => Properties;

    public override Task Edit(MainWindowViewModel model)
    {
        Console.WriteLine("Implement GUI to allow reordering, inserting, removing elements");
        Properties = MakeProperties();
        this.RaisePropertyChanged(nameof(Children));
        throw new NotImplementedException();
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

    public static ComponentList<T> MakeList<T>(JArray array) where T : ComponentValue
    {
        return new ComponentList<T>(new FullObservableCollection<T>(array.Select(token => token.ToObject<T>()!)));
    }
}