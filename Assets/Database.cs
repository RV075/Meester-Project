
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

//public class PlayerSave
//{
//    [PrimaryKey, AutoIncrement]
//    public int id { get; set; } // player id
//    public string name { get; set; } // players name
//    public string skin { get; set; } // save player skin

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

    public void SavePlayerData()
    {
        conn.InsertOrReplace(data);
    }

    public void LoadPlayerData()
    {
        data = conn.Table<PlayerSave>()
               .Where(x => x.id == GameData.playerID)
               .FirstOrDefault();
    }

    public void CreateNewPlayer()
    {
        conn.InsertOrReplace(new PlayerSave
        {
            name = "",
            skin = "default" // standaart Playerobject
        });
    }

    public void DeleteAPlayer(int id)
    {
        conn.Execute("DELETE FROM PlayerSave WHERE id = ?", id);
    }
}