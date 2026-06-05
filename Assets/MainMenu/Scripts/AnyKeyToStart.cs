using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyKeyToStart : MonoBehaviour
{
    [Header("Objects to toggle")]
    [SerializeField] private GameObject toTurnOff;
    [SerializeField] private GameObject toTurnOn;
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            ToggleObject(toTurnOff, toTurnOn);
        }
    }
    private void ToggleObject(GameObject toTurnOff, GameObject toTurnOn)
    {
        if (toTurnOff != null)
            toTurnOff.SetActive(false);

        if (toTurnOn != null)
            toTurnOn.SetActive(true);
    }
}