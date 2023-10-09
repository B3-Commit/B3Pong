using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectsSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayEffect(AudioClip audioClip)
    {
        effectsSource.PlayOneShot(audioClip);
    }

    public AudioSource CreateAudioSource(AudioClip clip, bool loop)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>() as AudioSource;
        source.clip = clip;
        source.loop = loop;
        return source;
    }

    public float GetMasterVolume()
    {
        return AudioListener.volume;
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public float GetMusicVolume()
    {
        return musicSource.volume;
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public float GetMusicPitch()
    {
        return musicSource.pitch;
    }

    public void SetMusicPitch(float pitch)
    {
        musicSource.pitch = pitch;
    }

    public float GetEffectsVolume()
    {
        return effectsSource.volume;
    }

    public void SetEffectsVolume(float volume)
    {
        effectsSource.volume = volume;
    }

    public void ToggleEffects()
    {
        effectsSource.mute = !effectsSource.mute;
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
}
