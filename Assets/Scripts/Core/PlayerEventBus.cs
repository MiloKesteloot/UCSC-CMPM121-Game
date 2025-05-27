using UnityEngine;
using System;

public class PlayerEventBus : EventBus
{
    private static PlayerEventBus theInstance;
    public static new PlayerEventBus Instance
    {
        get
        {
            if (theInstance == null)
                theInstance = new PlayerEventBus();
            return theInstance;
        }
    }

    public event Action OnStandStill; // for one second
    public void DoStandStill()
    {
        OnStandStill?.Invoke();
    }

    public event Action<float> OnMove;
    public void DoMove(float distance)
    {
        OnMove?.Invoke(distance);
    }
    
    public event Action OnKill;
    public void DoKill()
    {
        OnKill?.Invoke();
    }

}
