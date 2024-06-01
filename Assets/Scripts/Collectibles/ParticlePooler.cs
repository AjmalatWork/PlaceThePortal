using System.Collections.Generic;
using UnityEngine;

public class ParticlePooler : MonoBehaviour, IResetable
{
    public static ParticlePooler Instance;

    [System.Serializable]
    public class Pool
    {
        public string name;
        public ParticleSystem prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<ParticleSystem>> poolDictionary;

    void Awake()
    {
        Instance = this;

        poolDictionary = new Dictionary<string, Queue<ParticleSystem>>();

        foreach (Pool pool in pools)
        {
            Queue<ParticleSystem> objectPool = new Queue<ParticleSystem>();

            for (int i = 0; i < pool.size; i++)
            {
                ParticleSystem obj = Instantiate(pool.prefab);
                obj.gameObject.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.name, objectPool);
        }
    }

    private void OnEnable()
    {
        PlayButtonController.Instance.RegisterResetableObject(this);
    }

    private void OnDisable()
    {
        PlayButtonController.Instance.UnregisterResetableObject(this);
    }

    public ParticleSystem SpawnFromPool(string name, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(name))
        {
            Debug.LogWarning("Pool with name " + name + " doesn't exist.");
            return null;
        }

        ParticleSystem objectToSpawn = poolDictionary[name].Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.gameObject.SetActive(true);

        objectToSpawn.Play();

        poolDictionary[name].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public void GetOriginalState()
    {
        return;
    }

    public void SetOriginalState()
    {
        foreach (var pool in poolDictionary)
        {
            foreach (var particleSystem in pool.Value)
            {
                particleSystem.Stop();
                particleSystem.Clear();
            }
        }
    }
}
