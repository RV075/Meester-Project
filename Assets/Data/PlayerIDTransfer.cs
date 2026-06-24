using SQLite4Unity3d;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class PlayerIDTransfer : ScriptableObject
{
    public static string playerID = null;
}

public class PlayerGameData
{
    public string TimeWhenMade { get; set; }

    [PrimaryKey]
    public string Id { get; set; }

    public string FileName { get; set; }
    public string Level { get; set; }
    public int CheckPoint { get; set; }
}

public class PlayerAbillities
{
    public string Id { get; set; }
    public string CurrentAbillitie { get; set; }
}
