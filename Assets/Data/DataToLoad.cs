using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataToLoad : MonoBehaviour
{
    public static List<GameObject> dashObjects = new();
    public static List<SpriteRenderer> dashSpritesRenderer = new();

    public static List<GameObject> playerLaserBulletObjects = new();
    public static List<GameObject> playerAutoTurretBulletObjects = new();
    public static List<GameObject> bulletObjects = new();
    public static List<GameObject> rocketObjects = new();

    public static GameObject player;
    private void Awake()
    {
        player = FindObjectOfType<Player>().gameObject;

        for (int i = 0; i < 10; i++)
        {
            GameObject dashObject = new("DashObject");
            dashObject.transform.position = new Vector2(1000, 1000);
            dashObjects.Add(dashObject);
            dashSpritesRenderer.Add(dashObject.AddComponent<SpriteRenderer>());
        }
    }
}
