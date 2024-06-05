using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip mainMenuMusic;
    public AudioClip levelMusic;
    public AudioClip[] starCollect;
    public AudioClip[] SFXList;
    private AudioSource audioSource;

    //SFXList must contain the entries in following order:
    //0   LevelEndSFX
    //1   TeleportSFX
    //2   PortalPlacementSFX
    private void Awake()
    {
        // Ensure this object persists across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMainMenuMusic()
    {
        audioSource.clip = mainMenuMusic;
        audioSource.Play();
    }

    public void PlayLevelMusic()
    {
        if(audioSource.isPlaying && audioSource.clip == levelMusic)
        {
            // keep level music playing
            return;
        }
        audioSource.clip = levelMusic;
        audioSource.Play();
    }

    public void PlayClip(AudioSource audio, int clipIndex)
    {
        if (audioSource.isPlaying && audioSource.clip == levelMusic)
        {
            audioSource.volume /= 2;
            audio.clip = SFXList[clipIndex];
            audio.Play();
            audioSource.volume *= 2;
        }
    }

    public void PlayStarCollectSound(int currentStarCount, AudioSource starAudio)
    {
        starAudio.clip = starCollect[currentStarCount];
        starAudio.Play();
    }
}
