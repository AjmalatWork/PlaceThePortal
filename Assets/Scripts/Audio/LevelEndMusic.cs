using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndMusic : MonoBehaviour
{
    AudioSource levelEndAudio;

    private void Awake()
    {
        levelEndAudio = GetComponent<AudioSource>();
    }
    void OnEnable()
    {
        AudioManager.instance.PlayClip(levelEndAudio, SFX.LevelEnd);
    }
}
