using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dash_pickups : MonoBehaviour
{
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Dash.dashAmount += 1;
    }

}
