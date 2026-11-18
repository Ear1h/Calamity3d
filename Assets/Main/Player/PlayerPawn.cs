using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

public class PlayerPawn: Actor
{

    // Player States
    public enum PlayerStates
    {
        PLR_ALIVE,
        PLR_DEAD,
    }

    public PlayerStates PlayerState;

    // Player Movement

    // WASD Speed
    public float WalkSpeed;
    public float Accel;
    public float airaccelerate;
    public float cl_Friction;
    public float StopSpeed;
    public float JumpSpeed;

    private float JumpTime;
    private bool JumpHeld;

    private readonly float EarthGravity = 9.8f;

    public Vector3 MoveDirection = Vector3.zero;
    

    private CharacterController cahrCon;
    private Rigidbody rb;
    
    // Player Regeneration 

    [SerializeField] private float  RegenerationDelay = 1f;
    [SerializeField] private int    RegenerationLimit = 25;
    [SerializeField] private int    RegenerationStep  = 2;

    [SerializeField] private bool   AllowRegeneration = true;

    private float RegenRate = 0.0f;

    // Jump

    private float FallSpeed;
    private bool WasGrounded = true;

    static public float DamageCount;
    private readonly float damagecountfactor = 10f;

    private void Start()
    {

        PlayerState = PlayerStates.PLR_ALIVE;
        cahrCon = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        DamageCount = 0;
    }

    public void FixedUpdate()
    {
        PlayerRegeneration();
        PlayerThink();
    }

    virtual protected void PlayerRegeneration()
    {
        if (Time.time < RegenRate)
            return;

        RegenRate = Time.time + RegenerationDelay;

        if (AllowRegeneration && PlayerState != PlayerStates.PLR_DEAD)
            GiveBody(RegenerationStep, RegenerationLimit);
    }

    virtual protected void DeathThink()
    {
        HUD.Instance.StartDeathHeart();
        if (Input.GetKey(KeyCode.E))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (DamageCount > 0)
            DamageCount--;
    }

    virtual protected void PlayerThink()
    {
        if (PlayerState == PlayerStates.PLR_DEAD)
        {
            DeathThink();
            cahrCon.enabled = false;
            rb.isKinematic = false;
            rb.useGravity = true;
            return;
        }

        if (!Input.GetKey(KeyCode.Space))
            JumpHeld = false;

        if (cahrCon.isGrounded)
            WalkMove();
        else
            AirMove();

        /*
        if (OnLadder)
        {
            LadderMove();
            return;
        }
        */

        if (!cahrCon.isGrounded)
        {
            MoveDirection.y -= EarthGravity * Time.deltaTime;
            FallSpeed = Mathf.Max(FallSpeed, -MoveDirection.y);
        }

        cahrCon.Move(MoveDirection * Time.deltaTime);

        if (cahrCon.isGrounded && !WasGrounded)
        {
            if (FallSpeed > 1f)
                PlayerCameraControll.StartLanding(FallSpeed);

            FallSpeed = 0f;
        }

        WasGrounded = cahrCon.isGrounded;

        if (DamageCount > 0)
            DamageCount--;
    }

    public bool Jump(float jump)
    {
        if (cahrCon.isGrounded)
        {
            MoveDirection.y = jump;
            return true;
        }

        return false;
    }

    public void Stop()
    {
        MoveDirection = Vector3.zero;
    }

    public override int DamageActor(int damage, Actor source, GameObject inflictor)
    {
        if (Health <= 0)
            return base.DamageActor(damage, source, inflictor);

        DamageCount += (damage * damagecountfactor);

        if (DamageCount > 1000)
            DamageCount = 1000;

        return base.DamageActor(damage, source, inflictor);

    }

    public override void KillActor(GameObject inflictor, Actor source)
    {
        base.KillActor(inflictor, source);

        PlayerState = PlayerStates.PLR_DEAD;
    }



    private void CalamityFriction()
    {
        Vector3 velocity = MoveDirection;
        velocity.y = 0f;

        float speed = velocity.magnitude;

        if (speed < 1f)
        {
            MoveDirection.x = 0f;
            MoveDirection.z = 0f;
            return;
        }

        float Drop = 0f;

        if (cahrCon.isGrounded && MoveDirection.y <= 0f)
        {
            float control = speed < StopSpeed ? StopSpeed : speed;
            Drop = control * cl_Friction * Time.deltaTime;
        }

        float NewSpeed = speed - Drop;

        if (NewSpeed < 0f)
            NewSpeed = 0f;

        NewSpeed /= speed;

        MoveDirection.x *= NewSpeed;
        MoveDirection.z *= NewSpeed;
    }

