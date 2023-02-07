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
using System.Windows;

namespace PvZHCardEditor
{
    internal static class GameDataManager
    {
        private static JObject _cardData = null!;
        private static TranslatedString[] _localeData = null!;

        public static void Init()
        {
            ReadCardData();
            ReadLocaleData();
        }

        public static IEnumerable<CardData> FindCards(int? cost, int? strength, int? health, CardType type, CardFaction faction)
        {
            foreach (var item in _cardData)
            {
                var card = item.Value;
                if (card is null)
                    continue;
                if ((string?)card["faction"] != faction.ToString())
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

                switch (type)
                {
                    case CardType.Fighter:
                        {
                            if ((bool?)card["isFighter"] is not true || (bool?)card["isEnv"] is not false)
                                continue;
                            break;
                        }
                    case CardType.Trick:
                        {
                            if ((bool?)card["isFighter"] is not false || (bool?)card["isEnv"] is not false)
                                continue;
                            break;
                        }
                    case CardType.Environment:
                        {
                            if ((bool?)card["isFighter"] is not false || (bool?)card["isEnv"] is not true)
                                continue;
                            break;
                        }
                }

                var prefabName = (string)card["prefabName"]!;
                var displayName = GetTranslatedString($"{prefabName}_name");
                var shortText = GetTranslatedString($"{prefabName}_shortDesc");
                var longText = GetTranslatedString($"{prefabName}_longDesc");
                var flavorText = GetTranslatedString($"{prefabName}_flavorText");
                var tribes = ((JArray)card["subtypes"]!).Select(t => GetCardTribe((string)t!)).ToArray();
                yield return new CardData(prefabName, displayName, shortText, longText, flavorText, 
                    item.Key, cardCost, cardStrength, cardHealth, type, faction, tribes);
            }
        }

        public static CardData LoadCard(string id)
        {
            var card = _cardData[id]!;
            var type = (bool?)card["isFighter"] is true ? CardType.Fighter :
                (bool?)card["isEnv"] is true ? CardType.Environment : CardType.Trick;
            var faction = Enum.Parse<CardFaction>((string)card["faction"]!);
            var cost = (int)card["displaySunCost"]!;
            var strength = type == CardType.Fighter ? (int?)card["displayAttack"] : null;
            var health = type == CardType.Fighter ? (int?)card["displayHealth"] : null;
            var prefabName = (string)card["prefabName"]!;
            var displayName = GetTranslatedString($"{prefabName}_name");
            var shortText = GetTranslatedString($"{prefabName}_shortDesc");
            var longText = GetTranslatedString($"{prefabName}_longDesc");
            var flavorText = GetTranslatedString($"{prefabName}_flavorText");
            var tribes = ((JArray)card["subtypes"]!).Select(t => GetCardTribe((string)t!)).ToArray();
            return new CardData(prefabName, displayName, shortText, longText, flavorText, 
                id, cost, strength, health, type, faction, tribes);
        }

        public static string GetTranslatedString(string key)
        {
            return _localeData.Where(s => s.Key == key).First().Text;
        }

        public static void SetTranslatedString(string key, string value)
        {
            _localeData.Where(s => s.Key == key).First().Text = value;
        }

        public static CardTribe GetCardTribe(string key)
        {
            return Enum.GetValues<CardTribe>().First(tribe => tribe.GetInternalKey() == key);
        }

        private static void ReadCardData()
        { 
            var info = Application.GetResourceStream(new Uri("/Data/cards.txt", UriKind.Relative));
            using var sr = new StreamReader(info.Stream);
            using var reader = new JsonTextReader(sr);
            _cardData = (JObject)JToken.ReadFrom(reader);
        }

        private static void ReadLocaleData()
        {
            var info = Application.GetResourceStream(new Uri("/Data/localizedstrings.txt", UriKind.Relative));
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };
            using var sr = new StreamReader(info.Stream);
            using var reader = new CsvReader(sr, config);
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
