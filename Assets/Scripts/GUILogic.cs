using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GUILogic : MonoBehaviour
{
    [Header("Reference to Spacial Inference Gameobject")]
    public SpacialInference SpacialInferenceReference;

    [Header("Dropdowns")]
    public Dropdown FrameOfReferenceMode;
    
    [Header("UI Elements Groups")]
    public GameObject StaticSceneInference;
    public GameObject DynamicSceneInference;

    
    
    public Text OutputText;

    void Start() 
    {
    }
   
    void Update()
    {
    //     if (SpacialInferenceReference.isMoving)
    //     {
    //         StaticSceneInference.SetActive(false);
    //         DynamicSceneInference.SetActive(true);
    //     }
    //     else
    //     {
    //         StaticSceneInference.SetActive(true);
    //         DynamicSceneInference.SetActive(false);
    //         FrameOfReferenceMode.value = 0;
    //     }

    //     OutputText.enabled = (SpacialInferenceReference.ShowTextOutput && !SpacialInferenceReference.isMoving) ||
    //                          (SpacialInferenceReference.InferenceModeOption != SpacialInference.InferenceMode.None &&
    //                           SpacialInferenceReference.ShowTextOutput && SpacialInferenceReference.isMoving);
    }
}
