using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int levelsUnlockedTill;
    public int total_coins;
    public Dictionary<string, int> currentPlayerLevels = new Dictionary<string, int>();
    public bool newGame;

    public PlayerData()
    {
        this.levelsUnlockedTill = Global.levelsUnlockedTill;
        this.total_coins = Global.total_coins;
        this.currentPlayerLevels = Global.currentPlayerLevels;
        this.newGame = Global.newGame;
    }
}
