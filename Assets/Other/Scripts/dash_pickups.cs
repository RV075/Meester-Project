using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dash_pickups : MonoBehaviour
{
    private bool ableToDash = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ableToDash) return;
        Dash.dashAmount += 1;
        ableToDash = false;
        gameObject.SetActive(false);
    }

}
