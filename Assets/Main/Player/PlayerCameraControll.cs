using NUnit;
using UnityEngine;

public class PlayerCameraControll : MonoBehaviour
{
    [SerializeField] private PlayerPawn Player;
    [SerializeField] private GameObject LookObject;

    [SerializeField] private float rotangle;
    [SerializeField] private float rotspeed;
    private Vector3 forward, right, up;
    float AxisY = 0;

    // Mouselook Speed Rotation
    private float HorizontalSpeed = 2.0f;
    private float VerticalSpeed = 2.0f;

    // Y look
    private float LookLimit = 75.0f;
    private float LookDead = 65.0f;

    private float angle;
    private float bob;
    [SerializeField] private float bobtime;
    [SerializeField] private float bobamount;
    private Vector3 playerviewz;


    [SerializeField] private float LandDeflectTime = 0.15f;
    [SerializeField] private float LandReturnTime = 0.3f;

    static float LandTime;
    static float LandChange;
    float LandOffset;

    private void Start()
    {
        playerviewz = transform.localPosition;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void FixedUpdate()
    {
        Vector3 horizontalMove = Player.MoveDirection;
        horizontalMove.y = 0f;
        bool Still = horizontalMove.sqrMagnitude < 0.01f;
        if (Player.PlayerState == PlayerPawn.PlayerStates.PLR_ALIVE)
        {
            if (Still)
            {
                angle += Time.time / (120 * bobtime) * 360;
                bob = Mathf.Sin(angle * Mathf.Deg2Rad) * bobamount;

                angle %= 360f;
            }
            else
            {
                bob = 0;
            }
            
            AxisY += -Input.GetAxis("Mouse Y") * VerticalSpeed;
            AxisY = Mathf.Clamp(AxisY, -LookLimit, LookLimit);
            Player.transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * HorizontalSpeed, 0);
        }

        LookObject.transform.localPosition = playerviewz + Vector3.up * (bob + LandOffset);
        CalcLanding();
        CalcViewRoll();
    }

    private void CalcViewRoll()
    {
        float side = CalcRoll(Player.transform.eulerAngles, Player.MoveDirection);

        LookObject.transform.localRotation = Quaternion.Euler(AxisY, 0f, side);

        if (Player.PlayerState == PlayerPawn.PlayerStates.PLR_DEAD)
        {
            LookObject.transform.localRotation = Quaternion.Euler(AxisY, 0f, LookDead);
            return;
        }
    }

    private float CalcRoll(Vector3 angles, Vector3 velocity)
    {
        float sign;
        float side;
        float value;

        Vector3 right = Player.transform.right;

        side = Vector3.Dot(velocity, right);

        sign = side < 0 ? -1 : 1;
        side = Mathf.Abs(side);

        value = rotangle;

        if (side < rotspeed)
            side = side * value / rotspeed;
        else
            side = value;

        return side * sign;
    }

    static public void StartLanding(float FallSpeed)
    {
        LandTime = Time.time;

        LandChange = -Mathf.Clamp(FallSpeed * 0.02f, 0f, 0.5f);
    }

    private void CalcLanding()
    {
        float delta = Time.time - LandTime;

        if (delta < LandDeflectTime)
        {
            float f = delta / LandDeflectTime;
            LandOffset = LandChange * f;
        }
        else if (delta < LandDeflectTime + LandReturnTime)
        {
            delta -= LandDeflectTime;

            float f = 1f - delta / LandReturnTime;
            LandOffset = LandChange * f;
        }
        else
        {
            LandOffset = 0f;
        }
    }
}