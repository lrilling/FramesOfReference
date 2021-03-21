using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLookAt : MonoBehaviour
{
    private Camera _camera;
    void Start()
    {
        _camera = Camera.main;
    }

    void LateUpdate()
    {
        transform.LookAt(_camera.transform, transform.up);
        transform.rotation = Quaternion.LookRotation(_camera.transform.forward);
        var eulerAngles = transform.eulerAngles;
        eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, 0f);
        transform.eulerAngles = eulerAngles;
    }
}
