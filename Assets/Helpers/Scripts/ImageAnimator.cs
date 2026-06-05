using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageAnimator : MonoBehaviour
{
    [Header("Place your sprites here")]
    [SerializeField] private Sprite[] frames;

    [Header("Time between frames in seconds")]
    [Range(0.05f, 1f)]
    [SerializeField] private float frameTime = 0.1f;

    private Image spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<Image>();
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        int index = 0;
        while (true)
        {
            spriteRenderer.sprite = frames[index];
            index = (index + 1) % frames.Length;
            yield return new WaitForSeconds(frameTime);
        }
    }
}
