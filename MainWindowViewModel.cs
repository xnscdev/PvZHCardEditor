﻿using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
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
            SelectedComponent.Edit(new ComponentInt(new JValue(69)));
            LoadedCard.UpdateComponentsView();
        }
    }
}
