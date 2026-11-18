using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    private DoorController Door;

    private DoorControllerInfo Info;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CharacterController>(out CharacterController controller))
        {
            Door.SetRange(true);
            Door.DoorTouchTrigger(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<CharacterController>(out CharacterController controller))
        {
            Door.SetRange(false);
            Door.DoorTouchTrigger(other);
        }
    }
} 

