using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraToCanvas : MonoBehaviour
{
    private Canvas canvas;

    private Canvas Canvas => canvas == null ? canvas = GetComponentInChildren<Canvas>() : canvas;
    
    void Start()
    {
        Canvas.worldCamera = Camera.main;
    }
}