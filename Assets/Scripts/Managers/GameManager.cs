using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Dictionary<string, List<GameObject>> objectDictionary = new(); 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterGameObject(GameObject obj)
    {
        // Check if the object is not null
        if (obj == null) return;

        // Check if the object's tag exists in the dictionary
        if (!objectDictionary.ContainsKey(obj.tag))
        {
            // If the tag doesn't exist, create a new list for it
            objectDictionary[obj.tag] = new List<GameObject>();
        }

        // Add the object to the list associated with its tag
        objectDictionary[obj.tag].Add(obj);
    }

    // Method to remove a GameObject from the object dictionary
    public void DeregisterGameObject(GameObject obj)
    {
        if (obj != null && objectDictionary.ContainsKey(obj.tag))
        {
            // Remove the object from the list associated with its tag
            objectDictionary[obj.tag].Remove(obj);
        }
    }

    public List<GameObject> GetObjectsByTag(string tag)
    {
        // Check if the tag exists in the dictionary
        if (objectDictionary.ContainsKey(tag))
        {
            // Return the list of GameObjects associated with the tag
            return objectDictionary[tag];
        }
        else
        {
            // If the tag doesn't exist, return an empty list
            Debug.LogWarning("No objects found with tag " + tag + " in GameManager.");
            return new List<GameObject>();
        }
    }
}

