using StardewValley;
using StardewModdingAPI;

namespace es_nicknames.Framework;

internal static class Nicknames
{
    public enum NameOptions
    {
        FirstName,
        LastName,
        Both
    }

    public static Dictionary<string, List<string>>? NicknameData { get; set; }

    internal static string GetNameBasedOnRelationship(Dialogue dialogue, bool hasPrefix = false)
    {
        Game1.player.friendshipData.TryGetValue(dialogue.speaker.Name, out Friendship relationship);

        int FRIENDSHIP_LEVEL = GetRelationshipLevel(relationship);

        string name = "";

        if (FRIENDSHIP_LEVEL > 0 && relationship.IsRoommate())
        {
            name = GetRandomName("Roomate");
        }
        else if (FRIENDSHIP_LEVEL > 0 && relationship.IsMarried())
        {
            name = GetRandomName("Spouse");
        }
        else if (FRIENDSHIP_LEVEL > 0 && (relationship.IsDating() || relationship.IsEngaged()))
        {
            name = GetRandomName("Dating");
        }
        else if (FRIENDSHIP_LEVEL < 2)
        {
            name = SplitName(GetRandomName("Generic"), FRIENDSHIP_LEVEL < 1 ? NameOptions.LastName : NameOptions.Both);
            name = hasPrefix ? name : "${Mr.^Ms.^Mx.}$ " + name;
        }
        else if (FRIENDSHIP_LEVEL < 4)
        {
            name = GetRandomName("Casual");
        }
        else if (FRIENDSHIP_LEVEL < 6)
        {
            name = GetRandomName("Friend");
        }
        else if (FRIENDSHIP_LEVEL < 8)
        {
            name = GetRandomName("Close");
        }
        else if (FRIENDSHIP_LEVEL < 10)
        {
            name = GetRandomName("Best");
        }

        return name;
    }


    internal static int GetRelationshipLevel(Friendship relationship)
    {
        if (relationship == null || relationship?.Points == null) { return 0; }
        return relationship.Points / 250; // 250 points = 1 friendship level
    }

    internal static string GetRandomName(string key)
    {
        return NicknameData![key][Game1.random.Next(0, NicknameData[key].Count)];
    }

    internal static string SplitName(string name, NameOptions options = NameOptions.Both)
    {
        string[] split = name.Split(' ');
        string firstName = split[0];
        string lastName = split[^1] ?? firstName;

        switch (options)
        {
            case NameOptions.FirstName:
                return firstName;
            case NameOptions.LastName:
                return lastName;
        }
        return split[Game1.random.Next(0, split.Length)];
    }
}