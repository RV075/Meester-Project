using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPLayer : MonoBehaviour
{
    public static MusicPLayer instance;

    [Header("Place your music here")]
    public AudioClip musicClip;
    [Header("Settings")]
    [Range(0.1f, 60f)]
    public float cooldown = 30f;
    private AudioSource audioSource;

    private Coroutine playCoroutine;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.Play();
    }

    public void SwitchMusic(AudioClip _musicClip, float _cooldown)
    {
        musicClip = _musicClip;
        cooldown = _cooldown;

        audioSource.Stop();
        audioSource.clip = musicClip;
        audioSource.Play();

        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
            playCoroutine = null;
        }
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
