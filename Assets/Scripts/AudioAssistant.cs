using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioListener))]
[RequireComponent(typeof(AudioSource))]
public class AudioAssistant : SerializedMonoBehaviour
{
    public static AudioAssistant instance;

    [SerializeField]
    AudioSource music;

    [SerializeField]
    AudioSource sfx;

    private SoundConfig soundCfg;
    static List<TYPE_SOUND> mixBuffer = new List<TYPE_SOUND>(32);
    static float mixBufferClearDelay = 0.05f;
    static float timeUnScaleDentaTime = 0.02f;

    private void OnEnable()
    {
        InitConstant();
        EventGlobalManager.Instance.OnUpdateSetting.AddListener(UpdateSoundSetting);
    }

    private void OnDestroy()
    {
        if (EventGlobalManager.Instance == null)
            return;

        EventGlobalManager.Instance.OnUpdateSetting.RemoveListener(UpdateSoundSetting);
    }

    private void UpdateSoundSetting()
    {
        if (GameManager.Instance != null
            && GameManager.Instance.Data != null
            && GameManager.Instance.Data.Setting.Sound == 0)
        {
            sfx.volume = 0;
            music.volume = 0;
        }
        else
        {
            sfx.volume = soundCfg.VolumeSFX;
            music.volume = soundCfg.VolumeBGM;
        }
    }

    public float musicVolume
    {
        get
        {
            if (GameManager.Instance.Data.Setting.Sound == 0)
            {
                return 0;
            }
            else
            {
                return soundCfg.VolumeBGM;
            }
        }
    }

    public string currentTrack;

    public void InitConstant()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);

        soundCfg = ConfigManager.Instance.Audio;

        StartCoroutine(MixBufferRoutine());
    }

    // Coroutine responsible for limiting the frequency of playing sounds
    IEnumerator MixBufferRoutine()
    {
        float time = 0;

        while (true)
        {
            time += timeUnScaleDentaTime;
            yield return null;
            if (time >= mixBufferClearDelay)
            {
                mixBuffer.Clear();
                time = 0;
            }
        }
    }

    // Launching a music track
    public void PlayMusic(string trackName, float delay = .3f, float delayStartFadeOut = 0f)
    {
        if (trackName != string.Empty)
            currentTrack = trackName;

        AudioClip to = null;
        if (trackName != string.Empty)
        {
            List<Bgm> listTrack = new List<Bgm>(1);

            foreach (Bgm track in soundCfg.ListBgm)
                if (track.name == trackName)
                    listTrack.Add(track);

            int random = Random.Range(0, listTrack.Count);
            to = listTrack[random].track;
        }

        if (instance != null && to != null)
            StartCoroutine(instance.CrossFade(to, delay, delayStartFadeOut));
    }

    public void PlayRandomMusic()
    {
        PlayMusic(soundCfg.ListBgm.PickRandom().name);
    }

    // A smooth transition from one to another music
    IEnumerator CrossFade(AudioClip to, float delay = .3f, float delayStartFadeOut = 0f)
    {
        float countDownDelay = delay;
        if (music.clip != null)
        {
            while (countDownDelay > 0)
            {
                music.volume = countDownDelay * musicVolume;
                countDownDelay -= timeUnScaleDentaTime;
                yield return null;
            }
        }

        music.clip = to;
        if (to == null)
        {
            music.Stop();
            yield break;
        }

        yield return Yielders.Get(delayStartFadeOut);
        countDownDelay = 0;
        if (!music.isPlaying) music.Play();
        while (countDownDelay < delay)
        {
            music.volume = countDownDelay * musicVolume;
            countDownDelay += timeUnScaleDentaTime;
            yield return null;
        }

        music.volume = musicVolume;
    }

    public static void Shot(TYPE_SOUND typeSound)
    {
        if (typeSound != TYPE_SOUND.NONE && !mixBuffer.Contains(typeSound))
        {
            mixBuffer.Add(typeSound);
            if (instance != null && instance.sfx != null && instance.soundCfg.GetAudio(typeSound) != null)
                instance.sfx.PlayOneShot(instance.soundCfg.GetAudio(typeSound));
        }
    }

    public void FadeOutMusic()
    {
        music.volume = 0.1f;
    }

    public void FadeInMusic()
    {
        music.volume = musicVolume;
    }

    public void Pause()
    {
        music.Pause();
    }

    public void PlayAgain()
    {
        music.UnPause();
    }
}