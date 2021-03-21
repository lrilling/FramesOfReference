using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpacialInference : MonoBehaviour
{
    //  Types of directions objects can be in
    public enum Side
    {
        None,
        Left,
        Right,
        Front,
        Back
    }

    /*
     *  Types of spacial inference
     *  Intrinsic - a binary system in which object location is defined in relation to another object
     *  Relative - a ternary system, in which object’s location is determined relatively to another object and viewport
     */
    public enum InferenceMode
    {
        None,
        Intrinsic,
        Relative
    }
    
    [Header("Flags")]
    public InferenceMode InferenceModeOption;
    //  If true displays front or back objects in magenta/cyan; if false - left/right in blue/red;
    public static bool showFrontBack;
    //  If true displays results of inference in textual form in application's viewport
    public bool ShowTextOutput;
    //  If true enables character movement. Used to change displayed GUI elements and "speed" in Animator components.
    public bool isMoving;
    
    [Header("Animator References")]
    //  Reference to the root object for character movement in scene
    public Animator RootAnimatorReference;
    //  Reference to the character model for walking animation (in place)
    public Animator CharacterAnimatorReference;
    
    [Header("Viewpoints in the scene")]
    //  Cached viewpoint object for relative frame of reference
    private GameObject CurrentViewpoint;
    //  Reference to the first viewpoint object in the scene
    public GameObject ViewpointA;
    //  Reference to the second viewpoint object in the scene
    public GameObject ViewpointB;
    //  Reference to a "reference" gameobject in regard to which the relative frame of reference is applied
    public GameObject ReferenceObject;
    
    [Header("UI Elements")]
    //  GUI element to which the result of spacial inferencing is fed 
    public Text UITextOutputField;
    
    /*
     *  List of gameobjects with Item component in the scene. 
     *  To include a new object in the list make sure it has Item component and is on Item raycast layer in Inspector.
     */
    private List<Item> _items;
    
    
    void Start()
    {
        isMoving = true;
        _items = new List<Item>();
        foreach (Item item in FindObjectsOfType<Item>())
        {
            _items.Add(item);
        }

        CurrentViewpoint = ViewpointA;
    }

    void Update()
    {
        switch (InferenceModeOption)
        {
            case InferenceMode.Intrinsic:
                IntrinsicFrameInference();
                break;
            case InferenceMode.Relative:
                RelativeFrameInference(CurrentViewpoint);
                break;
            case InferenceMode.None:
                if(isMoving) Clear();
                break;
        }
    }

    public void Clear()
    {
        foreach (var item in _items)
        {
            item.Front_Back = Side.None;
            item.Left_Right = Side.None;
        }
    }

    public void RelativeFrameInference(GameObject Viewpoint)
    {
        UITextOutputField.text = "";
        foreach (var item in _items)
        {
            Vector3 positionViewpoint = Viewpoint.transform.position;
            Vector3 positionItem = item.transform.position;
            Vector3 positionReference = ReferenceObject.transform.position;
            // Left/Right
            Vector3 viewpointToItem = positionItem - positionViewpoint;
            Vector3 forward = positionReference - positionViewpoint;
            Vector3 up = ReferenceObject.transform.up;
            // Front/Back
            Vector3 referenceToItem = positionItem - positionReference;
            Vector3 referenceToViewpoint = positionViewpoint - positionReference;
            
            String f_b = DetermineFrontBack(item, referenceToItem, referenceToViewpoint);
            String l_r = DetermineLeftRight(item, viewpointToItem, forward, up);
            String result = "Object " + item.name + " is " + f_b + " and " + l_r + " relatively to object " +
                            ReferenceObject.name + '\n';
            UITextOutputField.text += result;
            Debug.Log(result);
            // Debug.DrawLine(positionViewpoint,positionReference,Color.yellow);
        }
    }
    
    public void IntrinsicFrameInference()
    {
        UITextOutputField.text = "";
        foreach (var item in _items)
        {
            Vector3 referenceToItem = item.transform.position - ReferenceObject.transform.position;
            //  IN THE DEMO CHARACTER ROOT WAS ANIMATED WITH FORWARD DIRECTION BEING X AXIS. THUS USAGE OF "RIGHT", not "FORWARD"
            Vector3 forward = ReferenceObject.transform.right;
            Vector3 up = ReferenceObject.transform.up;
            
            String f_b = DetermineFrontBack(item, referenceToItem, forward);
            String l_r = DetermineLeftRight(item, referenceToItem, forward, up);
            String result = "Object " + item.name + " is " + f_b + " and " + l_r + " relatively to object " +
                            ReferenceObject.name + '\n';
            UITextOutputField.text += result;
            Debug.Log(result);
        }
    }

    public String DetermineLeftRight(Item obj, Vector3 targetDir, Vector3 forward, Vector3 up)
    {
        Vector3 perpendicular = Vector3.Cross(up.normalized, targetDir.normalized);
        float direction = Vector3.Dot(perpendicular.normalized, forward.normalized);
        if (direction > 0f)
        {
            obj.Left_Right = Side.Left;
            return "left";
        }
        if (direction < 0f)
        {
            obj.Left_Right = Side.Right;
            return "right";
        }
        return "in front or behind";
    }

    public String DetermineFrontBack(Item obj, Vector3 targetDir, Vector3 forwardDir)
    {
        forwardDir.Normalize();
        targetDir.Normalize();
        float dot = Vector3.Dot(targetDir, forwardDir);
        if (dot > 0f)
        {
            obj.Front_Back = Side.Front;
            return "front";
        }

        if (dot < 0f)
        {
            obj.Front_Back = Side.Back;
            return "back";
        }
        return "";
    }

    public void setShaderMode(Dropdown options)
    {
        if (options.value == 1)
        {
            showFrontBack = true;
        }
        else if(options.value == 0)
        {
            showFrontBack = false;
        }
    }

    public void setViewpoint(Dropdown Viewpoint)
    {
        if (Viewpoint.value == 0)
        {
            CurrentViewpoint = ViewpointA;
        }
        else if(Viewpoint.value == 1)
        {
            CurrentViewpoint = ViewpointB;
        }
    }

    public void showText(Toggle toggle)
    {
        ShowTextOutput = toggle.isOn;
    }
    
    public void setMovement(Toggle toggle)
    {
        isMoving = toggle.isOn;
        RootAnimatorReference.speed = toggle.isOn?1f:0f;
        CharacterAnimatorReference.speed = toggle.isOn?1f:0f;
    }

    public void setInferenceMode(Dropdown option)
    {
        switch (option.value)
        {
            case 0:
                InferenceModeOption = InferenceMode.None;
                break;
            case 1:
                InferenceModeOption = InferenceMode.Intrinsic;
                break;
            case 2:
                InferenceModeOption = InferenceMode.Relative;
                break;
        }
    }
}
