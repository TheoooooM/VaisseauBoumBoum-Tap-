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
        if(currentGamepad == null) return;
        if (isRumbling) return;
        isRumbling = true;
        if(rumble)currentGamepad.SetMotorSpeeds(rumble.lowRumble, rumble.highRumble);
        await Task.Delay((int)(rumble.duration * 1000));
        
        StopRumble();
    }

    public void OverrideRumble(RumbleScriptable rumble)
    {
        StopRumble();
        RumbleConstant(rumble);
    }

    public void StopRumble()
    {
        if(currentGamepad == null) return;
        Gamepad.current.SetMotorSpeeds(0, 0);
        isRumbling = false;
    }
    
    
}
