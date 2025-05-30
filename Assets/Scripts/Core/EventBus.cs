using UnityEngine;
using System;
using System.Collections.Generic;

public class EventBus
{
    public List<(Func<bool>, Action)> fixedCustomEvents = new();

    public void Reset()
    {
        fixedCustomEvents = new();
        OnDamage = null;
        OnKill = null;
    }

    private static EventBus theInstance;
    public static EventBus Instance
    {
        get
        {
            if (theInstance == null) theInstance = new EventBus();
            return theInstance;
        }
    }

    public event Action<Vector3, Damage, Hittable> OnDamage;
    public void DoDamage(Vector3 where, Damage dmg, Hittable target)
    {
        OnDamage?.Invoke(where, dmg, target);
    }

    public event Action OnKill;
    public void DoKill()
    {
        OnKill?.Invoke();
    }

    public event Action OnMiss;
    public void DoMiss()
    {
        Debug.Log("Miss!");
        OnMiss.Invoke();
    }

    public void FixedUpdate()
    {
        foreach (var i in fixedCustomEvents)
        {
            if (i.Item1()) i.Item2();
        }
    }
}
