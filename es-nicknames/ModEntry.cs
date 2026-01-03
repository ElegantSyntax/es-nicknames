using es_nicknames.Framework;
using StardewModdingAPI;

namespace es_nicknames;

internal sealed class ModEntry : Mod
{
#if DEBUG
    private const LogLevel DEFAULT_LOG_LEVEL = LogLevel.Debug;
#else
    private const LogLevel DEFAULT_LOG_LEVEL = LogLevel.Trace;
#endif
    private static IMonitor? mon;

    internal const string ModId = "elegantsyntax.es_nicknames";

    public Dictionary<string, List<string>>? NicknameData { get; set; }

    public override void Entry(IModHelper helper)
    {
        mon = Monitor;
        LoadNameData();
        Patches.Apply();
    }

    private void LoadNameData()
    {
        NicknameData = Helper.Data.ReadJsonFile<Dictionary<string, List<string>>>("nicknames.json");
        if (NicknameData == null)
        {
            NicknameData = new(){
                { "Generic", new List<string>{"Firstname Lastname"} }, // 0 - 1 - Aquaintance
                { "Casual", new List<string>{"Firstname"}}, // 2 - 3 - Casual Friend
                { "Friend", new List<string>{"Firsthname", "Shortname"} }, // 4 - 5 - Friend
                { "Close", new List<string>{"Shortname"}}, // 6 - 7 - Close Friend
                { "Best", new List<string>{"Shortname", "Bestie", "Babe", "Girly" }}, // 8 - 9 - Best Friend
                { "Spouse", new List<string>{"Firstname", "Shortname", "Wife", "Love", "Lover", "Darling", "Kitten", "Honey", "Sweetie", "Babe"}}, // 10 - 14 - Roomate / Spouse
                { "Roomate", new List<string>{"Firstname", "Shortname", "Roomie"}}, // 10 - 14 - Roomate / Spouse
                { "Dating", new List<string>{"Firstname", "Shortname", "Love", "Darling", "Kitten", "Honey", "Sweetie", "Babe"}}, // 10 - 14 - Roomate / Spouse
            };
            Helper.Data.WriteJsonFile("nicknames.json", NicknameData);
        }
        Nicknames.NicknameData = NicknameData;
    }

    internal static void Log(string msg, LogLevel level = DEFAULT_LOG_LEVEL)
    {
        mon!.Log(msg, level);
    }

    internal static void LogOnce(string msg, LogLevel level = DEFAULT_LOG_LEVEL)
    {
        mon!.LogOnce(msg, level);
    }

}
