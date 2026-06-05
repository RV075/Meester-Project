using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [Header("Button to start game")]
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(StartTheGame);
    }

    private void StartTheGame()
    {
        if (PlayerIDTransfer.playerID == null) return;

        PlayerData.instance.LoadPlayerData();
        SceneLoader.LoadScene(PlayerData.data.gameData.Level);
    }
}
