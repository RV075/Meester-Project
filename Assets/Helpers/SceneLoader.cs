using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [Tooltip("The name of the scene to load")]
    public static void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public static void ToggleObject(GameObject toTurnOff, GameObject toTurnOn)
    {
        if (toTurnOff != null)
            toTurnOff.SetActive(false);

        if (toTurnOn != null)
            toTurnOn.SetActive(true);
    }
}
