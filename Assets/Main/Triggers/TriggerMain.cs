using System;
using UnityEngine;
using static MainInventory;

public class TriggerCore: MonoBehaviour
{
    [Flags]
    public enum TriggerFlagsPublic
    {
        None = 0,
        StartOff = 1 << 0,
        Once = 1 << 1,
    }

    [SerializeField] protected TriggerFlagsPublic flags;

    virtual protected bool CanTouchTrigger(Actor toucher)
    {
        if (flags.HasFlag(TriggerFlagsPublic.StartOff))
        {
            return false;
        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Actor actor = other.GetComponent<Actor>();
        if (!CanTouchTrigger(actor))
        {
            return;
        }
        // If True && Has flag Once, destroy then
        if (flags.HasFlag(TriggerFlagsPublic.Once)) 
        {
            Destroy(gameObject);
        }
    }
}