using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //  Variables that determine the outline color
    [HideInInspector]
    public SpacialInference.Side Left_Right;
    [HideInInspector]
    public SpacialInference.Side Front_Back;
    
    //  Select shader for outlines in editor
    [Header("Shader For Outlines")]
    public Shader outline;
    
    //  Shaders and their properties
    private Shader original;
    private Renderer current;
    private static readonly int OutlineWidth = Shader.PropertyToID("_OutlineWidth");
    private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");

    void Start()
    {
        original = GetComponent<Renderer>().material.shader;
        current = GetComponent<Renderer>();
    }

    void Update()
    {
        if (Left_Right == SpacialInference.Side.None && Front_Back == SpacialInference.Side.None)
        {
            current.material.shader = original;
        }
        else
        {
            current.material.shader = outline;
            if (SpacialInference.showFrontBack)
            {
                if (Front_Back == SpacialInference.Side.Front)
                {
                    current.material.SetColor(OutlineColor,Color.magenta);
                }
                else if (Front_Back == SpacialInference.Side.Back)
                {
                    current.material.SetColor(OutlineColor,Color.cyan);
                }
            }
            else
            {
                if (Left_Right == SpacialInference.Side.Right)
                {
                    current.material.SetColor(OutlineColor,Color.red);
                }
                else if (Left_Right == SpacialInference.Side.Left)
                {
                    current.material.SetColor(OutlineColor,Color.blue);
                }
            }
            
            current.material.SetFloat(OutlineWidth,0.05f);
        }
    }
}
