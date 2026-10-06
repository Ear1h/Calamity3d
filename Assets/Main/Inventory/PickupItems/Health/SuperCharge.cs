using UnityEngine;

public class Supercharge: HealthItem
{
    public void OnEnable()
    {
        PickupMessage = "Supercharge!";
        AmountCounter = (100, 200);
        PickupMessageDelay = 3.0f;
        intFlags |= ItemFlagsInternal.AlwaysPickup;
    }
}