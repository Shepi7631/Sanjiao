using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AudioType { Dig, Win, Lose }

[Serializable]
public struct AudioPair
{
    public AudioType Type;
    public AudioClip Clip;
}

public class AudioManager : SingletonBase<AudioManager>
{

    private AudioSource audioSource;

    [SerializeField] private List<AudioPair> audioList = new List<AudioPair>();

    private Dictionary<AudioType, AudioClip> audioDic = new Dictionary<AudioType, AudioClip>();

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        foreach (var audioPair in audioList)
        {
            audioDic.Add(audioPair.Type, audioPair.Clip);
        }
    }

    public void PlayEffect(AudioType audioType)
    {
        audioSource.PlayOneShot(audioDic[audioType]);
    }
}
