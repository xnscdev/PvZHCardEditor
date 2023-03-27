using System;
using System.Linq;
using ReactiveUI;

namespace PvZHCardEditor.Models;

public static class Extensions
{
    public static T? GetAttribute<T>(this Enum @enum) where T : Attribute
    {
        var type = @enum.GetType();
        var info = type.GetMember(@enum.ToString());
        var attr = info[0].GetCustomAttributes(typeof(T), false);
        return attr.Length > 0 ? (T)attr[0] : null;
    }

    public static string GetInternalKey(this Enum @enum)
    {
        return GetAttribute<InternalKeyAttribute>(@enum)?.Key ?? @enum.ToString();
    }

    public static string GetCardSetKey(this CardSet set)
    {
        return GetAttribute<CardSetDataAttribute>(set)?.SetKey ?? set.ToString();
    }

    public static FullObservableCollection<ComponentProperty> CreateReactiveProperties(this ReactiveObject obj,
        params (string PropertyName, ComponentValue Value)[] properties)
    {
        return new FullObservableCollection<ComponentProperty>(properties.Select(t =>
            obj.CreateReactiveProperty(t.PropertyName, t.Value)));
    }

    public static ComponentProperty CreateReactiveProperty(this ReactiveObject obj, string propertyName,
        ComponentValue value)
    {
        var property = new ComponentProperty(propertyName, value);
        property.PropertyChanged += (_, _) => obj.RaisePropertyChanged(propertyName);
        return property;
    }
}