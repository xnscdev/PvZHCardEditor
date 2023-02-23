using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json.Linq;
using PvZHCardEditor.Components;
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
        public ICommand AddEntityCommand => new DelegateCommand(DoAddEntity);
        public ICommand DuplicateEntityCommand => new DelegateCommand(DoDuplicateEntity);
        public ICommand ChangeCostStatsCommand => new DelegateCommand(DoChangeCostStats);
        public ICommand ChangeDescriptionCommand => new DelegateCommand(DoChangeDescription);
        public ICommand ChangeTribesCommand => new DelegateCommand(DoChangeTribes);
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

        private object? EditValueAction(object? parameter)
        {
            var (model, node) = ((EditValueViewModel, ComponentNode))parameter!;
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

        private void EditValueReverseAction(object? parameter, object? data)
        {
            var (_, node) = ((EditValueViewModel, ComponentNode))parameter!;
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

        private object? AddValueAction(object? parameter)
        {
            var (model, node) = ((AddValueViewModel, ComponentNode))parameter!;
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

        private void AddValueReverseAction(object? parameter, object? data)
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

        private object? DeleteValueAction(object? parameter)
        {
            var node = (ComponentNode)parameter!;
            var parent = node.Parent;
            var index = node.Parent is null ? LoadedCard!.RemoveComponent(node) : node.Parent.RemoveComponent(node);
            ActionPerformed();
            return (index, parent);
        }

        private void DeleteValueReverseAction(object? parameter, object? data)
        {
            var (index, parent) = ((int, ComponentNode?))data!;
            var node = (ComponentNode)parameter!;
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

        private object? AddComponentAction(object? parameter)
        {
            var model = (AddComponentViewModel)parameter!;
            var component = ComponentNode.CreateComponent($"Components.{model.ComponentType}");
            if (component is null)
                throw new ArgumentException(nameof(model.ComponentType));
            var name = component.GetType().Name;
            var node = component.Value is null ? new AutoComponentNode(name, component.Token, component.AllowAdd, component.FullToken) : new AutoComponentNode(name, component.Value, component.AllowAdd, component.FullToken);
            LoadedCard!.AddComponent(node);
            ActionPerformed();
            return node;
        }

        private void AddComponentReverseAction(object? parameter, object? data)
        {
            var node = (ComponentNode)data!;
            LoadedCard!.RemoveComponent(node);
            ActionPerformed();
        }

        #endregion

        #region Add Effect Entity Action

        private void DoAddEntity(object? parameter)
        {
            if (LoadedCard is null)
                return;

            var action = new EditorAction(AddEntityAction, AddEntityReverseAction, null, "Add Effect Entity");
            _actionStack.AddAction(action);
        }

        private object? AddEntityAction(object? parameter)
        {
            var node = LoadedCard!.FindOrInsertComponent(typeof(EffectEntitiesDescriptor));
            var obj = new JObject
            {
                ["components"] = new JArray()
            };
            var entity = new ComponentArray(obj["components"]!);
            var array = (ComponentArray)node.Value!;
            var result = array.Add(null, obj, entity, null, true);
            result.Parent = node;
            ActionPerformed();
            return entity;
        }

        private void AddEntityReverseAction(object? parameter, object? data)
        {
            var node = LoadedCard!.FindOrInsertComponent(typeof(EffectEntitiesDescriptor));
            var entity = (ComponentArray)data!;
            var array = (ComponentArray)node.Value!;
            foreach (var n in array.Children)
            {
                if (ReferenceEquals(n.Value, entity))
                {
                    entity.Token.Parent?.Parent?.Remove();
                    array.Remove(n);
                    break;
                }
            }
            ActionPerformed();
        }

        #endregion

        #region Duplicate Effect Entity Action

        private void DoDuplicateEntity(object? parameter)
        {
            if (LoadedCard is null || SelectedComponent is null)
                return;

            var action = new EditorAction(DuplicateEntityAction, DuplicateEntityReverseAction, SelectedComponent, "Duplicate Effect Entity");
            _actionStack.AddAction(action);
        }

        private object? DuplicateEntityAction(object? parameter)
        {
            var parent = LoadedCard!.FindOrInsertComponent(typeof(EffectEntitiesDescriptor));
            var node = (ComponentNode)parameter!;
            var obj = node.Token.Parent!.Parent!.DeepClone();
            var components = (JArray)obj["components"]!;
            var entity = new ComponentArray(components, components.Select(c => ComponentNode.ParseComponent(c)).Where(v => v is not null).Select(v => v!));
            var array = (ComponentArray)parent.Value!;
            var result = array.Add(null, obj, entity, null, true);
            result.Parent = parent;
            ActionPerformed();
            return entity;
        }

        private void DuplicateEntityReverseAction(object? parameter, object? data)
        {
            var node = LoadedCard!.FindOrInsertComponent(typeof(EffectEntitiesDescriptor));
            var entity = (ComponentArray)data!;
            var array = (ComponentArray)node.Value!;
            foreach (var n in array.Children)
            {
                if (ReferenceEquals(n.Value, entity))
                {
                    entity.Token.Parent?.Parent?.Remove();
                    array.Remove(n);
                    break;
                }
            }
            ActionPerformed();
        }

        #endregion

        #region Change Cost/Stats Action

        private void DoChangeCostStats(object? parameter)
        {
            if (LoadedCard is null)
                return;

            var dialog = new ChangeCostStatsDialog(LoadedCard.Cost, LoadedCard.Type == CardType.Fighter, LoadedCard.Strength, LoadedCard.Health);
            if (dialog.ShowDialog() is not true)
                return;

            var action = new EditorAction(ChangeCostStatsAction, ChangeCostStatsReverseAction, dialog.Model, "Change Cost/Stats");
            _actionStack.AddAction(action);
        }

        private object? ChangeCostStatsAction(object? parameter)
        {
            var model = (ChangeCostStatsViewModel)parameter!;
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

        private void ChangeCostStatsReverseAction(object? parameter, object? data)
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

            var dialog = new ChangeDescriptionDialog(LoadedCard.DisplayName, LoadedCard.ShortText, LoadedCard.LongText, LoadedCard.FlavorText, LoadedCard.TargetingText, LoadedCard.HeraldFighterText, LoadedCard.HeraldTrickText);
            if (dialog.ShowDialog() is not true)
                return;

            var action = new EditorAction(ChangeDescriptionAction, ChangeDescriptionReverseAction, dialog.Model, "Change Name/Description");
            _actionStack.AddAction(action);
        }

        private object? ChangeDescriptionAction(object? parameter)
        {
            var model = (ChangeDescriptionViewModel)parameter!;
            var oldValues = (LoadedCard!.DisplayName, LoadedCard.ShortText, LoadedCard.LongText, LoadedCard.FlavorText, LoadedCard.TargetingText, LoadedCard.HeraldFighterText, LoadedCard.HeraldTrickText);
            LoadedCard.DisplayName = model.Name;
            LoadedCard.ShortText = model.ShortDescription;
            LoadedCard.LongText = model.LongDescription;
            LoadedCard.FlavorText = model.FlavorText;
            LoadedCard.TargetingText = model.Targeting.Length > 0 ? model.Targeting : null;
            LoadedCard.HeraldFighterText = model.HeraldFighter.Length > 0 ? model.HeraldFighter : null;
            LoadedCard.HeraldTrickText = model.HeraldTrick.Length > 0 ? model.HeraldTrick : null;
            ActionPerformed();
            return oldValues;
        }

        private void ChangeDescriptionReverseAction(object? parameter, object? data)
        {
            var (name, shortDescription, longDescription, flavorText, targeting, heraldFighter, heraldTrick) = ((string, string, string, string, string?, string?, string?))data!;
            LoadedCard!.DisplayName = name;
            LoadedCard.ShortText = shortDescription;
            LoadedCard.LongText = longDescription;
            LoadedCard.FlavorText = flavorText;
            LoadedCard.TargetingText = targeting;
            LoadedCard.HeraldFighterText = heraldFighter;
            LoadedCard.HeraldTrickText = heraldTrick;
            ActionPerformed();
        }

        #endregion

        #region Change Class/Tribes Action

        private void DoChangeTribes(object? parameter)
        {
            if (LoadedCard is null)
                return;

            var dialog = new ChangeTribesDialog(LoadedCard.Tribes, LoadedCard.Classes);
            if (dialog.ShowDialog() is not true)
                return;

            var action = new EditorAction(ChangeTribesAction, ChangeTribesReverseAction, dialog.Model, "Change Class/Tribes");
            _actionStack.AddAction(action);
        }

        private object? ChangeTribesAction(object? parameter)
        {
            var model = (ChangeTribesViewModel)parameter!;
            var oldValues = (LoadedCard!.Tribes, LoadedCard.Classes);
            LoadedCard.Tribes = model.SelectedTribes;
            LoadedCard.Classes = model.SelectedClasses;
            ActionPerformed();
            return oldValues;
        }

        private void ChangeTribesReverseAction(object? parameter, object? data)
        {
            var (tribes, classes) = ((CardTribe[], CardClass[]))data!;
            LoadedCard!.Tribes = tribes;
            LoadedCard.Classes = classes;
            ActionPerformed();
        }

        #endregion

        #region Change Attributes Action

        private void DoChangeAttributes(object? parameter)
        {
            if (LoadedCard is null)
                return;

            var data = LoadedCard.GetExtraAttributes();
            var dialog = new ChangeAttributesDialog(data);
            if (dialog.ShowDialog() is not true)
                return;

            var action = new EditorAction(ChangeAttributesAction, ChangeAttributesReverseAction, dialog.Model, "Change Attributes");
            _actionStack.AddAction(action);
        }

        private object? ChangeAttributesAction(object? parameter)
        {
            var model = (ChangeAttributesViewModel)parameter!;
            var oldValues = LoadedCard!.GetExtraAttributes();
            LoadedCard.SetExtraAttributes(model.GetExtraAttributes());
            ActionPerformed();
            return oldValues;
        }

        private void ChangeAttributesReverseAction(object? parameter, object? data)
        {
            LoadedCard!.SetExtraAttributes((CardExtraAttributes)data!);
            ActionPerformed();
        }

        #endregion

        private void ActionPerformed()
        {
            LoadedCard?.ActionPerformed();
        }
    }
}
