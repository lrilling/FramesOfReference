using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RuntimeGizmos;

public class ObjectController : MonoBehaviour
{
    public Button deleteButton;

    private List<GameObject> selected = new List<GameObject>();

    public void Add(Transform transform) {
        GameObject selectedObj = transform.gameObject;
        
        while(selectedObj.tag == "RigPart") {
            selectedObj = transform.parent.gameObject;
        }

        selected.Add(selectedObj);
        deleteButton.gameObject.SetActive(true);

        Debug.Log(selected);
    }

    public void Remove(Transform transform) {
        selected.Remove(transform.gameObject);
        if (selected.Count == 0) {
            deleteButton.gameObject.SetActive(false);
        }
    }

    public void RemoveAll() {
        selected.Clear();
        deleteButton.gameObject.SetActive(false);
    }

    public void HandleDeleteButton() {
        foreach (GameObject obj in selected) {
            Debug.Log(obj);
            obj.SetActive(false);
            Debug.Log(obj.activeSelf);
            DestroyImmediate(obj);
        }
    }
}
