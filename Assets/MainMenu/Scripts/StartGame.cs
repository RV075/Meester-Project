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
        SceneLoader.LoadScene(PlayerData.data.Level);
    }
}
