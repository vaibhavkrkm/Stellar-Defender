using System.Collections.Generic;
using UnityEngine;

public static class Global
{
    public static int score = 0;
    public static int coins = 0;
    public static int total_coins = 0;
    public static int selectedLevel = 2;
    public static int levelsUnlockedTill = 1;
    public static float playerBulletMultiplier;
    public static bool newGame = true;

    public static bool shieldsUp = false;
    public static int magnetMultiplier = 1;
    public static int strongBullets = 3;

    public const int magnetPower = 10;
    public const int totalLevels = 12;

    public static Dictionary<string, Dictionary<int, float>> playerUpgradesDict = new Dictionary<string, Dictionary<int, float>>()
    {
        {"health", new Dictionary<int, float> { { 1, 1f }, { 2, 1.3f }, { 3, 1.5f } } },
        {"damage", new Dictionary<int, float> { { 1, 1f }, { 2, 1.4f }, { 3, 1.7f } } },
        {"magnet", new Dictionary<int, float> { { 1, 0f }, { 2, 4f }, { 3, 8f } } },
        {"shield", new Dictionary<int, float> { { 1, 0f }, { 2, 3f }, { 3, 6f } } },
    };

    public static Dictionary<string, Dictionary<int, int>> playerUpgradesCosts = new Dictionary<string, Dictionary<int, int>>()
    {
        {"health", new Dictionary<int, int> { { 1, 0 }, { 2, 500 }, { 3, 2500 } } },
        {"damage", new Dictionary<int, int> { { 1, 0 }, { 2, 500 }, { 3, 2500 } } },
        {"magnet", new Dictionary<int, int> { { 1, 0 }, { 2, 500 }, { 3, 2500 } } },
        {"shield", new Dictionary<int, int> { { 1, 0 }, { 2, 500 }, { 3, 2500 } } },
    };

    // update the following dictionary when player upgrades
    public static Dictionary<string, int> currentPlayerLevels = new Dictionary<string, int>()
    {
        {"health", 1 },
        {"damage", 1 },
        {"magnet", 1 },
        {"shield", 1 },
    };

    public static T GetRandomElement<T>(this T[] array)
    {
        if (array == null || array.Length == 0) return default(T);

        T randomElement = array[Random.Range(0, array.Length)];
        return randomElement;
    }

    public static T GetRandomElement<T>(this List<T> list)
    {
        if (list == null || list.Count == 0) return default(T);

        T randomElement = list[Random.Range(0, list.Count)];
        return randomElement;
    }
}
