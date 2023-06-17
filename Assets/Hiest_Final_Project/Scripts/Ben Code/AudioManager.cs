using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip introTrack;
    public AudioClip actionTrack;
    public List<AudioClip> additionalTracks;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayIntroTrack();
    }

    public void PlayIntroTrack()
    {
        audioSource.Stop();
        audioSource.clip = introTrack;
        audioSource.Play();
    }

    public void PlayActionTrack()
    {
        audioSource.Stop();
        audioSource.clip = actionTrack;
        audioSource.Play();
    }

    public void PlayAdditionalTrack(int trackIndex)
    {
        if (trackIndex < 0 || trackIndex >= additionalTracks.Count)
        {
            Debug.LogError("Invalid track index: " + trackIndex);
            return;
        }

        audioSource.Stop();
        audioSource.clip = additionalTracks[trackIndex];
        audioSource.Play();
    }
}
