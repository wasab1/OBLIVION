using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> objects;
    [SerializeField] public List<float> timer;

    public float time = 0f;
    void Start()
    {
    }

    void Update()
    {
        time += Time.deltaTime;
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i]!=null && time > timer[i])
            {
                objects[i].SetActive(true);
            }
        }
        for (int i = 0; i < objects.Count; i++) {
            if (objects[i]!=null && objects[i].activeSelf)
            {
                objects.RemoveAt(i);
                timer.RemoveAt(i);
            }
        }
    }
}

// using UnityEngine;
// using System;   
// using System.Collections.Generic;

// public class scenev2 : MonoBehaviour, ISerializationCallbackReceiver
// {
//     public List<int> _keys = new List<int> { 3, 4, 5 };
//     public List<string> _values = new List<string> { "I", "Love", "Unity" };

//     //Unity doesn't know how to serialize a Dictionary
//     public Dictionary<int, string> _myDictionary = new Dictionary<int, string>();

//     public void OnBeforeSerialize()
//     {
//         _keys.Clear();
//         _values.Clear();

//         foreach (var kvp in _myDictionary)
//         {
//             _keys.Add(kvp.Key);
//             _values.Add(kvp.Value);
//         }
//     }

//     public void OnAfterDeserialize()
//     {
//         _myDictionary = new Dictionary<int, string>();

//         for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
//             _myDictionary.Add(_keys[i], _values[i]);
//     }

//     void OnGUI()
//     {
//         foreach (var kvp in _myDictionary)
//             GUILayout.Label("Key: " + kvp.Key + " value: " + kvp.Value);
//     }
// }