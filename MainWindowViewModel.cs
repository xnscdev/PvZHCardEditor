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
        public ICommand LoadCardCommand => new DelegateCommand(DoLoadCard);
        public ICommand EditValueCommand => new DelegateCommand(DoEditValue);
        public ICommand AddValueCommand => new DelegateCommand(DoAddValue);
        public ICommand DeleteValueCommand => new DelegateCommand(DoDeleteValue);
        public ICommand AddComponentCommand => new DelegateCommand(DoAddComponent);
        public ICommand ChangeCostStatsCommand => new DelegateCommand(DoChangeCostStats);
        public ICommand ChangeDescriptionCommand => new DelegateCommand(DoChangeDescription);
        public ICommand ChangeAttributesCommand => new DelegateCommand(DoChangeAttributes);

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

            var cards = Path.Combine(dialog.FileName, "cards.txt");
            var strings = Path.Combine(dialog.FileName, "localizedstrings.txt");
            if (GameDataManager.LoadData(cards, strings))
            {
                LoadedCard = null;
                SelectedComponent = null;
                GameDataManager.ResetUnsavedChanges();
                _actionStack.Reset();
                StatusText = $"Open {dialog.FileName}";
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
            if (GameDataManager.SaveData(cards, strings))
                StatusText = $"Save {_directoryPath}";
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
            {
                _directoryPath = dialog.FileName;
                StatusText = $"Save {dialog.FileName}";
            }
        }

        private void DoUndoCommand(object? parameter)
        {
            StatusText = _actionStack.UndoAction();
        }

        private void DoRedoCommand(object? parameter)
        {
            StatusText = _actionStack.RedoAction();
        }

        private void DoLoadCard(object? parameter)
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

            ActionPerformed();
            return oldValue;
        }

        private void EditValueReverseAction(object parameter, object? data)
        {
            var (_, node) = ((EditValueViewModel, ComponentNode))parameter;
            node.Edit((ComponentValue?)data);
            ActionPerformed();
        }

        #endregion

        #region Add Value Action

        private void DoAddValue(object? parameter)
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

            var action = new EditorAction(AddValueAction, AddValueReverseAction, (dialog.Model, SelectedComponent), "Add Child Value");
            _actionStack.AddAction(action);
        }

        private object? AddValueAction(object parameter)
        {
            var (model, node) = ((AddValueViewModel, ComponentNode))parameter;
            ComponentNode newNode;
            if (model.Type == EditValueType.Component || model.Type == EditValueType.Query)
            {
                var (ns, value) = model.Type == EditValueType.Component ? ("Components", model.ComponentValue) : ("Queries", model.QueryValue);
                var component = ComponentNode.CreateComponent($"{ns}.{value}")!;
                newNode = node.Value!.Add(model, component.FullToken, component.IsolatedObject, value, component.AllowAdd);
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

                newNode = node.Value!.Add(model, component.Token, component, null, true);
            }

            newNode.Parent = node;
            ActionPerformed();
            return newNode;
        }

        private void AddValueReverseAction(object parameter, object? data)
        {
            var node = (ComponentNode)data!;
            node.Parent!.RemoveComponent(node);
            ActionPerformed();
        }

        #endregion

        #region Delete Value Action

        private void DoDeleteValue(object? parameter)
        {
            if (LoadedCard is null || SelectedComponent is null)
                return;

            var action = new EditorAction(DeleteValueAction, DeleteValueReverseAction, SelectedComponent, "Delete Value");
            _actionStack.AddAction(action);
        }

        private object? DeleteValueAction(object parameter)
        {
            var node = (ComponentNode)parameter;
            var parent = node.Parent;
            var index = node.Parent is null ? LoadedCard!.RemoveComponent(node) : node.Parent.RemoveComponent(node);
            ActionPerformed();
            return (index, parent);
        }

        private void DeleteValueReverseAction(object parameter, object? data)
        {
            var (index, parent) = ((int, ComponentNode?))data!;
            var node = (ComponentNode)parameter;
            if (parent is null)
                LoadedCard!.AddComponent(node, index);
            else
                parent.Value!.Add(index, node);
            node.Parent = parent;
            ActionPerformed();
        }

        #endregion

        #region Add Component Action

        private void DoAddComponent(object? parameter)
        {
            if (LoadedCard is null)
                return;

            var dialog = new AddComponentDialog();
            if (dialog.ShowDialog() is not true)
                return;

            var action = new EditorAction(AddComponentAction, AddComponentReverseAction, dialog.Model, "Add Component");
            _actionStack.AddAction(action);
        }

        private object? AddComponentAction(object parameter)
        {
            var model = (AddComponentViewModel)parameter;
            var component = ComponentNode.CreateComponent($"Components.{model.ComponentType}");
            if (component is null)
                throw new ArgumentException(nameof(model.ComponentType));
            var name = component.GetType().Name;
            var node = component.Value is null ? new AutoComponentNode(name, component.Token, component.AllowAdd) : new AutoComponentNode(name, component.Value, component.AllowAdd, component.FullToken);
            LoadedCard!.AddComponent(node);
            ActionPerformed();
            return node;
        }

        private void AddComponentReverseAction(object parameter, object? data)
        {
            var node = (ComponentNode)data!;
            LoadedCard!.RemoveComponent(node);
            ActionPerformed();
        }

        #endregion

        #region Change Cost/Stats Action

        private void DoChangeCostStats(object? parameter)
        {
            if (LoadedCard is null)
                return;

            var dialog = new ChangeCostStatsDialog(LoadedCard.Type == CardType.Fighter);
            if (dialog.ShowDialog() is not true)
                return;

            var action = new EditorAction(ChangeCostStatsAction, ChangeCostStatsReverseAction, dialog.Model, "Change Cost/Stats");
            _actionStack.AddAction(action);
        }

        private object? ChangeCostStatsAction(object parameter)
        {
            var model = (ChangeCostStatsViewModel)parameter;
            var oldValues = (model.IsFighter, LoadedCard!.Cost, LoadedCard.Strength, LoadedCard.Health);
            LoadedCard.Cost = model.Cost;
            if (model.IsFighter)
            {
                LoadedCard.Strength = model.Strength;
                LoadedCard.Health = model.Health;
            }

            ActionPerformed();
            return oldValues;
        }

        private void ChangeCostStatsReverseAction(object parameter, object? data)
        {
            var (isFighter, cost, strength, health) = ((bool, int, int?, int?))data!;
            LoadedCard!.Cost = cost;
            if (isFighter)
            {
                LoadedCard.Strength = strength;
                LoadedCard.Health = health;
            }

            ActionPerformed();
        }

        #endregion

        #region Change Name/Description Action

        private void DoChangeDescription(object? parameter)
        {
            if (LoadedCard is null)
                return;

            var dialog = new ChangeDescriptionDialog(LoadedCard.DisplayName, LoadedCard.ShortText, LoadedCard.LongText, LoadedCard.FlavorText);
            if (dialog.ShowDialog() is not true)
                return;

            var action = new EditorAction(ChangeDescriptionAction, ChangeDescriptionReverseAction, dialog.Model, "Change Name/Description");
            _actionStack.AddAction(action);
        }

        private object? ChangeDescriptionAction(object parameter)
        {
            var model = (ChangeDescriptionViewModel)parameter;
            var oldValues = (LoadedCard!.DisplayName, LoadedCard.ShortText, LoadedCard.LongText, LoadedCard.FlavorText);
            LoadedCard.DisplayName = model.Name;
            LoadedCard.ShortText = model.ShortDescription;
            LoadedCard.LongText = model.LongDescription;
            LoadedCard.FlavorText = model.FlavorText;
            ActionPerformed();
            return oldValues;
        }

        private void ChangeDescriptionReverseAction(object parameter, object? data)
        {
            var (name, shortDescription, longDescription, flavorText) = ((string, string, string, string))data!;
            LoadedCard!.DisplayName = name;
            LoadedCard.ShortText = shortDescription;
            LoadedCard.LongText = longDescription;
            LoadedCard.FlavorText = flavorText;
            ActionPerformed();
        }

        #endregion

        #region Change Attributes Action

        private void DoChangeAttributes(object? parameter)
        {
            if (LoadedCard is null)
                return;

            var dialog = new ChangeAttributesDialog(LoadedCard.Tribes, LoadedCard.Classes, LoadedCard.Set, LoadedCard.Rarity);
            if (dialog.ShowDialog() is not true)
                return;

            var action = new EditorAction(ChangeAttributesAction, ChangeAttributesReverseAction, dialog.Model, "Change Attributes");
            _actionStack.AddAction(action);
        }

        private object? ChangeAttributesAction(object parameter)
        {
            var model = (ChangeAttributesViewModel)parameter;
            var oldValues = (LoadedCard!.Tribes, LoadedCard.Classes, LoadedCard.Set, LoadedCard.Rarity);
            LoadedCard.Tribes = model.SelectedTribes;
            LoadedCard.Classes = model.SelectedClasses;
            LoadedCard.Set = model.Set;
            LoadedCard.Rarity = model.Rarity;
            ActionPerformed();
            return oldValues;
        }

        private void ChangeAttributesReverseAction(object parameter, object? data)
        {
            var (tribes, classes, set, rarity) = ((CardTribe[], CardClass[], CardSet, CardRarity))data!;
            LoadedCard!.Tribes = tribes;
            LoadedCard.Classes = classes;
            LoadedCard.Set = set;
            LoadedCard.Rarity = rarity;
            ActionPerformed();
        }

        #endregion

        private void ActionPerformed()
        {
            LoadedCard?.ActionPerformed();
        }
    }
}
