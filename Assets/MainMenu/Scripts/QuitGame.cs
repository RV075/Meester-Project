using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    [Header("Button to quit game")]
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(Quit);
    }
    private void Quit()
    {
        Application.Quit();
    }
}
