using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AudioType { Dig, Win, Lose, Open, Land, Jump, Run, Car, DigBGM, BGM, DreamBGM }

[Serializable]
public struct AudioPair
{
    public AudioType Type;
    public AudioClip Clip;
}

public class AudioManager : SingletonBase<AudioManager>
{
    public AudioType curBGMType;
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

    public void PlayBGM(AudioType audioType)
    {
        if (curBGMType != audioType)
        {
            curBGMType = audioType;
            audioSource.clip = audioDic[curBGMType];
            audioSource.Play();
        }

    }
}
