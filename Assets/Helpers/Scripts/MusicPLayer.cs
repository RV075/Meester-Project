using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPLayer : MonoBehaviour
{
    [Header("Place your music here")]
    [SerializeField] private AudioClip musicClip;
    [Header("Settings")]
    [Range(0.1f, 60f)]
    [SerializeField] private float cooldown = 30f;
    private AudioSource audioSource;

    private Coroutine playCoroutine;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.Play();
    }


    private void Update()
    {
        if (audioSource == null) return;

        if (!audioSource.isPlaying)
        {
            playCoroutine ??= StartCoroutine(PlayMusic());
        }
    }

    private IEnumerator PlayMusic()
    {
        yield return new WaitForSeconds(cooldown);
        audioSource.Play();
        playCoroutine = null;
    }
}