    private void CalamityAcceleration(Vector3 WishDir, float WishSpeed, float Accel)
    {
        Vector3 velocity = MoveDirection;
        velocity.y = 0f;

        float CurrentSpeed = Vector3.Dot(velocity, WishDir);
        float AddSpeed = WishSpeed - CurrentSpeed;

        if (AddSpeed <= 0f) return;

        float AccelerationSpeed = Accel * WishSpeed * Time.deltaTime;

        if (AccelerationSpeed > AddSpeed)
            AccelerationSpeed = AddSpeed;


        MoveDirection.x += AccelerationSpeed * WishDir.x;
        MoveDirection.z += AccelerationSpeed * WishDir.z;
    }


    public bool CheckJump()
    {
        if (Time.time - JumpTime < 0.55f)
        {
            return false;
        }

        if (!Input.GetButton("Jump"))
            return false;

        if (JumpHeld)
            return false;

        JumpHeld = true;

        MoveDirection.y = JumpSpeed;
        return true;
    }

    public void AirMove()
    {
        Vector3 WishVel;
        float WishSpeed;

        CalamityFriction();

        float fmove = Input.GetAxisRaw("Vertical");
        float smove = Input.GetAxisRaw("Horizontal");

        Vector3 WishDir = transform.right * smove + transform.forward * fmove;

        WishSpeed = WishDir.magnitude;

        if (WishSpeed > 1f)
        {
            WishDir.Normalize();
            WishSpeed = 1f;
        }

        WishSpeed *= WalkSpeed;
        WishVel = WishDir * WishSpeed;

        CalamityAcceleration(WishDir, WishSpeed, airaccelerate);
    }

    public void WalkMove()
    {
        float WishSpeed;

        float fmove = Input.GetAxisRaw("Vertical");
        float smove = Input.GetAxisRaw("Horizontal");

        if (CheckJump())
        {
            AirMove();
            JumpTime = Time.time;
            return;
        }

        CalamityFriction();

        Vector3 WishDir = transform.right * smove + transform.forward * fmove;

        WishSpeed = WishDir.magnitude;

        if (WishSpeed > 1f)
        {
            WishDir.Normalize();
            WishSpeed = 1f;
        }

        WishSpeed *= WalkSpeed;

        CalamityAcceleration(WishDir, WishSpeed, Accel);
        
    }

    /*
    private bool LadderForward;
    private Vector3 LadderVec;
    public void LadderMove()
    {
        float WishSpeed;
        float upscale;
        Vector3 wishdir, wishvel;

        float fmove = Input.GetAxisRaw("Vertical");
        float smove = Input.GetAxisRaw("Horizontal");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        if (LadderForward)
        {
            wishvel = LadderVec * -200f;
            MoveDirection.x *= wishvel.x;
            MoveDirection.z *= wishvel.z;
        }

        upscale = (transform.forward.y + 0.5f) * 2.5f;
        if (upscale > 1f)
            upscale = 1f;

        else if (upscale < -1f)
            upscale = -1f;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        wishvel = Vector3.zero;

        if (Mathf.Abs(fmove) > 0.0001f)
        {
            wishvel.y = 0.8f * upscale * fmove;
        }

        if (Mathf.Abs(smove) > 0.0001f && LadderVec.sqrMagnitude > 0.0001f)
        {
            Vector3 LadderRight, ang;
            Quaternion rotation = Quaternion.LookRotation(LadderVec);
            ang = rotation.eulerAngles;

            PlayerCameraControll.AngleVectors(ang, out _, out LadderRight, out _);

            if (Vector3.Dot(LadderVec, forward) > 0f)
            {
                LadderRight = -LadderRight;
            }

            wishvel += right * (0.5f * smove);
        }

        CalamityFriction();

        WishSpeed = wishvel.magnitude;

        wishdir = WishSpeed > 0f
        ? wishvel / WishSpeed
        : Vector3.zero;

        CalamityAcceleration(wishdir, WishSpeed, Accel);

        if (Mathf.Abs(wishvel.y) > 0.0001f)
        {
            if(MoveDirection.y > 0)
            {
                MoveDirection.y -= EarthGravity * Time.deltaTime;
                if (MoveDirection.y < 0)
                {
                    MoveDirection.y = 0;
                }
            }

            else
            {
                MoveDirection.y += EarthGravity * Time.deltaTime;
                if (MoveDirection.y > 0)
                {
                    MoveDirection.y = 0;
                }
            }
        }
    }


    /*
    private int Ladder;
    public void CheckLadderMove()
    {
        Vector3 Spot;
        Vector3 FlatForward;
        float tracedist;
        const float TRACE_LADDER_DIST = 48f;
        RaycastHit trace;

        bool wasOnLadder;

        wasOnLadder = (Ladder) != 0;


    }


    private bool OnLadder;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
            OnLadder = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
            OnLadder = false;
    }
    */
}