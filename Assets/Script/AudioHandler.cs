using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;



public class AudioHandler : MonoBehaviour
{
    public AudioSource MusicSource, EfxSource;
    public AudioClip[] MusicClip, EfxClip;
    private Random _random = new Random();
    private int MusicIndex;
    private void Start()
    {
        MusicIndex = 0;
        if(!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetInt("Music",1);
        if(PlayerPrefs.HasKey("Efx")) 
            PlayerPrefs.SetInt("Efx",1);
        Music();
    }

    public void Music()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            MusicSource.clip = MusicClip[MusicIndex];
            MusicIndex++;
            if (MusicIndex == MusicClip.Length)
                MusicIndex = 0;
            MusicSource.Play();
        }
        else MusicSource.Stop();
    }

    public void Efx(string effect)
    {
        if (PlayerPrefs.GetInt("Efx") == 1)
        {
            switch (effect)
            {
                case "Exposion":
                    EfxSource.Play();
                    break;
                case "Charge_Coin":
                    EfxSource.Play();
                    break;
                case "Charge_Fuel":
                    EfxSource.Play();
                    break;
                case "Click":
                    EfxSource.Play();
                    break;
                case "NewRecord":
                    EfxSource.Play();
                    break;
                case "Viewlaboratory":
                    EfxSource.Play();
                    break;
            }
        }
    }
}
