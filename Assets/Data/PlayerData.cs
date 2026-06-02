using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    private SQLiteConnection conn;
    public static PlayerSave data;
    public List<PlayerSave> allPlayers;

    void Awake()
    {
        Instance = this;

        string dbPath = System.IO.Path.Combine(Application.persistentDataPath, "save.db");
        conn = new(dbPath);

        // Maak tabellen aan als ze nog niet bestaan
        conn.CreateTable<PlayerSave>();

        LoadPlayerData();

        DeleteAllPlayers(); // Gebruik deze functie alleen voor testdoeleinden, anders worden alle spelers verwijderd
    }

    public void UpdatePlayerData(string level, int checkPoint)
    {
        data.Level = level;
        data.CheckPoint = checkPoint;
        conn.InsertOrReplace(data);
    }

    public void LoadPlayerData()
    {
        data = conn.Table<PlayerSave>()
               .Where(x => x.Id == PlayerIDTransfer.playerID)
               .FirstOrDefault();
    }

    public List<PlayerSave> LoadAllPlayers()
    {
        return conn.Table<PlayerSave>().ToList();
    }

    public void DeleteAllPlayers() // Gebruik deze functie alleen voor testdoeleinden, anders worden alle spelers verwijderd
    {
        conn.Execute("DELETE FROM PlayerSave");
    }

    public void CreateNewPlayer(string fileName)
    {
        string id = Guid.NewGuid().ToString();
        conn.InsertOrReplace(new PlayerSave
        {
            TimeWhenMade = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            Id = id,
            FileName = fileName,
            Level = "Lab",
            CheckPoint = 0
        });

        data = conn.Table<PlayerSave>()
               .Where(x => x.Id == id)
               .FirstOrDefault();

        PlayerIDTransfer.playerID = id;
    }

    public void DeleteAPlayer()
    {
        conn.Execute("DELETE FROM PlayerSave WHERE id = ?", data.Id);
    }
}