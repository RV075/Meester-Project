using System;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData
{
    public static string playerID = "1d31d964-8993-45ae-9e77-0186e2c081fb"; // slaat de ID tussen de main scene en de game op, zodat we weten welke speler we moeten laden
}

public class PlayerSave
{
    [PrimaryKey]
    public string Id { get; set; } // player id
    public string Name { get; set; } // players name
    public string Level { get; set; } // players level

}
public class Database : MonoBehaviour
{
    public static Database Instance;

    private SQLiteConnection conn;
    public PlayerSave data;

    void Awake()
    {
        Instance = this;

        string dbPath = System.IO.Path.Combine(Application.persistentDataPath, "save.db");
        conn = new(dbPath);

        // Maak tabellen aan als ze nog niet bestaan
        conn.CreateTable<PlayerSave>();

        LoadPlayerData();
    }

    public void UpdatePlayerData(string level)
    {
        data.Level = level;
        conn.InsertOrReplace(data);
    }

    public void LoadPlayerData()
    {
        data = conn.Table<PlayerSave>()
               .Where(x => x.Id == GameData.playerID)
               .FirstOrDefault();
    }

    public void CreateNewPlayer(string name)
    {
        conn.InsertOrReplace(new PlayerSave
        {
            Id = Guid.NewGuid().ToString(),
            Name = name,
            Level = "Lab",
        });
    }

    public void DeleteAPlayer()
    {
        conn.Execute("DELETE FROM PlayerSave WHERE id = ?", data.Id);
    }
}