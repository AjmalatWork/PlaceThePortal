using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    public GameObject audioManagerPrefab;
    private void Awake()
    {
        if (AudioManager.instance == null)
        {
            Instantiate(audioManagerPrefab);
        }
    }
    private void Start()
    {
        AudioManager.instance.PlayLevelMusic();
    }
}
