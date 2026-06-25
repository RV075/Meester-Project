using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class DataToLoad : MonoBehaviour
{
    public static List<GameObject> dashObjects = new();
    public static List<SpriteRenderer> dashSpritesRenderer = new();

    public static List<GameObject> playerLaserBulletObjects = new();
    public static List<GameObject> bulletObjects = new();
    public static List<GameObject> rocketObjects = new();

    public static GameObject player;
    private void Awake()
    {
        Checkpoint.checkpoints.Clear();
        dashObjects.Clear();
        dashSpritesRenderer.Clear();
        playerLaserBulletObjects.Clear();
        bulletObjects.Clear();
        rocketObjects.Clear();

        player = FindObjectOfType<Player>().gameObject;

        for (int i = 0; i < 10; i++)
        {
            GameObject dashObject = new("DashObject");
            dashObject.transform.position = new Vector2(1000, 1000);
            dashObjects.Add(dashObject);
            dashSpritesRenderer.Add(dashObject.AddComponent<SpriteRenderer>());
        }
    }
    private void Start()
    {
        SpawnPlayer();
    }

    public static void SpawnPlayer()
    {
        if (Checkpoint.checkpoints.Count == 0)
        {
            Debug.LogWarning("No checkpoints in scene! Add atleast 1");
            return;
        }
        foreach (Checkpoint checkpoint in Checkpoint.checkpoints)
        {
            if (checkpoint.checkpointID == PlayerData.data.gameData.CheckPoint)
            {
                if (checkpoint != null)
                    player.transform.position = checkpoint.transform.position;
                break;
            }
        }
    }
}
