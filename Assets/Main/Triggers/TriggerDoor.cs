using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]
    private DoorController Door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CharacterController>(out CharacterController controller))
        {
            if (!Door.isDoorOpen)
            {
                Door.Open(other.transform.position);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<CharacterController>(out CharacterController controller))
        {
            if (Door.isDoorOpen)
            {
                Door.Close();
            }
        }
    }
}