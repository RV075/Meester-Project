using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveFile : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private GameObject saveFilePrefab;
    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            float newHeight = container.rect.height / 5f;
            saveFilePrefab.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
            Instantiate(saveFilePrefab, container);
        }
    }
}
