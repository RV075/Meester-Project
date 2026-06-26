using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [Tooltip("The name of the scene to load")]
    public static void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
