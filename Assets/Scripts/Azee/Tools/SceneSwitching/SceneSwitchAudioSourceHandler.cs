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
        public float prevTime = 0f;
        public int prevTimeSamples = 0;
        public IEnumerator resetPlaybackTimeCoroutine;
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
            audioSourceData.prevTime = audioSourceData.audioSource.time;
            audioSourceData.prevTimeSamples = audioSourceData.audioSource.timeSamples;
        }
    }

    void OnSceneResumedCallback()
    {
        InitMissingVars();

        foreach (AudioSourceData audioSourceData in _audioSourceDataList)
        {
            if (audioSourceData.wasPlaying)
            {
                // NOTE: This can cause a glitch when the audio source plays some other clip before this clip is finished and the audio source is reset.
                // NOTE: It won't cause a glitch on looping audio sources.

                audioSourceData.audioSource.time = audioSourceData.prevTime;
                audioSourceData.audioSource.Play();

                if (!audioSourceData.audioSource.loop)
                {
                    audioSourceData.resetPlaybackTimeCoroutine = ResetPlaybackTimeAtEndOfClip(audioSourceData);
                    StartCoroutine(audioSourceData.resetPlaybackTimeCoroutine);
                }
            }
            else
            {
                if (!audioSourceData.audioSource.loop)
                {
                    if (audioSourceData.resetPlaybackTimeCoroutine != null)
                    {
                        ResetPlaybackTimeNow(audioSourceData);
                    }
                }
            }
        }
    }

    IEnumerator ResetPlaybackTimeAtEndOfClip(AudioSourceData audioSourceData)
    {
        float remainingTime = audioSourceData.audioSource.clip.length - audioSourceData.audioSource.time;
        yield return new WaitForSecondsRealtime(remainingTime);

        ResetPlaybackTimeNow(audioSourceData);
    }

    void ResetPlaybackTimeNow(AudioSourceData audioSourceData)
    {
        audioSourceData.audioSource.time = 0;
        audioSourceData.resetPlaybackTimeCoroutine = null;
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