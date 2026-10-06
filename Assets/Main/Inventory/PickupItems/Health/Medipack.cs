using UnityEngine;

public class Medipack : HealthItem
{
    private void OnEnable()
    {
        PickupMessage = "You pickup a Medipack";
        Amount = 50;
        LowMessage = (50, "You need that Medipack, Now!");
        PickupMessageDelay = 5.0f;
    }
}