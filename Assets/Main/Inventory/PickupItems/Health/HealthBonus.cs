using UnityEngine;

public class HealthBonus : HealthItem
{
    public void OnEnable()
    {
        PickupMessage = "HealthBonus!";
        AmountCounter = (1, 200);
        PickupMessageDelay = 1.0f;

        intFlags |= ItemFlagsInternal.AlwaysPickup;
    }
}