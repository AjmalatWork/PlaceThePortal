using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.PlayMainMenuMusic();
    }
}
