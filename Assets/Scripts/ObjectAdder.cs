using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ObjectAdder : MonoBehaviour
{
    [Serializable]
    public struct DictionaryEntry {
        public string key;
        public GameObject obj;
    }
    public Dropdown dropdown;
    public  DictionaryEntry[] dict;
    public void OnAddButtonClick(GameObject prefab)
    {
        string selectedText = dropdown.options[dropdown.value].text;
        GameObject selectedPrefab = Array.Find<DictionaryEntry>(dict, el => (el.key == selectedText)).obj;
        if (selectedPrefab) {
            Instantiate(selectedPrefab, new Vector3(UnityEngine.Random.Range(0, 2f),0,UnityEngine.Random.Range(0, 2f)), selectedText != "Character" ? Quaternion.LookRotation(new Vector3(0, 1f, 0), new Vector3(1f, 0, 0)) : Quaternion.identity); 
        }
    }
}
