using UnityEngine;
using System.Collections;

public class TriggerHurt : TriggerCore
{
    [SerializeField]
    private int DamageCount = 20;

    [SerializeField]
    public float DamageRate = 3.0f;


    private float nextDamage = 0.0f;

    protected override bool CanTouchTrigger(Actor toucher)
    {
        if (!base.CanTouchTrigger(toucher))
        {
            return false;
        }
        PlayerPawn player = toucher.GetComponent<PlayerPawn>();

        if (player == null)
        {
            return false;
        }

        if (Time.time < nextDamage)
        {
            return false;
        }

        nextDamage = Time.time + DamageRate;
        if (DamageCount > 0)
        {
            player.DamageActor(DamageCount);
        }

        else
        {
            player.GiveBody(DamageCount, 0);
        }

        return true;
    }
}