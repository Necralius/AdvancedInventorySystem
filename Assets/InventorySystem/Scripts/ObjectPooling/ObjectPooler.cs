using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    //Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
    //ObjectPooler - Code Update Version 0.4 - (Refactored code).
    //Feel free to take all the code logic and apply in yours projects.
    //This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

    #region - Singleton Pattern -
    //This statement  a simple Singleton Pattern implementation
    public static ObjectPooler Instance;
    private void Awake() => Instance = this;
    #endregion

    #region - General Game Pools -
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    #endregion

    #region - Object Pooler Intatiation -
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
    #endregion

    #region - Main Objet Pooler Objet Gathering -
    public GameObject SpawnFromPool(string poolTag, Vector3 position, Quaternion rotation)//This Method get an object from the pool and set it as active, an set the object in a new position and rotation
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

        objToSpawn.GetComponent<IPooled>().OnObjectspawn();//This statement calls the OnObjectSpawn Method that all pooled object has by default on the interface IPooled

        poolDictionary[tag].Enqueue(objToSpawn);
        return objToSpawn;
    }
    #endregion
}

#region - Pool Structure -
[Serializable]
public struct Pool//This Struct represent the default pool structure
{
    public string tag;
    public GameObject prefab;
    public int size;
}
#endregion