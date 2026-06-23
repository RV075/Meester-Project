using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public void BuySpeed()
    {
        if (!UpgradeManager.Instance.speedBought)
        {
            UpgradeManager.Instance.speedBought = true;
            UpgradeManager.Instance.speedMultiplier = 1.2f; // Slight speed boost
            LoadNextLevel();
        }
    }

    public void BuyJump()
    {
        if (!UpgradeManager.Instance.jumpBought)
        {
            UpgradeManager.Instance.jumpBought = true;
            UpgradeManager.Instance.jumpMultiplier = 1.15f; // Slight jump boost
            LoadNextLevel();
        }
    }

    public void BuyDash()
    {
        if (!UpgradeManager.Instance.dashBought)
        {
            UpgradeManager.Instance.dashBought = true;
            UpgradeManager.Instance.dashMultiplier = 1.3f; // Slight dash distance boost
            LoadNextLevel();
        }
    }

    public void SkipShop()
    {
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }
}