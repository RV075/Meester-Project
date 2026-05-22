using System;
using UnityEngine;

public class AddAbility : MonoBehaviour
{
    public static void Add(Type abilityName)
    {
        if (DataToLoad.player.GetComponent(abilityName) == null)
        {
            DataToLoad.player.AddComponent(abilityName);
        }
    }
}