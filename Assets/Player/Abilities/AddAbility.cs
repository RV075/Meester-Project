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

    public static void Remove(Type abilityName)
    {
        if (DataToLoad.player.GetComponent(abilityName) != null)
        {
            Destroy(DataToLoad.player.GetComponent(abilityName));
        }
    }
}