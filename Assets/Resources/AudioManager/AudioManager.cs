using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public const float NORMAL_TIME = 1f;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectsSource;
    [SerializeField] private AudioSource heartBeatAudioSource;
    public float volumeReduction = 8;
    public float maximumSpeedReduction = 0.2f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SlowMoManagerScript.SetTimeScale += SetMusicSpeed;
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

    public void SetMusicSpeed(float speed)
    {
        if (!heartBeatAudioSource.isPlaying && speed < NORMAL_TIME)
        {
            Debug.Log("start heartbeat");

            AudioManager.Instance.SetMusicVolume(AudioManager.Instance.GetMusicVolume() / volumeReduction);

            StartCoroutine("SlowDownMusic");
            heartBeatAudioSource.Play();
        }
        else if (heartBeatAudioSource.isPlaying && speed == NORMAL_TIME)
        {
            Debug.Log("stop heartbeat");
            AudioManager.Instance.SetMusicVolume(AudioManager.Instance.GetMusicVolume() * volumeReduction);
            StopCoroutine("SlowDownMusic");
            musicSource.pitch = 1f;
            heartBeatAudioSource.Stop();
        }
    }

    IEnumerator SlowDownMusic()
    {
        float duration = 0.3f;
        float targetPitch = 0.9f;
        float startPitch = 1;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float lerpFactor = t / duration;
            musicSource.pitch = Mathf.Lerp(startPitch, targetPitch, lerpFactor);
            yield return null;
        }

        musicSource.pitch = targetPitch;
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
