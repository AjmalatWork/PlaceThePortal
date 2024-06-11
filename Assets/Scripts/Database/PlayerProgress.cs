using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    // Save the stars collected for a specific level
    public static void SaveStarsForLevel(int level, int stars)
    {
        string key = GetKey(level);
        PlayerPrefs.SetInt(key, stars);
        PlayerPrefs.Save();
    }

    // Get the stars collected for a specific level
    public static int GetStarsForLevel(int level)
    {
        string key = GetKey(level);
        return PlayerPrefs.GetInt(key, 0); 
    }

    // Get the key for PlayerPrefs
    private static string GetKey(int level)
    {
        return $"Level_{level}_Stars";
    }
}
