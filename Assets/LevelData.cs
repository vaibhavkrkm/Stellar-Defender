using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Create new Level")]
public class LevelData : ScriptableObject
{
    public int levelNo;
    public List<WaveData> waves;
}
