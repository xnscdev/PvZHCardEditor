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
        private readonly EditorActionStack _actionStack = new();
        private string? _directoryPath;
        private string _loadId = "";
        private string _statusText = "";
        private CardData? _loadedCard;
        private ComponentNode? _selectedComponent;

        public ICommand OpenCommand => new DelegateCommand(DoOpenCommand);
        public ICommand SaveCommand => new DelegateCommand(DoSaveCommand);
        public ICommand SaveAsCommand => new DelegateCommand(DoSaveAsCommand);
        public ICommand UndoCommand => new DelegateCommand(DoUndoCommand);
        public ICommand RedoCommand => new DelegateCommand(DoRedoCommand);
        public ICommand LoadCardCommand => new DelegateCommand(LoadCard);
        public ICommand EditValueCommand => new DelegateCommand(DoEditValue);
        public ICommand AddValueCommand => new DelegateCommand(AddValue);
        public ICommand DeleteValueCommand => new DelegateCommand(DeleteValue);
        public ICommand AddComponentCommand => new DelegateCommand(AddComponent);
        public ICommand ChangeCostStatsCommand => new DelegateCommand(ChangeCostStats);

        public string LoadId
        {
            get => _loadId;
            set => SetProperty(ref _loadId, value);
        }

        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
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

        private void DoOpenCommand(object? parameter)
        {
            if (GameDataManager.PreventReplaceUnsavedChanges())
                DoSaveCommand(parameter);

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
                GameDataManager.ResetUnsavedChanges();
                _actionStack.Reset();
            }
        }

        private void DoSaveCommand(object? parameter)
        {
            if (_directoryPath is null)
            {
                DoSaveAsCommand(parameter);
                return;
            }

            var cards = Path.Combine(_directoryPath, "cards.txt");
            var strings = Path.Combine(_directoryPath, "localizedstrings.txt");
            GameDataManager.SaveData(cards, strings);
        }

        private void DoSaveAsCommand(object? parameter)
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

        private void DoUndoCommand(object? parameter)
        {
            StatusText = _actionStack.UndoAction();
        }

        private void DoRedoCommand(object? parameter)
        {
            StatusText = _actionStack.RedoAction();
        }

        private void LoadCard(object? parameter)
        {
            LoadedCard = GameDataManager.LoadCard(LoadId);
            if (LoadedCard is null)
                MessageBox.Show($"No card exists with ID {LoadId}", "Load Failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #region Edit Value Action

        private void DoEditValue(object? parameter)
        {
            if (LoadedCard is null || SelectedComponent is null)
                return;

            var dialog = new EditValueDialog(SelectedComponent.Value?.Token.Type.GetEditValueType());
            if (dialog.ShowDialog() is not true)
                return;

            var action = new EditorAction(EditValueAction, EditValueReverseAction, (dialog.Model, SelectedComponent), "Edit Value");
            _actionStack.AddAction(action);
        }

        private object? EditValueAction(object parameter)
        {
            var (model, node) = ((EditValueViewModel, ComponentNode))parameter;
            ComponentValue? oldValue;
            if (model.Type == EditValueType.Component || model.Type == EditValueType.Query)
            {
                var (ns, value) = model.Type == EditValueType.Component ? ("Components", model.ComponentValue) : ("Queries", model.QueryValue);
                var component = ComponentNode.CreateComponent($"{ns}.{value}")!;
                oldValue = node.Edit(component);
            }
            else
            {
                ComponentValue component = model.Type switch
                {
                    EditValueType.Integer => new ComponentInt(new JValue(model.IntegerValue)),
                    EditValueType.String => new ComponentString(new JValue(model.StringValue)),
                    EditValueType.Boolean => new ComponentBool(new JValue(model.BoolValue)),
                    EditValueType.Object => new ComponentObject(new JObject()),
                    EditValueType.Array => new ComponentArray(new JArray()),
                    EditValueType.Null => new ComponentNull(JValue.CreateNull()),
                    _ => throw new NotImplementedException()
                };

                oldValue = node.Edit(component);
            }

            LoadedCard!.UpdateComponentsView();
            return oldValue;
        }

        private void EditValueReverseAction(object parameter, object? data)
        {
            var (_, node) = ((EditValueViewModel, ComponentNode))parameter;
            node.Edit((ComponentValue?)data);
        }

        #endregion

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
            if (dialog.Model.Type == EditValueType.Component || dialog.Model.Type == EditValueType.Query)
            {
                var (ns, value) = dialog.Model.Type == EditValueType.Component ? ("Components", dialog.Model.ComponentValue) : ("Queries", dialog.Model.QueryValue);
                var component = ComponentNode.CreateComponent($"{ns}.{value}")!;
                node = SelectedComponent.Value.Add(dialog.Model, component.FullToken, component.IsolatedObject, value, component.AllowAdd);
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

        private void AddComponent(object? parameter)
        {
            if (LoadedCard is null)
                return;

            var dialog = new AddComponentDialog();
            if (dialog.ShowDialog() is not true)
                return;

            var component = ComponentNode.CreateComponent($"Components.{dialog.Model.ComponentType}");
            if (component is null)
                throw new ArgumentException(nameof(dialog.Model.ComponentType));
            var name = component.GetType().Name;
            var node = component.Value is null ? new AutoComponentNode(name, component.Token, component.AllowAdd) : new AutoComponentNode(name, component.Value, component.AllowAdd, component.FullToken);
            LoadedCard.AddComponent(node);
            LoadedCard.UpdateComponentsView();
        }

        private void ChangeCostStats(object? parameter)
        {
            if (LoadedCard is null)
                return;

            LoadedCard.Cost = 69;
        }
    }
}
