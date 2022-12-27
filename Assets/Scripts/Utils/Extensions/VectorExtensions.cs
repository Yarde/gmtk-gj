using UnityEngine;

namespace Yarde.Utils.Extensions
{
    public static class VectorExtensions
    {
        public static float Dot(this Vector2 v, Vector2 u)
        {
            return (v.x * u.x + v.y * u.y);
        }

        public static int Cross(this Vector2Int v, Vector2Int u)
        {
            return v.x * u.y - v.y * u.x;
        }

        public static Vector3 Clamp(this Vector3 v3, Vector3 lowerLimit, Vector3 upperLimit)
        {
            float x = Mathf.Clamp(v3.x, lowerLimit.x, upperLimit.x);
            float y = Mathf.Clamp(v3.y, lowerLimit.y, upperLimit.y);
            float z = Mathf.Clamp(v3.z, lowerLimit.z, upperLimit.z);
            return new Vector3(x, y, z);
        }

        public static Vector3 Abs(this Vector3 v3)
        {
            return new Vector3(Mathf.Abs(v3.x), Mathf.Abs(v3.y), Mathf.Abs(v3.z));
        }

        public static Vector3 WithX(this Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }

        public static Vector3 WithY(this Vector3 v, float y)
        {
            return new Vector3(v.x, y, v.z);
        }

        public static Vector3 WithZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }

        public static Vector2 WithX(this Vector2 v, float x)
        {
            return new Vector2(x, v.y);
        }

        public static Vector2 WithY(this Vector2 v, float y)
        {
            return new Vector2(v.x, y);
        }

        public static Vector3 WithZ(this Vector2 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }

#if NET_4_6
	[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
        public static Vector3 Rotate180(this Vector3 v)
        {
            return new Vector3(-v.x, v.y, -v.z);
        }

#if NET_4_6
	[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
        public static Vector3 Rotate90Left(this Vector3 v)
        {
            return new Vector3(-v.z, v.y, v.x);
        }

#if NET_4_6
	[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
        public static Vector3 Rotate90Right(this Vector3 v)
        {
            return new Vector3(v.z, v.y, -v.x);
        }

        public static bool IsAnyBigger(this Vector3 v, float value)
        {
            return v.x > value || v.y > value || v.z > value;
        }

        public static float BiggestAxis(this Vector3 v)
        {
            return Mathf.Max(v.x, Mathf.Max(v.y, v.z));
        }

        public static float SmallestAxis(this Vector3 v)
        {
            return Mathf.Min(v.x, Mathf.Min(v.y, v.z));
        }

        public static float FarestAxis(this Vector3 v)
        {
            if (Mathf.Abs(v.x) >= Mathf.Abs(v.y))
            {
                if (Mathf.Abs(v.x) >= Mathf.Abs(v.z))
                {
                    return v.x;
                }
                return v.z;
            }
            if (Mathf.Abs(v.y) >= Mathf.Abs(v.z))
            {
                return v.y;
            }
            return v.z;
        }

        public static float FarestAxisAbs(this Vector3 v)
        {
            if (Mathf.Abs(v.x) >= Mathf.Abs(v.y))
            {
                if (Mathf.Abs(v.x) >= Mathf.Abs(v.z))
                {
                    return Mathf.Abs(v.x);
                }
                return Mathf.Abs(v.z);
            }
            if (Mathf.Abs(v.y) >= Mathf.Abs(v.z))
            {
                return Mathf.Abs(v.y);
            }
            return Mathf.Abs(v.z);
        }

        public static float CalculateAngle(Vector3 from, Vector3 to)
        {
            return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
        }

        public static Vector2 Clamp(this Vector2 v, float clampX, float clampY)
        {
            return new Vector2(Mathf.Min(v.x, clampX), Mathf.Min(v.y, clampY));
        }

        public static Vector3 NearestPointOnAxis(this Vector3 axisDirection, Vector3 point, bool isNormalized = false)
        {
            if (!isNormalized) axisDirection.Normalize();
            var d = Vector3.Dot(point, axisDirection);
            return axisDirection * d;
        }

        public static Vector3 NearestPointOnLine(
            this Vector3 lineDirection, Vector3 point, Vector3 pointOnLine, bool isNormalized = false)
        {
            if (!isNormalized) lineDirection.Normalize();
            var d = Vector3.Dot(point - pointOnLine, lineDirection);
            return pointOnLine + (lineDirection * d);
        }

        public static Vector3 MulEveryAxis(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vector3 DiffEveryAxis(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        public static Vector2 FromXZ(Vector3 a)
        {
            return new Vector2(a.x, a.z);
        }

        public static bool IsValid(this Vector2 a)
        {
            return a.x.IsValid() && a.y.IsValid();
        }

        public static bool IsValid(this Vector3 a)
        {
            return a.x.IsValid() && a.y.IsValid() && a.z.IsValid();
        }

        public static Vector2 Valid(this Vector2 a)
        {
            return a.IsValid() ? a : Vector2.zero;
        }

        public static Vector3 Valid(this Vector3 a)
        {
            return a.IsValid() ? a : Vector3.zero;
        }

        public static bool IsValid(this float a)
        {
            return !float.IsNaN(a) && !float.IsInfinity(a);
        }
    }
}
