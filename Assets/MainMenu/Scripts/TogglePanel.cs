using UnityEngine;
using UnityEngine.UI;

public class TogglePanel : MonoBehaviour
{
    [Header("Button to toggle setting")]
    [SerializeField] private Button button;
    [Header("Objects to toggle")]
    [SerializeField] private GameObject toTurnOff;
    [SerializeField] private GameObject toTurnOn;

    private void Start()
    {
        button.onClick.AddListener(Toggle);
    }

    private void Toggle()
    {
        if (toTurnOff != null)
            toTurnOff.SetActive(false);

        if (toTurnOn != null)
            toTurnOn.SetActive(true);

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
