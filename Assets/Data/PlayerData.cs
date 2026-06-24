using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite4Unity3d;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Data
{
    public PlayerGameData gameData;
    public PlayerAbillities abillities;
}

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;
    private SQLiteConnection conn;

    public static Data data = new();

    void Awake()
    {
        instance = this;

        conn = new SQLiteConnection(Path.Combine(Application.persistentDataPath, "save.db"));

        conn.CreateTable<PlayerGameData>();
        conn.CreateTable<PlayerAbillities>();

        LoadPlayerData();

        conn.Execute("DELETE FROM PlayerGameData WHERE FileName = ?", "New Player");
        conn.Execute("DELETE FROM PlayerAbillities WHERE Id NOT IN (SELECT Id FROM PlayerGameData)");

    }

    public void UpdatePlayerData(string level, int checkPoint)
    {
        data.gameData.Level = level;
        data.gameData.CheckPoint = checkPoint;
        conn.InsertOrReplace(data.gameData);
    }

    public void UpdatePlayerAbillities(string abilityName)
    {
        data.abillities.CurrentAbillitie = abilityName;
        conn.InsertOrReplace(data.abillities);

        AddAbility.Remove(Type.GetType(data.abillities.CurrentAbillitie));
        AddAbility.Add(Type.GetType(abilityName));
    }

    public void LoadPlayerData()
    {
        if (PlayerIDTransfer.playerID == null)
        {
            if (SceneManager.GetActiveScene().name == "MainMenu") return;
            data.gameData = new PlayerGameData
            {
                TimeWhenMade = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Id = Guid.NewGuid().ToString(),
                FileName = "New Player",
                Level = "Lab",
                CheckPoint = 0
            };
        }
        else
        {
            data.gameData = conn.Table<PlayerGameData>()
                .Where(x => x.Id == PlayerIDTransfer.playerID)
                .FirstOrDefault();

            data.abillities = conn.Table<PlayerAbillities>()
                .Where(x => x.Id == PlayerIDTransfer.playerID)
                .FirstOrDefault();
        }
    }

    public List<PlayerGameData> LoadAllPlayers()
    {
        return conn.Table<PlayerGameData>().ToList();
    }

    public void DeleteAllPlayers() // Gebruik deze functie alleen voor testdoeleinden, anders worden alle spelers verwijderd
    {
        conn.Execute("DELETE FROM PlayerGameData");
        conn.Execute("DELETE FROM PlayerAbillities");
    }

    public void CreateNewPlayer(string fileName)
    {
        string id = Guid.NewGuid().ToString();
        conn.InsertOrReplace(new PlayerGameData
        {
            TimeWhenMade = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            Id = id,
            FileName = fileName,
            Level = "Lab",
            CheckPoint = 0
        });

        data.gameData = conn.Table<PlayerGameData>()
               .Where(x => x.Id == id)
               .FirstOrDefault();

        PlayerIDTransfer.playerID = id;
    }

    public void DeleteAPlayer(string id)
    {
        conn.Execute("DELETE FROM PlayerGameData WHERE id = ?", id);
    }
}