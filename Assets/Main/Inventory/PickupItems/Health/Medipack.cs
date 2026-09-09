using UnityEngine;

public class Medipack : HealthItem
{
    private void OnEnable()
    {
        PickupMessage = "You pickup a Medipack";
        Amount = 50;
        LowHealth = 50;
        LowHealthMessage = "YOU NEED THAT FUCKING SHIT!";
        PickupMessageDelay = 5.0f;
    }
}