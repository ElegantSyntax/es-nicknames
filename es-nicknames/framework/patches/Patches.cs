using System.Text.RegularExpressions;
using HarmonyLib;
using StardewModdingAPI;
using StardewValley;

namespace es_nicknames.Framework;

internal static partial class Patches
{
    private static readonly Regex dialogueTokenizedStringPattern = new(@"@");

    private static readonly Harmony harmony = new Harmony(ModEntry.ModId);

    internal static void Apply()
    {
        try
        {
            Apply_Nicknames();
        }
        catch (Exception err)
        {
            ModEntry.Log($"Failed to patch Nicknames:\n{err}", LogLevel.Error);
        }
    }

    internal static void Apply_Nicknames()
    {
        harmony.Patch(
           original: AccessTools.DeclaredMethod(typeof(Dialogue), "parseDialogueString"),
            prefix: new HarmonyMethod(typeof(Patches), nameof(Dialogue_parseDialogueString_Prefix))
        );
    }

    private static void Dialogue_parseDialogueString_Prefix(Dialogue __instance, ref string masterString)
    {
        masterString = dialogueTokenizedStringPattern.Replace(masterString, $"{Nicknames.GetNameBasedOnRelationship(__instance, masterString.Contains("Mr. @") || masterString.Contains("Ms. @"))}");
    }
}
