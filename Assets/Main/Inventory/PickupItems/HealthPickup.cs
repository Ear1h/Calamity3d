using UnityEngine;

public class HealthItem: MainInventory
{
    protected string LowHealthMessage;
    protected int LowHealth;

    public (int LowHealth, string LowHealthMessage) LowMessage
    {
        get => (LowHealth, LowHealthMessage);
        set => (LowHealth, LowHealthMessage) = value;
    }

    private int PrevHealth;

    public override string StringMessage()
    {
        if (PrevHealth < LowMessage.LowHealth)
        {
            PickupMessage = LowMessage.LowHealthMessage;
            if (PickupMessage != null)
            {
                return PickupMessage;
            }
        }

        return base.StringMessage();
    }
    public override bool TryPickup(Actor toucher, bool pickup = false)
    {
        PlayerPawn player = toucher as PlayerPawn;

        if (player == null)
            return false;

        PrevHealth = player.Health;

        if (player.GiveBody(Amount, MaxAmount))
        {
            StringMessage();
            return true;
        }

        if (intFlags.HasFlag(ItemFlagsInternal.AlwaysPickup))
        {
            StringMessage();
            return true;
        }

        return false;
    }
}
