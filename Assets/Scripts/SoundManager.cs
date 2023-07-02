using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    public AudioSource soundEffect;
    public AudioSource soundMusic;
    public SoundType[] Sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic(global::Sounds.BGMusic);
    }
    public void PlayMusic(Sounds sound)
    {
        AudioClip clip = GetSoundClip(sound);
        if (clip != null)
        {
            soundMusic.clip = clip;
            soundMusic.Play();
        }
        else
        {
            Debug.LogError("Clip not found for sound tyep: " + sound);
        }
    }

    public void PlaySound(Sounds sound)
    {
        AudioClip clip = GetSoundClip(sound);
        if (clip != null)
        {
            soundEffect.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Clip not found for sound tyep: " + sound);
        }
    }

    private AudioClip GetSoundClip(Sounds sound)
    {
        SoundType item = Array.Find(Sounds, i => i.soundType == sound);
        if (item != null)
            return item.soundClip;
        return null;
    }
    public void StopMusic()
    {
        soundMusic.enabled = false;
    }
    public void StopSfx()
    {
        soundEffect.enabled = false;
    }
    public void StartMusic()
    {
        soundMusic.enabled = true;
    }
    public void StartSfx()
    {
        soundEffect.enabled = true;
    }
}

[Serializable]
public class SoundType
{
    public Sounds soundType;
    public AudioClip soundClip;
}

public enum Sounds
{
    PlayButtonClick,
    ExitButtonClick,
    Pause,
    BGMusic,
    PistolFire,
    PistolSocket,
    SpiderHit,
    SpiderDie,
    SpiderAttack,
    GoblinHit,
    GoblinDie,
    GoblinAttack,
    MinoHit,
    MinoDie,
    MinoAttack,
    PlayerHit,
    PlayerDie,
    GameOver,
    PlayerWin,
    Error,
    Celebrate
}