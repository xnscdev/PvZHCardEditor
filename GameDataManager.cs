using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

namespace PvZHCardEditor
{
    internal static class GameDataManager
    {
        private static readonly CsvConfiguration _csvConfig = new(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false
        };

        private static JObject _cardData = null!;
        private static TranslatedString[] _localeData = null!;
        private static bool _unsavedChanges;

        public static IEnumerable<string> ComponentTypes => typeof(GameDataManager).Assembly.GetTypes()
            .Where(t => t.Namespace == "PvZHCardEditor.Components" && !t.GetTypeInfo().IsDefined(typeof(CompilerGeneratedAttribute), true)).Select(t => t.Name);
        public static IEnumerable<string> QueryTypes => typeof(GameDataManager).Assembly.GetTypes()
            .Where(t => t.Namespace == "PvZHCardEditor.Queries" && !t.GetTypeInfo().IsDefined(typeof(CompilerGeneratedAttribute), true)).Select(t => t.Name);

        public static void Init()
        {
            var cards = Application.GetResourceStream(new Uri("/Data/cards.txt", UriKind.Relative));
            var strings = Application.GetResourceStream(new Uri("/Data/localizedstrings.txt", UriKind.Relative));
            ReadCardData(cards.Stream);
            ReadLocaleData(strings.Stream);
        }

        public static bool LoadData(string cards, string strings)
        {
            try
            {
                using var cardsStream = File.OpenRead(cards);
                using var stringsStream = File.OpenRead(strings);
                ReadCardData(cardsStream);
                ReadLocaleData(stringsStream);
            }
            catch
            {
                MessageBox.Show("Failed to read card data from this directory.", "Load Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public static bool SaveData(string cards, string strings)
        {
            try
            {
                File.WriteAllText(cards, JsonConvert.SerializeObject(_cardData));
                using var sw = new StreamWriter(strings);
                using var writer = new CsvWriter(sw, _csvConfig);
                writer.WriteRecords(_localeData);
                ResetUnsavedChanges();
            }
            catch
            {
                MessageBox.Show("Failed to write card data to this directory.", "Save Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public static void MarkUnsavedChanges()
        {
            _unsavedChanges = true;
        }

        public static void ResetUnsavedChanges()
        {
            _unsavedChanges = false;
        }

        public static bool PreventCloseUnsavedChanges()
        {
            return _unsavedChanges && MessageBox.Show("There are unsaved changes. Close anyway?", "Unsaved Changes", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No;
        }

        public static bool PreventReplaceUnsavedChanges()
        {
            return _unsavedChanges && MessageBox.Show("There are unsaved changes. Save before opening?", "Unsaved Changes", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }

        public static IEnumerable<CardData> FindCards(string name, int? cost, int? strength, int? health, CardType type, CardFaction faction)
        {
            foreach (var item in _cardData)
            {
                var card = item.Value;
                if (card is null)
                    continue;
                var cardFaction = (string?)card["faction"];
                if (cardFaction != faction.ToString() && cardFaction != CardFaction.All.ToString())
                    continue;

                var cardCost = (int)card["displaySunCost"]!;
                if (cost is not null && cardCost != cost)
                    continue;

                var cardStrength = type == CardType.Fighter ? (int?)card["displayAttack"] : null;
                if (strength is not null && cardStrength is not null && cardStrength != strength)
                    continue;

                var cardHealth = type == CardType.Fighter ? (int?)card["displayHealth"] : null;
                if (health is not null && cardHealth is not null && cardHealth != health)
                    continue;

                var prefabName = (string)card["prefabName"]!;
                if (!GetTranslatedString($"{prefabName}_name").ToLower().Contains(name.ToLower()))
                    continue;

                if (CardData.ParseType(card) != type)
                    continue;

                yield return new CardData(item.Key, card);
            }
        }

        public static CardData? LoadCard(string id)
        {
            var card = _cardData[id];
            if (card is null)
                return null;
            return new CardData(id, card);
        }

        public static string GetTranslatedString(string key, string fallback = "")
        {
            return _localeData.Where(s => s.Key == key).FirstOrDefault()?.Text ?? fallback;
        }

        public static void SetTranslatedString(string key, string value)
        {
            _localeData.Where(s => s.Key == key).First().Text = value;
        }

        public static T GetEnumInternalKey<T>(string key) where T : struct, Enum
        {
            return Enum.GetValues<T>().First(x => x.GetInternalKey() == key);
        }

        private static void ReadCardData(Stream stream)
        { 
            using var sr = new StreamReader(stream);
            using var reader = new JsonTextReader(sr);
            _cardData = (JObject)JToken.ReadFrom(reader);
        }

        private static void ReadLocaleData(Stream stream)
        {
            using var sr = new StreamReader(stream);
            using var reader = new CsvReader(sr, _csvConfig);
            _localeData = reader.GetRecords<TranslatedString>().ToArray();
        }

        private class TranslatedString
        {
            [Index(0)]
            public string Key { get; set; } = null!;

            [Index(1)]
            public string Text { get; set; } = null!;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    internal class InternalKeyAttribute : Attribute
    {
        public string Key { get; }

        public InternalKeyAttribute(string key)
        {
            Key = key;
        }
    }
}
