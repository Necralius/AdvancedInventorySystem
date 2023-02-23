using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    #region - Singleton Pattern -
    public static ObjectPooler Instance;
    private void Awake() => Instance = this;
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(tag, objectPool);
        }
    }
    public GameObject SpawnFromPool(string poolTag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(poolTag))
        {
            Debug.LogWarning("This Pool doent's exists! Pool Tag: " + poolTag);
            return null;
        }

        GameObject objToSpawn = poolDictionary[poolTag].Dequeue();

        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;

        objToSpawn.GetComponent<IPooled>().OnObjectspawn();

        poolDictionary[tag].Enqueue(objToSpawn);
        return objToSpawn;
    }
}
[Serializable]
public class Pool
{
    public string tag;
    public GameObject prefab;
    public int size;
}