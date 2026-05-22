using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataToLoad : MonoBehaviour
{
    public static DataToLoad instance;
    public List<GameObject> dashObjects;
    public List<SpriteRenderer> dashSpritesRenderer;

    public static List<GameObject> playerLaserBulletObjects = new();
    public static List<GameObject> playerAutoTurretBulletObjects = new();
    public static List<GameObject> bulletObjects = new();
    public static List<GameObject> rocketObjects = new();

    public static GameObject player;
    private void Awake()
    {
        player = FindObjectOfType<Player>().gameObject;
        instance = this;
    }
}
