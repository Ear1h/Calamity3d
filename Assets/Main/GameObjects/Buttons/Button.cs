using System;
using System.Collections;
using UnityEngine;

public class Button : Thinker
{
    [SerializeField] private MoveControllerInfo Move;
    [SerializeField] private ButtonInfo btn;

    public void SetRange(bool choise)
    {
        Move.InRange = choise;
    }
    private void Awake()
    {
        Vector3 angles = Move.Angles;
        Vector3 direction = Move.MoveDirection;

        Move.LipMesh = GetComponent<BoxCollider>();
        Use = ButtonUse;

        VectorMathExtended.SetMoveDir(ref angles, ref direction);

        Move.MoveDirection = direction;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = Vector3.zero;

        VectorMathExtended.VectorCopy(transform.position, ref startPosition);

        Vector3 fabsDirection;

        fabsDirection.x = Mathf.Abs(Move.MoveDirection.x);
        fabsDirection.y = Mathf.Abs(Move.MoveDirection.y);
        fabsDirection.z = Mathf.Abs(Move.MoveDirection.z);

        float distance =
            (fabsDirection.x * Move.LipMesh.bounds.size.x) +
            (fabsDirection.y * Move.LipMesh.bounds.size.y) +
            (fabsDirection.z * Move.LipMesh.bounds.size.z) -
            Move.Lip;

        VectorMathExtended.VectorMA(
            startPosition,
            distance,
            Move.MoveDirection,
            ref endPosition
        );

        Move.EndPos = endPosition;
        Move.StartPos = startPosition;
        Move.MoveStates = MoveControllerInfo.MoveState.MOVE_STAY;
    }

    public void ButtonUse(Thinker self, Thinker other, Thinker activator)
    {
        self.Activator = activator;
        ButtonFire(self);

    }

    private void ButtonFire(Thinker activator)
    {
        if (Move.MoveStates == MoveControllerInfo.MoveState.MOVE_UP || Move.MoveStates == MoveControllerInfo.MoveState.MOVE_STAYUP)
            return;

        Move.MoveStates = MoveControllerInfo.MoveState.MOVE_UP;

        StartButtonMove(Move.EndPos, ButtonWait);
    }

    private void StartButtonMove(Vector3 end, Action Choise)
    {
        if (Move.MoveCoroutine != null)
            StopCoroutine(Move.MoveCoroutine);

        Move.MoveCoroutine = StartCoroutine(ButtonMovePosition(end, Choise));
    }

    IEnumerator StartButtonWait()
    {
        yield return new WaitForSeconds(Move.Wait);
        ButtonReturn();

    }
    private void ButtonWait()
    {
        Move.MoveStates = MoveControllerInfo.MoveState.MOVE_STAYUP;

        UseTargets(this, this.Activator);
        if (Move.Wait >= 0)
        {
            StartCoroutine(StartButtonWait());
        }
    }

    private void ButtonReturn()
    {
        if (Move.MoveStates == MoveControllerInfo.MoveState.MOVE_DOWN)
            return;

        Move.MoveStates = MoveControllerInfo.MoveState.MOVE_DOWN;

        StartButtonMove(Move.StartPos, ButtonDone);
    }

    private void ButtonDone()
    {
        Move.MoveStates = MoveControllerInfo.MoveState.MOVE_STAY;
    }

    private IEnumerator ButtonMovePosition(Vector3 end, Action Choise)
    {
        Vector3 start = transform.position;
        float progress = 0f;

        while (progress < 1f)
        {
            progress += Time.deltaTime * Move.Speed;

            Vector3 nextPosition = Vector3.Lerp(
                start,
                end,
                Mathf.Clamp01(progress)
            );

            transform.position = nextPosition;

            yield return null;
        }

        transform.position = end;

        Move.MoveCoroutine = null;

        Choise?.Invoke();
    }

    public void ButtonTouchTrigger(Collider other)
    {
        Actor toucher = other.GetComponentInParent<Actor>();
        if (toucher.GetHealth() <= 0)
            return;

        if (!(toucher is PlayerPawn player))
            return;

        if (!string.IsNullOrEmpty(this.TargetName))
            return;

        this.Activator = toucher;
        Activate(toucher, toucher);
    }
}

