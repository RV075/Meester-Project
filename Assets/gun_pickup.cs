using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun_pickup : MonoBehaviour
{
    public Door door;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(door.Open());
    }
}
