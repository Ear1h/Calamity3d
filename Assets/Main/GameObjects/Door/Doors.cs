using System;
using System.Collections;
using UnityEngine;

public class DoorController : Thinker
{
    [SerializeField] private MoveControllerInfo Move;
    [SerializeField] private DoorControllerInfo Door;

    public void SetRange(bool choise)
    {
        Move.InRange = choise;
    }
    private void Awake()
    {
        Vector3 angles = Move.Angles;
        Vector3 direction = Move.MoveDirection;

        Move.LipMesh = GetComponent<BoxCollider>();
        Use = DoorUse;

        VectorMathExtended.SetMoveDir(ref angles, ref direction);

        Move.MoveDirection = direction;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = Vector3.zero;
        Vector3 originPosition = transform.position;

        VectorMathExtended.VectorCopy(transform.position, ref startPosition);
        VectorMathExtended.VectorCopy(transform.position, ref originPosition);

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

        if (Door.Flags.HasFlag(DoorControllerInfo.DoorFlags.StayOpen))
        {
            VectorMathExtended.VectorCopy(endPosition, ref originPosition);
            VectorMathExtended.VectorCopy(startPosition, ref endPosition);
            VectorMathExtended.VectorCopy(originPosition, ref startPosition);
        }

        Move.StartPos = startPosition;
        Move.EndPos = endPosition;
        Move.OriginPos = originPosition;

        transform.position = Move.OriginPos;

        Move.MoveStates = MoveControllerInfo.MoveState.MOVE_STAY;
    }

    public void DoorUse(Thinker self, Thinker other, Thinker activator)
    {
        if (other is PlayerPawn && !Move.InRange)
        {
            if (Move.MoveStates == MoveControllerInfo.MoveState.MOVE_STAYUP ||
                Move.MoveStates == MoveControllerInfo.MoveState.MOVE_UP)
            {
                DoorGoDown();
                return;
            }
        }

        else
        {
            DoorGoUp(activator);
        }

    }

    private void DoorGoUp(Thinker activator)
    {
        if (Move.MoveStates == MoveControllerInfo.MoveState.MOVE_UP || Move.MoveStates == MoveControllerInfo.MoveState.MOVE_STAYUP)
            return;

        Move.MoveStates = MoveControllerInfo.MoveState.MOVE_UP;

        UseTargets(this, activator);
        StartDoorMove(Move.EndPos, DoorHitTop);
    }

    private void StartDoorMove(Vector3 end, Action Choise)
    {
        if (Move.MoveCoroutine != null)
            StopCoroutine(Move.MoveCoroutine);

        if (Door.Flags.HasFlag(DoorControllerInfo.DoorFlags.Smooth))
        {
            Move.MoveCoroutine = StartCoroutine(DoorSmoothMovePosition(end, Choise));
            return;
        }

        Move.MoveCoroutine = StartCoroutine(DoorMovePosition(end, Choise));
    }

    private void DoorHitTop()
    {
        Move.MoveStates = MoveControllerInfo.MoveState.MOVE_STAYUP;
    }

    private void DoorGoDown()
    {
        if (Move.MoveStates == MoveControllerInfo.MoveState.MOVE_DOWN)
            return;

        Move.MoveStates = MoveControllerInfo.MoveState.MOVE_DOWN;

        StartDoorMove(Move.StartPos, DoorHitBottom);
    }

    private void DoorHitBottom()
    {
        Move.MoveStates = MoveControllerInfo.MoveState.MOVE_STAY;
    }

    private IEnumerator DoorMovePosition(Vector3 end, Action Choise)
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

    private IEnumerator DoorSmoothMovePosition(Vector3 end, Action Choise)
    {
        float duration = Move.Speed;

        float targetProgress =
            end == Move.EndPos ? 1f : 0f;

        while (true)
        {
            Move.Progress = Mathf.MoveTowards(
                Move.Progress,
                targetProgress,
                Time.deltaTime * duration
            );

            float step = Mathf.SmoothStep(0f, 1f, Move.Progress);

            Vector3 nextPosition = Vector3.Lerp(
                Move.StartPos,
                Move.EndPos,
                step
            );

            transform.position = nextPosition;

            if (Mathf.Approximately(Move.Progress, targetProgress))
                break;

            yield return null;
        }

        transform.position = end;

        Move.MoveCoroutine = null;

        Choise?.Invoke();
    }

    public void DoorTouchTrigger(Collider other)
    {
        Actor toucher = other.GetComponentInParent<Actor>();
        if (toucher.GetHealth() <= 0)
            return;

        if (!(toucher is PlayerPawn player))
            return;

        if (!string.IsNullOrEmpty(this.TargetName))
            return;

        Activate(toucher, toucher);
    }
}

