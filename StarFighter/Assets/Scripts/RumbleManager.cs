using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleManager : MonoBehaviour
{
    public bool isRumbling;
    public Gamepad currentGamepad = Gamepad.current;
    
    public async void RumbleConstant(RumbleScriptable rumble)
    {
        if (isRumbling) return;
        isRumbling = true;
        currentGamepad.SetMotorSpeeds(rumble.lowRumble, rumble.highRumble);
        await Task.Delay((int)(rumble.duration * 1000));
        
        StopRumble();
    }

    public void StopRumble()
    {
        Gamepad.current.SetMotorSpeeds(0, 0);
        isRumbling = false;
    }
    
    
}
