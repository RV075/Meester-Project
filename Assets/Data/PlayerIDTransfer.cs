using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class PlayerIDTransfer : ScriptableObject
{
    public static string playerID = null; // slaat de ID tussen de main scene en de game op, zodat we weten welke speler we moeten laden
}

public class PlayerGameData
{
    public string TimeWhenMade { get; set; } // time when made
    [PrimaryKey]
    public string Id { get; set; } // player id
    public string FileName { get; set; } // players name
    public string Level { get; set; } // players level

    public int CheckPoint { get; set; } // players checkpoint
}

public class PlayerAbillities
{
    public string Id { get; set; } // player id

    public string CurrentAbillitie { get; set; }
}