
using Unity.VisualScripting;
using UnityEngine;

public static class VectorMathExtended
{
    public static void AngleVectors(Vector3 angles, out Vector3 forward, out Vector3 right, out Vector3 up)
    {
        float yaw = angles.y * Mathf.Deg2Rad;
        float pitch = angles.x * Mathf.Deg2Rad;
        float roll = angles.z * Mathf.Deg2Rad;

        float sy = Mathf.Sin(yaw);
        float cy = Mathf.Cos(yaw);

        float sp = Mathf.Sin(pitch);
        float cp = Mathf.Cos(pitch);

        float sr = Mathf.Sin(roll);
        float cr = Mathf.Cos(roll);

        forward = new Vector3(
            cp * cy,
            -sp,
            cp * sy
        );

        right = new Vector3(
            -sr * sp * cy + cr * sy,
            sr * cp,
            -sr * sp * sy - cr * cy
        );

        up = new Vector3(
            cr * sp * cy + sr * sy,
            cr * cp,
            cr * sp * sy - sr * cy
        );
    }

    static Vector3 VEC_UP = new (0, 0, -1);
    static Vector3 MOVEDIR_UP = new (0, 1, 0);
    static Vector3 VEC_DOWN = new (0, 0, -2);
    static Vector3 MOVEDIR_DOWN = new (0, -1, 0);

    public static void SetMoveDir(ref Vector3 angles, ref Vector3 movedir)
    {
        if (VectorCompare(angles, VEC_UP))
        {
            VectorCopy(MOVEDIR_UP, ref movedir);
        }

        else if (VectorCompare(angles, VEC_DOWN))
        {
            VectorCopy(MOVEDIR_DOWN, ref movedir);
        }

        else
        {
            AngleVectors(angles, out movedir, out _, out _);
        }

        angles = Vector3.zero;
    }

    public static bool VectorCompare(Vector3 v1, Vector3 v2)
    {
        return v1 == v2;
    }

    public static void VectorCopy(Vector3 v1, ref Vector3 v2)
    {
        v2 = v1;
    }

    public static void VectorClear(ref Vector3 v1)
    {
        v1 = Vector3.zero;
    }

    public static void VectorMA(Vector3 veca, float scale, Vector3 vecb, ref Vector3 vecc)
    {
        vecc = veca + scale * vecb;
    }
}
