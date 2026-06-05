using System.Linq;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor;
using UnityEngine;

public class PrettyDatabaseDebugger : EditorWindow
{
    [MenuItem("Window/Pretty Database Debugger")]
    public static void ShowWindow()
    {
        GetWindow<PrettyDatabaseDebugger>("Pretty Database Debugger");
    }

    private SQLiteConnection conn;

    void OnEnable()
    {
        conn = new(System.IO.Path.Combine(Application.persistentDataPath, "save.db"));
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Load Player Data"))
        {
            var allPlayers = conn.Table<PlayerGameData>().ToList();
            foreach (var player in allPlayers)
            {
                Debug.Log($"ID: {player.Id}, FileName: {player.FileName}, TimeWhenMade: {player.TimeWhenMade}, Level: {player.Level}, CheckPoint: {player.CheckPoint}");
            }
        }

        if (GUILayout.Button("Clear console"))
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            method.Invoke(null, null);
        }

        if (GUILayout.Button("Delete All Players"))
        {
            conn.Execute("DELETE FROM PlayerGameData");
        }
    }
}