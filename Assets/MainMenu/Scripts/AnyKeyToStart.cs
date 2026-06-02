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
            SceneLoader.ToggleObject(toTurnOff, toTurnOn);
        }
    }
}
