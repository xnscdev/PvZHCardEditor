using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ReactiveUI;

namespace PvZHCardEditor.Models;

public static class GameDataManager
{
    private const string CardsFile = "cards.txt";
    private const string StringsFile = "localizedstrings.txt";

    private static readonly CsvConfiguration CsvConfig = new(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = false
    };

    public static JObject CardsData { get; set; } = null!;
    public static ObservableCollection<LocalizedString> StringsData { get; set; } = null!;

    public static bool Modified { get; set; }

    public static CardData? LoadCard(string id)
    {
        var card = CardsData[id];
        return card == null ? null : new CardData(id, card);
    }

    public static bool CardExists(string id)
    {
        return CardsData.ContainsKey(id);
    }

    public static void AddCard(string id, JToken token)
    {
        CardsData[id] = token;
    }

    public static bool DeleteCard(string id)
    {
        return CardsData.Remove(id);
    }

    public static string? TryGetLocalizedString(string key)
    {
        return StringsData.Where(s => s.Key == key).Select(s => s.Text).DefaultIfEmpty(null).First();
    }

    public static string GetLocalizedString(string key)
    {
        return StringsData.FirstOrDefault(s => s.Key == key)?.Text ?? string.Empty;
    }

    public static void SetLocalizedString(string key, string? value)
    {
        var s = StringsData.FirstOrDefault(s => s.Key == key);
        if (s != null)
        {
            if (value == null)
                StringsData.Remove(s);
            else
                s.Text = value;
        }
        else if (value != null)
        {
            StringsData.Add(new LocalizedString
            {
                Key = key,
                Text = value
            });
        }
    }

    public static T GetEnumInternalKey<T>(string key) where T : struct, Enum
    {
        return Enum.GetValues<T>().First(x => x.GetInternalKey() == key);
    }

    public static IEnumerable<Type> GetComponentTypes<T>()
    {
        return typeof(T).Assembly.GetTypes().Where(t =>
            t.IsSubclassOf(typeof(T)) && Attribute.GetCustomAttribute(t, typeof(CompilerGeneratedAttribute)) == null);
    }

    public static bool OpenData(string dir)
    {
        try
        {
            using var cardsStream = File.OpenRead(Path.Combine(dir, CardsFile));
            using var cardsStreamReader = new StreamReader(cardsStream);
            using var cardsReader = new JsonTextReader(cardsStreamReader);
            CardsData = (JObject)JToken.ReadFrom(cardsReader);

            using var stringsStream = File.OpenRead(Path.Combine(dir, StringsFile));
            using var stringsStreamReader = new StreamReader(stringsStream);
            using var stringsReader = new CsvReader(stringsStreamReader, CsvConfig);
            StringsData = new ObservableCollection<LocalizedString>(stringsReader.GetRecords<LocalizedString>());
        }
        catch
        {
            return false;
        }

        return true;
    }

    public static bool SaveData(string dir)
    {
        try
        {
            File.WriteAllText(Path.Combine(dir, CardsFile), JsonConvert.SerializeObject(CardsData));

            using var sw = new StreamWriter(Path.Combine(dir, StringsFile));
            using var writer = new CsvWriter(sw, CsvConfig);
            writer.WriteRecords(StringsData);
            Modified = false;
        }
        catch
        {
            return false;
        }

        return true;
    }
}

public class LocalizedString : ReactiveObject
{
    private string _key = null!;
    private string _text = null!;

    [Index(0)]
    public string Key
    {
        get => _key;
        set => this.RaiseAndSetIfChanged(ref _key, value);
    }

    [Index(1)]
    public string Text
    {
        get => _text;
        set => this.RaiseAndSetIfChanged(ref _text, value);
    }
}