using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public bool speedBought = false;
    public bool jumpBought = false;
    public bool dashBought = false;

    public float speedMultiplier = 1f;
    public float jumpMultiplier = 1f;
    public float dashMultiplier = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}