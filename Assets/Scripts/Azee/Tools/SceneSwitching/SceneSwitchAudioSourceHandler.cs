using System.Collections;
using System.Collections.Generic;
using ProBuilder2.Common;
using UnityEngine;

public class SceneSwitchAudioSourceHandler : SceneSwitchHandler
{
    private class AudioSourceData
    {
        public AudioSource audioSource = null;
        public bool wasPlaying = false;
        public float offset = 0f;
    }

    private List<AudioSourceData> _audioSourceDataList = null;

    void Awake()
    {
        InitMissingVars();
    }

    // Use this for initialization
    void Start()
    {
        InitMissingVars();

        foreach (AudioSourceData audioSourceData in _audioSourceDataList)
        {
//            Debug.Log("AudioSource: " + audioSourceData.audioSource.clip.name);
            audioSourceData.audioSource.playOnAwake = false;
        }

        OnScenePaused.AddListener(OnScenePausedCallback);
        OnSceneResumed.AddListener(OnSceneResumedCallback);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnScenePausedCallback()
    {
        InitMissingVars();

        foreach (AudioSourceData audioSourceData in _audioSourceDataList)
        {
            audioSourceData.wasPlaying = audioSourceData.audioSource.isPlaying;
            audioSourceData.offset = audioSourceData.audioSource.time;
        }
    }

    void OnSceneResumedCallback()
    {
        InitMissingVars();

        foreach (AudioSourceData audioSourceData in _audioSourceDataList)
        {
            if (audioSourceData.wasPlaying)
            {
                audioSourceData.audioSource.time = audioSourceData.offset;
                audioSourceData.audioSource.Play();
            }
        }
    }

    void InitMissingVars()
    {
        if (_audioSourceDataList == null)
        {
            _audioSourceDataList = new List<AudioSourceData>();
            foreach (AudioSource audioSource in GetComponents<AudioSource>())
            {
//                Debug.Log("AudioSource: " + audioSource.clip.name);
                AudioSourceData audioSourceData = new AudioSourceData
                {
                    audioSource = audioSource
                };

                _audioSourceDataList.Add(audioSourceData);
//                Debug.Log("Added!");
            }
        }
    }
}