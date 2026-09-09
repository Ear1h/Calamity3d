using UnityEngine;

public class Stimpack : HealthItem
{
    private void OnEnable()
    {
        PickupMessage = "You pickup a Stimpack";
        Amount = 10;
        PickupMessageDelay = 3.0f;
    }
}