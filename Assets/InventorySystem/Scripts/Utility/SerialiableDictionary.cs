using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    // We have a struct of pairs instead of two lists incase the key or value is a list type because Unity can't serialize lists of lists unless they are inside of a class or struct and not directly a list of lists.
    [Serializable]
    private struct SerialiableKeyValuePair
    {
        public TKey key;
        public TValue value;

        public SerialiableKeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }
    }
    [SerializeField] private List<SerialiableKeyValuePair> serializedPairs;
    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        // Basically just copy the code from the docs but instead of putting the pairs in to to lists you just create a new instance of SerialiableKeyValuePair and put it in the one list. And you get the pairs from `this`.
        serializedPairs = new List<SerialiableKeyValuePair>();
        foreach (var kvp in this) serializedPairs.Add(new SerialiableKeyValuePair(kvp.Key, kvp.Value));
    }
    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        // Again same as the docs, but do it with the _serializedPairss instead. And `this` is the dictionary
        this.Clear();

        for (int i = 0; i != serializedPairs.Count; i++)
        {
            if (this.ContainsKey(serializedPairs[i].key))
            {
                TKey[] attempt = new TKey[1] { new string("change the key").ConvertTo<TKey>() };
                this.Add(attempt[0], serializedPairs[i].value);
            }
            else this.Add(serializedPairs[i].key, serializedPairs[i].value);
        }
    }
    void OnGUI()
    {
        foreach (var kvp in this) GUILayout.Label("Key: " + kvp.Key + " value: " + kvp.Value);
    }
}