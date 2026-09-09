using UnityEngine;

public class Medikit : HealthItem
{
    private void OnEnable()
    {
        PickupMessage = "You pickup a Medikit";
        Amount = 25;
        LowHealth = 25;
        LowHealthMessage = "YOU NEED THAT FUCKING SHIT!";
        PickupMessageDelay = 3.0f;
    }
}