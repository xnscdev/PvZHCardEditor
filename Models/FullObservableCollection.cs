using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace PvZHCardEditor.Models;

public sealed class FullObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
{
    public FullObservableCollection()
    {
    }

    public FullObservableCollection(IEnumerable<T> collection) : base(collection)
    {
    }

    public event EventHandler<string?>? ChildChanged;

    protected override void InsertItem(int index, T item)
    {
        base.InsertItem(index, item);
        item.PropertyChanged += OnItemPropertyChanged;
    }

    protected override void RemoveItem(int index)
    {
        var item = Items[index];
        item.PropertyChanged -= OnItemPropertyChanged;
        base.RemoveItem(index);
    }

    private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        ChildChanged?.Invoke(this, e.PropertyName);
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}