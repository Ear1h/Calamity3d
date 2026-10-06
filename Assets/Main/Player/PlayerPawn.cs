using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerPawn: Actor
{
    // Player Movement

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
    
    // Player Regeneration 

    [SerializeField] private float  RegenerationDelay = 1f;
    [SerializeField] private int    RegenerationLimit = 25;
    [SerializeField] private int    RegenerationStep  = 2;

    [SerializeField] private bool   AllowRegeneration = true;

    private float RegenRate = 0.0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cahrCon = GetComponent<CharacterController>();
    }

    public void Update()
    {
        PlayerRegeneration();
        PlayerThink();
    }

    private void PlayerRegeneration()
    {
        if (Time.time < RegenRate)
            return;

        RegenRate = Time.time + RegenerationDelay;

        if (AllowRegeneration)
            GiveBody(RegenerationStep, RegenerationLimit);
    }

    private void PlayerThink()
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
            Jump(JumpSpeed);
        }

        if (!cahrCon.isGrounded)
        {
            MoveDirection.y -= EarthGravity * Time.deltaTime;
        }


        cahrCon.Move(MoveDirection * Time.deltaTime);

        z = 0;
        x = 0;
    }

    public void Jump(float jump)
    {
        if (cahrCon.isGrounded)
        {
            MoveDirection.y = jump;
        }
    }

    public void Stop()
    {
        MoveDirection = Vector3.zero;
    }

    public override int DamageActor(int damage)
    {
        return base.DamageActor(damage);  
    }
}