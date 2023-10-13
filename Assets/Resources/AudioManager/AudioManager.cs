using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public const float NORMAL_TIME = 1f;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource musicSourceGravity;
    [SerializeField] private AudioSource effectsSource;
    [SerializeField] private AudioSource heartBeatAudioSource;
    public float reducedVolume = 0.1f;
    public float maximumSpeedReduction = 0.2f;

    float defaultMusicVolume;

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

    void OnDestroy()
    {
        SlowMoManagerScript.SetTimeScale -= SetMusicSpeed;
    }


    private void Start()
    {
        defaultMusicVolume = AudioManager.Instance.GetMusicVolume();
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
            StartCoroutine("SlowDownMusic");
            heartBeatAudioSource.Play();
        }
        else if (heartBeatAudioSource.isPlaying && speed == NORMAL_TIME)
        {
            StopCoroutine("SlowDownMusic");
            AudioManager.Instance.SetMusicVolume(defaultMusicVolume);
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
            AudioManager.Instance.SetMusicVolume(Mathf.Lerp(defaultMusicVolume, defaultMusicVolume * reducedVolume, lerpFactor));
            yield return null;
        }

        musicSource.pitch = targetPitch;
    }

    public void UseGravityMusic(bool useGravityMusic)
    {
        if (useGravityMusic && musicSource.isPlaying)
        {
            musicSource.Stop();
            musicSourceGravity.Play();

        }
        else if (!useGravityMusic && musicSourceGravity.isPlaying)
        {
            musicSourceGravity.Stop();
            musicSource.Play();

        }
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
