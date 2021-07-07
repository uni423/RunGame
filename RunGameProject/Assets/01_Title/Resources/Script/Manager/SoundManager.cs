using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] AudioClip[] clips;

    Dictionary<string, AudioClip> clipDic;
    AudioSource sfxPlayer;
    AudioSource bgmPlayer;

    float sfxVolume = 1f;
    float bgmVolume = 1f;

    new void Awake()
    {
        base.Awake();

        sfxPlayer = GetComponent<AudioSource>();
        bgmPlayer = transform.GetChild(0).GetComponent<AudioSource>();
        clipDic = new Dictionary<string, AudioClip>();

        foreach (AudioClip clip in clips)
        {
            clipDic.Add(clip.name, clip);
        }
    }

    public void Start()
    {
        StartCoroutine(Fade("in"));
        bgmPlayer.Play();
    }

    public void PlaySound(string clipname, bool stop = true)
    {
        if (!clipDic.ContainsKey(clipname))
        { Debug.Log("coludn't find sound"); return; }

        if (stop) StopSFX();

        sfxPlayer.PlayOneShot(clipDic[clipname], sfxVolume);
    }

    public GameObject LoopSound(string clipname)
    {
        if (!clipDic.ContainsKey(clipname))
        { Debug.Log("coludn't find sound"); return null; }

        GameObject loop = new GameObject("Loop");
        AudioSource source = loop.AddComponent<AudioSource>();

        source.clip = clipDic[clipname];
        source.volume = bgmVolume;
        source.loop = true;

        StartCoroutine(Fade("in"));
        source.Play();

        return loop;
    }

    public void StopBGM()
    {
        StartCoroutine(Fade("out"));
        bgmPlayer.Stop();
    }

    public void StopSFX()
    {
        StartCoroutine(Fade("out"));
        sfxPlayer.Stop();
    }

    public void PlayBGM(string name = null)
    {
        if (name != null)
        {
            if (!clipDic.ContainsKey(name))
            { Debug.Log("coludn't find sound"); return; }
            else
                bgmPlayer.clip = clipDic[name];
        }
        StartCoroutine(Fade("In"));
        bgmPlayer.Play();
    }

    public void SetSFX(float volume)
    {
        sfxVolume = volume;
    }

    public void SetBGM(float volume)
    {
        bgmVolume = volume;
        bgmPlayer.volume = volume;
    }

    IEnumerator Fade(string fadekind)
    {
        // BGM Base
        float curVolume = 0f;

        while (curVolume <= bgmVolume)
        {
            if (fadekind == "In")
                bgmPlayer.volume = curVolume;
            if (fadekind == "Out")
                bgmPlayer.volume -= curVolume;

            curVolume += Time.deltaTime;
            yield return null;
        }

        bgmPlayer.volume = bgmVolume;
    }

}
