using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<KeyValue> _keyValue;

    public void OnBeforeSerialize()
    {
        _keyValue.Clear();

        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            _keyValue.Add(new KeyValue(pair.Key, pair.Value));
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        foreach(var keyValue in _keyValue)
            this.Add(keyValue.Key, keyValue.Value);
    }

    [Serializable]
    private class KeyValue
    {

        public TKey Key;
        public TValue Value;
        public KeyValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}