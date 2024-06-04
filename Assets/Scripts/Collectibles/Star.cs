using System;
using UnityEngine;

public class Star : MonoBehaviour, IResetable, ICollectible
{
    Vector3 originalPosition;
    [NonSerialized] public Renderer starRenderer;
    public ParticleSystem collectionEffect;
    AudioSource starAudio;

    private void Awake()
    {
        starRenderer = gameObject.GetComponent<Renderer>();
        starAudio = gameObject.GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        PlayButtonController.Instance.RegisterResetableObject(this);
    }

    private void OnDisable()
    {
        PlayButtonController.Instance.UnregisterResetableObject(this);
    }

    public void GetOriginalState()
    {        
        originalPosition = transform.position;
    }

    public void SetOriginalState()
    {
        transform.position = originalPosition;
        starRenderer.enabled = true;
    }

    public void OnCollect(int currentStarCount)
    {
        starRenderer.enabled = false;
        ParticlePooler.Instance.SpawnFromPool(NameConstants.StarBurst, transform.position, Quaternion.identity);
        AudioManager.instance.PlayStarCollectSound(currentStarCount, starAudio);
    }
}
