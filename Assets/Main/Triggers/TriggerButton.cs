using UnityEngine;

public class ButtonTrigger : MonoBehaviour
{
    [SerializeField]
    private Button btn;

    private ButtonInfo Info;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<CharacterController>(out CharacterController controller))
        {
            btn.ButtonTouchTrigger(other);
        }
    }
}

