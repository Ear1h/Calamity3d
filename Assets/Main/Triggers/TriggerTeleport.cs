using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TriggerTeleportDoomStyle : TriggerCore
{
    [SerializeField]
    private Transform TeleportDestination;

    protected override bool CanTouchTrigger(Actor toucher)
    {
        if (!base.CanTouchTrigger(toucher))
        {
            return false;
        }
        var player = toucher.GetComponent<PlayerPawn>();
        if (player == null)
            return false;

        CharacterController controller = player.GetComponent<CharacterController>();

        if (controller != null)
        {
            controller.enabled = false;

            player.transform.position = TeleportDestination.position;
            player.transform.rotation = TeleportDestination.rotation;

            controller.enabled = true;
        }

        return true;
    }
}
