using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint ID")]
    public int checkpointID;
    private Vector2 spawn;

    public static List<Checkpoint> checkpoints = new();

    private void Awake()
    {
        spawn = transform.position;
        checkpoints.Add(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (checkpointID > PlayerData.data.gameData.CheckPoint)
                PlayerData.instance.UpdatePlayerData(PlayerData.data.gameData.Level, checkpointID);
        }
    }
}
