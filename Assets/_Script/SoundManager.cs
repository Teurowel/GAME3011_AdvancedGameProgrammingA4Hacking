using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : Singleton<SoundManager>
{
    public Sound[] listOfSounds;

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(gameObject);

        //Create AudioSource for each sound
        foreach (Sound s in listOfSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound foundSound = Array.Find(listOfSounds, sound => sound.name == name);
        if (foundSound != null)
        {
            foundSound.source.PlayOneShot(foundSound.clip);
        }
    }

}