using UnityEngine;
using System.Collections;
using TMPro;

public class MainInventory : MonoBehaviour
{
    [SerializeField] protected PickupMessageUI PickupMessageUI;
    protected string PickupMessage;
    protected int    Amount;
    protected float  PickupMessageDelay;
    // public virtual bool Use(bool Pickup) { return true; }
    public virtual void StringMessage()
    {
        if (PickupMessageUI != null)
        {
            PickupMessageUI.ShowMessage(
                PickupMessage,
                PickupMessageDelay
            );
        }
    }
}