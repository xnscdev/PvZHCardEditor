using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PvZHCardEditor
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private string? _directoryPath;
        private string _loadId = "";
        private CardData? _loadedCard;
        private ComponentNode? _selectedComponent;

        public ICommand OpenCommand => new DelegateCommand(OpenAction);
        public ICommand SaveCommand => new DelegateCommand(SaveAction);
        public ICommand SaveAsCommand => new DelegateCommand(SaveAsAction);
        public ICommand LoadCardCommand => new DelegateCommand(LoadCard);
        public ICommand ChangeCostStatsCommand => new DelegateCommand(ChangeCostStats);
        public ICommand EditValueCommand => new DelegateCommand(EditValue);
        public ICommand AddValueCommand => new DelegateCommand(AddValue);
        public ICommand DeleteValueCommand => new DelegateCommand(DeleteValue);

        public string LoadId
        {
            get => _loadId;
            set => SetProperty(ref _loadId, value);
        }

        public CardData? LoadedCard
        {
            get => _loadedCard;
            set => SetProperty(ref _loadedCard, value);
        }

        public ComponentNode? SelectedComponent
        {
            get => _selectedComponent;
            set => SetProperty(ref _selectedComponent, value);
        }

        private void OpenAction(object? parameter)
        {
            var dialog = new CommonOpenFileDialog
            {
                Title = "Open Directory",
                IsFolderPicker = true,
                AddToMostRecentlyUsedList = true,
                AllowNonFileSystemItems = false,
                EnsureFileExists = true,
                EnsurePathExists = true,
                EnsureReadOnly = false,
                EnsureValidNames = true,
                Multiselect = false,
                ShowPlacesList = true
            };
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;

            var cards = Path.Combine(dialog.FileName, "cards.json");
            var strings = Path.Combine(dialog.FileName, "localizedstrings.txt");
            if (GameDataManager.LoadData(cards, strings))
            {
                LoadedCard = null;
                SelectedComponent = null;
            }
        }

        private void SaveAction(object? parameter)
        {
            if (_directoryPath is null)
            {
                SaveAsAction(parameter);
                return;
            }

            var cards = Path.Combine(_directoryPath, "cards.txt");
            var strings = Path.Combine(_directoryPath, "localizedstrings.txt");
            GameDataManager.SaveData(cards, strings);
        }

        private void SaveAsAction(object? parameter)
        {
            var dialog = new CommonOpenFileDialog
            {
                Title = "Save To Directory",
                IsFolderPicker = true,
                AddToMostRecentlyUsedList = true,
                AllowNonFileSystemItems = false,
                EnsureFileExists = true,
                EnsurePathExists = true,
                EnsureReadOnly = false,
                EnsureValidNames = true,
                Multiselect = false,
                ShowPlacesList = true
            };
            if (dialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;

            var cards = Path.Combine(dialog.FileName, "cards.txt");
            var strings = Path.Combine(dialog.FileName, "localizedstrings.txt");
            if (GameDataManager.SaveData(cards, strings))
                _directoryPath = dialog.FileName;
        }

        private void LoadCard(object? parameter)
        {
            LoadedCard = GameDataManager.LoadCard(LoadId);
            if (LoadedCard is null)
                MessageBox.Show($"No card exists with ID {LoadId}", "Load Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ChangeCostStats(object? parameter)
        {
            if (LoadedCard is null)
                return;
            LoadedCard.Cost = 69;
        }

        private void EditValue(object? parameter)
        {
            if (LoadedCard is null || SelectedComponent is null)
                return;
            
            var dialog = new EditValueDialog(SelectedComponent.Value?.Token.Type.GetEditValueType());
            if (dialog.ShowDialog() is not true)
                return;

            if (dialog.Model.Type == EditValueType.Component)
            {
                var component = ComponentNode.CreateComponent($"Components.{dialog.Model.ComponentValue}");
                if (component is null)
                    throw new ArgumentException(nameof(dialog.Model.ComponentValue));
                SelectedComponent.Edit(component);
            }
            else
            {
                ComponentValue component = dialog.Model.Type switch
                {
                    EditValueType.Integer => new ComponentInt(new JValue(dialog.Model.IntegerValue)),
                    EditValueType.String => new ComponentString(new JValue(dialog.Model.StringValue)),
                    EditValueType.Boolean => new ComponentBool(new JValue(dialog.Model.BoolValue)),
                    EditValueType.Object => new ComponentObject(new JObject()),
                    EditValueType.Array => new ComponentArray(new JArray()),
                    EditValueType.Null => new ComponentNull(JValue.CreateNull()),
                    _ => throw new NotImplementedException()
                };

                SelectedComponent.Edit(component);
            }

            LoadedCard.UpdateComponentsView();
        }

        private void AddValue(object? parameter)
        {
            if (LoadedCard is null || SelectedComponent is null)
                return;

            IEnumerable<string> existingKeys;
            if (SelectedComponent.Value is ComponentObject obj)
                existingKeys = obj.Children.Select(node => node.Key);
            else
                existingKeys = Enumerable.Empty<string>();

            var dialog = new AddValueDialog(SelectedComponent.Value!.AddValueType, existingKeys);
            if (dialog.ShowDialog() is not true)
                return;

            ComponentNode node;
            var saveSelected = SelectedComponent;
            if (dialog.Model.Type == EditValueType.Component)
            {
                var component = ComponentNode.CreateComponent($"Components.{dialog.Model.ComponentValue}");
                if (component is null)
                    throw new ArgumentException(nameof(dialog.Model.ComponentValue));
                node = SelectedComponent.Value.Add(dialog.Model, component.FullToken, component.IsolatedObject, dialog.Model.ComponentValue, component.AllowAdd);
            }
            else
            {
                ComponentValue component = dialog.Model.Type switch
                {
                    EditValueType.Integer => new ComponentInt(new JValue(dialog.Model.IntegerValue)),
                    EditValueType.String => new ComponentString(new JValue(dialog.Model.StringValue)),
                    EditValueType.Boolean => new ComponentBool(new JValue(dialog.Model.BoolValue)),
                    EditValueType.Object => new ComponentObject(new JObject()),
                    EditValueType.Array => new ComponentArray(new JArray()),
                    EditValueType.Null => new ComponentNull(JValue.CreateNull()),
                    _ => throw new NotImplementedException()
                };

                node = SelectedComponent.Value.Add(dialog.Model, component.Token, component, null, true);
            }

            node.Parent = saveSelected;
            LoadedCard.UpdateComponentsView();
        }

        private void DeleteValue(object? parameter)
        {
            if (LoadedCard is null || SelectedComponent is null)
                return;

            if (SelectedComponent.Parent is null)
                LoadedCard.RemoveComponent(SelectedComponent);
            else
                SelectedComponent.Parent.RemoveComponent(SelectedComponent);

            LoadedCard.UpdateComponentsView();
        }
    }
}
