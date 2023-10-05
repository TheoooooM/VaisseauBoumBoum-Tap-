using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RumbleSO", order = 1)]
public class RumbleScriptable : ScriptableObject
{
    public float lowRumble, highRumble, duration;
}
