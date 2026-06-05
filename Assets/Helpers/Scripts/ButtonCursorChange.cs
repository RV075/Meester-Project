using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonCursorChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Cursor instellingen")]
    public Texture2D hoverCursor;
    public Vector2 hotspot = new(64, 64);
    public CursorMode cursorMode = CursorMode.Auto;

    private Texture2D defaultCursor;
    private Vector2 defaultHotspot;
    private CursorMode defaultMode;

    private void Awake()
    {
        defaultCursor = null;
        defaultHotspot = Vector2.zero;
        defaultMode = CursorMode.Auto;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverCursor != null)
        {
            Cursor.SetCursor(hoverCursor, hotspot, cursorMode);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(defaultCursor, defaultHotspot, defaultMode);
    }
}
