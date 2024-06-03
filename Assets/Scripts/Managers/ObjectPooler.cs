using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    [System.Serializable]
    public class Pool
    {
        public string name;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        Instance = this;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.name, objectPool);
        }
    }

    public GameObject SpawnFromPool(string name, Vector3 position, Quaternion rotation, PlayerController playerController = null)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            Debug.LogWarning("Pool with name " + name + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[name].Dequeue();
        objectToSpawn.transform.SetPositionAndRotation(position, rotation);

        if (name == NameConstants.Portal)
        {
            InitPortal(objectToSpawn, playerController);
        }

        if(!objectToSpawn.activeSelf)
        {
            objectToSpawn.SetActive(true);
        }                        
        poolDictionary[name].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    void InitPortal(GameObject portal, PlayerController playerController) 
    {
        // Deactivate the PortalAnimator to prevent OnEnable from being called
        PortalAnimator portalAnimator = portal.GetComponent<PortalAnimator>();
        portalAnimator.enabled = false;

        SpriteRenderer portalSR = portal.GetComponent<SpriteRenderer>();        
        portalSR.sprite = Resources.Load<Sprite>(Mapping.PortalColorToSprite[playerController.color]); 

        portalAnimator.enabled = true;
        portalAnimator.color = playerController.color;
        portal.SetActive(true);
        portalAnimator.AnimatePortal();
    }

}
