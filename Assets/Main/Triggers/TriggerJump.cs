using UnityEngine;
using UnityEngine.EventSystems;

public class TriggerJump : TriggerCore
{
    [SerializeField]
    private float PowerJump = 20;

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

        PlayerMovement movement = player.GetComponent<PlayerMovement>();

        if (movement == null)
        {
            return false;
        }

        movement.Jump(PowerJump);

        return true;
    }
}
