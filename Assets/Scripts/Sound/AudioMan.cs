using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioMan : MonoBehaviour
{
    public static AudioMan Instance;
    private Coroutine fadeOutCoroutine;
    [Header("Audio Sources")]
    private AudioSource musicSource;
    private List<AudioSource> sfxSources = new List<AudioSource>();
    public int sfxPoolSize = 8;

    [Header("Sound Clips")]
    public List<SoundClip> soundClips;

    private Dictionary<SoundType, AudioClip> soundDictionary;

    [System.Serializable]
    public struct SoundClip
    {
        public SoundType type;
        public AudioClip clip;
    }

    public enum SoundType
    {
        Engine,
        EngineWait,
        Zombie,
        ZombieJump,
        BackgroundMusic
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;

        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource sfx = gameObject.AddComponent<AudioSource>();
            sfxSources.Add(sfx);
        }

        soundDictionary = new Dictionary<SoundType, AudioClip>();
        foreach (var soundClip in soundClips)
        {
            soundDictionary[soundClip.type] = soundClip.clip;
        }
    }

    public void PlaySFX(SoundType sound, float volume = 1f)
    {
        if (!soundDictionary.ContainsKey(sound))
        {
            Debug.LogWarning($"SFX {sound} bulunamadý.");
            return;
        }

        AudioSource availableSource = GetAvailableSFXSource();
        if (availableSource != null)
        {
            availableSource.volume = volume;
            availableSource.PlayOneShot(soundDictionary[sound]);
        }
    }

    public void PlayMusic(SoundType music,float volume = 1f)
    {
        if (!soundDictionary.ContainsKey(music))
        {
            Debug.LogWarning($"Müzik {music} bulunamadý.");
            return;
        }

        musicSource.clip = soundDictionary[music];
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    private AudioSource GetAvailableSFXSource()
    {
        foreach (var source in sfxSources)
        {
            if (!source.isPlaying)
                return source;
        }
        return null; // Hepsi doluysa null döner
    }
    

    public void FadeOutMusic(float duration = 0.5f)
    {
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }
        fadeOutCoroutine = StartCoroutine(FadeOutRoutine(duration));
    }

    private IEnumerator FadeOutRoutine(float duration)
    {
        float startVolume = musicSource.volume;

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0.3f, time / duration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume; // Sonraki çalmalarda sesi tekrar normal kullanabilelim
        AudioMan.Instance.PlayMusic(AudioMan.SoundType.EngineWait,0.5f);
    }
}
