using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SaveFile : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private GameObject saveFilePrefab;
    [SerializeField] private Button deleteButtonPrefab;
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private Texture2D cursor;
    private void Start()
    {
        LoadSaveFiles();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log(PlayerIDTransfer.playerID);
        }
    }

    private void LoadSaveFiles()
    {
        GameObject placeHolder = Instantiate(saveFilePrefab);
        placeHolder.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, container.rect.height / 5f);
        placeHolder.AddComponent<HorizontalLayoutGroup>().childControlWidth = false;
        placeHolder.GetComponent<Image>().color = Color.clear;

        GameObject instance = Instantiate(placeHolder, placeHolder.transform);
        instance.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, container.rect.width / 100f * 90f);
        instance.GetComponent<Image>().color = saveFilePrefab.GetComponent<Image>().color;
        instance.AddComponent<ButtonCursorChange>().hoverCursor = cursor;
        HorizontalLayoutGroup layoutGroup = instance.GetComponent<HorizontalLayoutGroup>();
        layoutGroup.childControlWidth = false; layoutGroup.childControlHeight = true; layoutGroup.childAlignment = TextAnchor.UpperCenter;
        instance.transform.SetParent(placeHolder.transform);

        Button button = Instantiate(deleteButtonPrefab, placeHolder.transform);
        button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, container.rect.width / 100 * 10);
        button.AddComponent<ButtonCursorChange>().hoverCursor = cursor;

        foreach (PlayerGameData data in PlayerData.instance.LoadAllPlayers())
        {
            GameObject finalPrefab = Instantiate(placeHolder, container);
            finalPrefab.GetComponentInChildren<Button>().onClick.AddListener(() => Delete(finalPrefab));
            finalPrefab.transform.GetChild(0).AddComponent<Button>().onClick.AddListener(() => PlayerIDTransfer.playerID = data.Id);
            string[] dataToShow = new string[] { data.TimeWhenMade, data.FileName, data.Level };

            foreach (string info in dataToShow)
            {
                GameObject gameobjectText = Instantiate(textPrefab, finalPrefab.transform.GetChild(0));
                gameobjectText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, finalPrefab.transform.GetChild(0).GetComponent<RectTransform>().rect.width / 100 * 33.33f);
                TextMeshProUGUI text = gameobjectText.GetComponent<TextMeshProUGUI>();
                text.text = info;
            }
            finalPrefab.name = data.Id;
        }

        Destroy(placeHolder);
    }

    private void Delete(GameObject prefab)
    {
        PlayerData.instance.DeleteAPlayer(prefab.name);
        PlayerIDTransfer.playerID = null;
        Destroy(prefab);
    }
}
