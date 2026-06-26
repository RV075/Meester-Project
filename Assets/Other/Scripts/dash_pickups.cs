using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_pickups : MonoBehaviour
{
    [SerializeField] private int amount = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Dash.dashAmount += amount;
            gameObject.SetActive(false);
        }
    }
}
