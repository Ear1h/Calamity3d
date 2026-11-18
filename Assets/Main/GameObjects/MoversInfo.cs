using System;
using UnityEngine;

[System.Serializable]
public class MoveControllerInfo
{
    public enum MoveState
    {
        MOVE_STAY,
        MOVE_UP,
        MOVE_STAYUP,
        MOVE_DOWN,
    }

    public MoveState MoveStates;

    [Header("Move Parameters Info")]
    [field: SerializeField] public float Speed { get; set; } = 3f;
    [field: SerializeField] public float Lip { get; set; } = 0.1f;
    [field: SerializeField] public Vector3 Angles { get; set; }

    [field: SerializeField] public float Wait { get; set; }
    [field: SerializeField] public string Targetname { get; set; }

    [HideInInspector] public BoxCollider LipMesh;

    [HideInInspector] public Vector3 StartPos;
    [HideInInspector] public Vector3 EndPos;
    [HideInInspector] public Vector3 MoveDirection;
    [HideInInspector] public Vector3 OriginPos;

    [HideInInspector] public Coroutine MoveCoroutine;

    [HideInInspector] public float Progress;

    [HideInInspector] public bool InRange;
}