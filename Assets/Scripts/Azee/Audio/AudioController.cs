using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [SerializeField]
    private AudioSource _selectedAudioSource;

    public List<AudioClip> AudioClips;

    void Awake()
    {
        if (_selectedAudioSource == null)
        {
            _selectedAudioSource = GetComponent<AudioSource>();
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioSource GetSelectedAudioSource()
    {
        return _selectedAudioSource;
    }

    public bool SelectClip(int index)
    {
        if (index >= AudioClips.Count)
        {
            Debug.LogError("Invalid Audio Clip Index: " + index);
            return false;
        }

        _selectedAudioSource.clip = AudioClips[index];
        return true;
    }

    public void PlayClip(int index)
    {
        if (SelectClip(index))
        {
//            _selectedAudioSource.time = 0;
            _selectedAudioSource.Play();
        }
    }
}