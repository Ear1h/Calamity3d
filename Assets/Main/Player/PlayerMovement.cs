using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // Mouselook Speed Rotation
    public float HorizontalSpeed = 2.0f;
    public float VerticalSpeed = 2.0f;

    // WASD Speed
    public float MovementSpeed = 3.0f;
    public float JumpSpeed = 3.0f;

    // Y look
    public float LookLimit = 45.0f;

    public float EarthGravity = 9.8f;

    Vector3 MoveDirection = Vector3.zero;
    float AxisY = 0;

    public GameObject LookObject;

    private CharacterController cahrCon;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cahrCon = GetComponent<CharacterController>();
    }

    void Update()
    {
        float x = 0;
        float z = 0;

        AxisY += -Input.GetAxis("Mouse Y") * VerticalSpeed;
        AxisY = Mathf.Clamp(AxisY, -LookLimit, LookLimit);
        LookObject.transform.rotation = Quaternion.Euler(AxisY, LookObject.transform.rotation.eulerAngles.y, LookObject.transform.rotation.eulerAngles.z);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * HorizontalSpeed, 0);

        if (Input.GetKey(KeyCode.A))
        {
            x = -MovementSpeed;
        }

        else if (Input.GetKey(KeyCode.D))
        {
            x = MovementSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            z = -MovementSpeed;
        }

        else if (Input.GetKey(KeyCode.W))
        {
            z = MovementSpeed;
        }

        Vector3 newMove = transform.TransformDirection(Vector3.forward) * z + transform.TransformDirection(Vector3.right) * x;
        MoveDirection = new Vector3(newMove.x, MoveDirection.y, newMove.z);

        if (Input.GetKeyDown(KeyCode.Space) && cahrCon.isGrounded)
        {
            MoveDirection.y = JumpSpeed;
        }

        if (!cahrCon.isGrounded)
        {
            MoveDirection.y -= EarthGravity * Time.deltaTime; //GANDON
        }


        cahrCon.Move(MoveDirection * Time.deltaTime);

        z = 0;
        x = 0;
    }
}