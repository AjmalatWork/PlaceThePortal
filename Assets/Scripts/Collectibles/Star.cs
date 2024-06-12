using System;
using System.Collections;
using UnityEngine;

public class Star : MonoBehaviour, IResetable, ICollectible
{
    Vector3 originalPosition;
    [NonSerialized] public Renderer starRenderer;
    public ParticleSystem collectionEffect;
    AudioSource starAudio;

    private readonly float floatAmplitude = 0.1f;
    private readonly float floatFrequency = 1f;
    private readonly float rotationSpeed = 5f;

    private Vector3 startPos;
    private float phaseOffset; 

    private void Awake()
    {
        starRenderer = gameObject.GetComponent<Renderer>();
        starAudio = gameObject.GetComponent<AudioSource>();

        startPos = transform.position;
        phaseOffset = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
        transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));
        StartCoroutine(StarFloat());
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
        StartCoroutine(StarFloat());
    }

    public void OnCollect(int currentStarCount)
    {
        starRenderer.enabled = false;
        ParticlePooler.Instance.SpawnFromPool(NameConstants.StarBurst, transform.position, Quaternion.identity);
        AudioManager.instance.PlayStarCollectSound(currentStarCount, starAudio);
        StopCoroutine(StarFloat());
    }

    IEnumerator StarFloat()
    {
        while (starRenderer.enabled)
        {
            float newY = startPos.y + Mathf.Sin(Time.unscaledTime * floatFrequency + phaseOffset) * floatAmplitude;
            transform.position = new Vector3(startPos.x, newY, startPos.z);
            transform.Rotate(Vector3.forward, rotationSpeed * Time.fixedDeltaTime);
            yield return null;
        }
        yield return null;
    }
}
