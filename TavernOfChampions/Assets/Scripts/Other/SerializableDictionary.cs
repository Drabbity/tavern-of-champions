using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SerializableStringDictionary<TValue> : Dictionary<string, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<KeyValue> _keyValue;

    public void OnBeforeSerialize()
    {
        _keyValue.Clear();

        foreach (KeyValuePair<string, TValue> pair in this)
        {
            _keyValue.Add(new KeyValue(pair.Key, pair.Value));
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        foreach (var keyValue in _keyValue)
        {
            while (this.ContainsKey(keyValue.Key))
                keyValue.Key += "*";

             this.Add(keyValue.Key, keyValue.Value);
        }
    }

    [Serializable]
    private class KeyValue
    {

        public string Key;
        public TValue Value;
        public KeyValue(string key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}