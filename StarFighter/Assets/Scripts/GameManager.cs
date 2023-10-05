using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public RumbleManager rumbleManager;
    public CameraShake cameraShakeManager;
    private void Awake()
    {
        instance = this;
    }
}
