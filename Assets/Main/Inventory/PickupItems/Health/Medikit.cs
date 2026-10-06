using UnityEngine;

public class Medikit : HealthItem
{
    private void OnEnable()
    {
        PickupMessage = "You pickup a Medikit";
        Amount = 25;
        LowMessage = (25, "You are low on health!");
        PickupMessageDelay = 3.0f;
    }
}