using Unity.VisualScripting;
using UnityEngine;

public class HealthItem: MainInventory
{
    protected string LowHealthMessage;
    protected int LowHealth;
    private int PrevHealth;

    public override void StringMessage()
    {
        if (PrevHealth < LowHealth)
        {
            PickupMessage = LowHealthMessage;
        }

        base.StringMessage();
    }
    public void OnTriggerEnter(Collider other)
    {
        PlayerPawn player = other.GetComponent<PlayerPawn>();
        if (player != null && player.currenthealth > 0)
        {
            PrevHealth = player.currenthealth;
            player.TakeDamage(-(Amount));
            StringMessage();
            Destroy(gameObject);
        }
    }
}
