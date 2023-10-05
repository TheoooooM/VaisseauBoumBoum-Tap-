using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public RumbleManager rumbleManager;
    public CameraShake cameraShakeManager;
    public Volume volumeManager;

    
    private void Awake()
    {
        instance = this;
    }
}
