using System;
using UnityEngine;

public class TriggerCore: Thinker
{
    [Flags]
    public enum TriggerFlagsPublic
    {
        None = 0,
        StartOff = 1 << 0,
        Once = 1 << 1,
    }

    [SerializeField] protected TriggerFlagsPublic flags;

    protected bool TriggerEnabled;
    protected virtual void Awake()
    {
        TriggerEnabled = !flags.HasFlag(TriggerFlagsPublic.StartOff);
    }

    protected virtual void Start()
    {
        Use += TriggerUse;
    }

    protected virtual void TriggerUse(Thinker self, Thinker other, Thinker activator)
    {
        TriggerEnabled = (!TriggerEnabled);
    }
    virtual protected bool CanTouchTrigger(Actor toucher)
    {
        return TriggerEnabled && toucher != null;
    }

    private void OnTriggerStay(Collider other)
    {
        Actor actor = other.GetComponent<Actor>();
        if (!CanTouchTrigger(actor))
        {
            return;
        }
        // If True && Has flag Once, destroy then
        if (flags.HasFlag(TriggerFlagsPublic.Once)) 
        {
            FreeEdict(this);
        }
    }
}