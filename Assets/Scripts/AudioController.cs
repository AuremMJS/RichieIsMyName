using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AudioInfo
{
    public string Name;
    public AudioSource AudioSource;
}

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    [SerializeField]
    private AudioInfo[] audioInfos;

    private Dictionary<string, AudioSource> audioSources;

    void Awake()
    {
        if(Instance == null)
            Instance = this;

        audioSources = new Dictionary<string, AudioSource>();
        foreach (var audioInfo in audioInfos)
        {
            audioSources.Add(audioInfo.Name, audioInfo.AudioSource);
        }
    }

    public void PlayAudio(string name, float delay = 0)
    {
        if (audioSources != null && audioSources.ContainsKey(name))
        {
            audioSources[name].PlayDelayed(delay);
        }
    }
}
