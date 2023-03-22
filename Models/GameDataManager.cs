using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
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

    private static readonly CsvConfiguration _csvConfig = new(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = false
    };

    public static JObject CardsData { get; set; } = null!;
    public static ObservableCollection<LocalizedString> StringsData { get; set; } = null!;

    public static bool Modified { get; set; }

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
            using var stringsReader = new CsvReader(stringsStreamReader, _csvConfig);
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
            using var writer = new CsvWriter(sw, _csvConfig);
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