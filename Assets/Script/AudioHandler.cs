using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;



public class AudioHandler : MonoBehaviour
{
    public AudioSource MusicSource, EfxSource;
    public AudioClip MusicClip, ExplosionClip, ChangeSkinClip, ClickClip, goLobby ;
    public AudioClip UpgradeClip, GetChestClip;
    public Coroutine _MusicLoop;

    private void Start()
    {
        Efx("GoLobby");
    }

    public void Music()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            MusicSource.clip = MusicClip;
            MusicSource.Play();
            _MusicLoop = StartCoroutine(MusicLoop());
        }
    }

    IEnumerator MusicLoop()
    {
        yield return new WaitForSeconds(MusicClip.length);
        yield return new WaitForSeconds(10);
        Music();
    }

    public void Efx(string effect)
    {
        if (PlayerPrefs.GetInt("Efx") == 1)
        {
            switch (effect)
            {
                case "Explosion":
                    EfxSource.clip = ExplosionClip;
                    EfxSource.Play();
                    break;
                case "Upgrade":
                    EfxSource.clip = UpgradeClip;
                    EfxSource.Play();
                    break;
                case "Click":
                    EfxSource.clip = ClickClip;
                    EfxSource.Play();
                    break;
                case "GoLobby":
                    EfxSource.clip = goLobby;
                    EfxSource.Play();
                    break;
                case "ChangeSkin":
                    EfxSource.clip = ChangeSkinClip;
                    EfxSource.Play();
                    break;
                case "GetChest":
                    EfxSource.clip = GetChestClip;
                    EfxSource.Play();
                    break;
            }
        }
    }
}
