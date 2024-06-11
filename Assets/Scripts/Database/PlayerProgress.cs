using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    public static int noOfLevels = 12;

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

    public static int GetTotalStars()
    {
        int totalStars = 0 ;
        for (int i = 1; i<= noOfLevels; i++)
        {
            int stars = GetStarsForLevel(i);
            totalStars += stars;
        }
        return totalStars;
    }
}
