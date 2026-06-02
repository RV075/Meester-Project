using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewGame : MonoBehaviour
{
    [Header("Button to press")]
    [SerializeField] private Button button;
    [Header("SaveFileName Input")]
    [SerializeField] private TMP_InputField inputField;


    private void Start()
    {
        button.onClick.AddListener(CreateNewGame);
    }

    private void CreateNewGame()
    {
        PlayerData.Instance.CreateNewPlayer(inputField.text);
        SceneLoader.LoadScene("Lab");
    }
}
