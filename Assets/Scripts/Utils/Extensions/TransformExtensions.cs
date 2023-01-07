using UnityEngine;

namespace Yarde.Utils.Extensions
{
    public static class TransformExtensions
    {
        public static Vector3 SetScaleX(this Transform transform, float x)
        {
            var scale = transform.localScale;
            scale.x = x;
            return transform.localScale = scale;
        }

        public static Vector3 SetScaleY(this Transform transform, float y)
        {
            var scale = transform.localScale;
            scale.y = y;
            return transform.localScale = scale;
        }

        public static Vector3 SetScaleZ(this Transform transform, float z)
        {
            var scale = transform.localScale;
            scale.z = z;
            return transform.localScale = scale;
        }

        /// <summary>
        /// Sets pivot of RectTransform without changing position
        /// </summary>
        public static void SetPivot(this RectTransform transform, Vector2 pivot)
        {
            Vector2 size = transform.rect.size;
            Vector2 deltaPivot = transform.pivot - pivot;
            Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);

            transform.pivot = pivot;
            transform.localPosition -= deltaPosition;
        }

        public static Vector2 SetAnchoredPositionX(this RectTransform transform, float x)
        {
            Vector2 anchoredPos = transform.anchoredPosition;
            anchoredPos.x = x;
            return transform.anchoredPosition = anchoredPos;
        }

        public static Vector2 SetAnchoredPositionY(this RectTransform transform, float y)
        {
            Vector2 anchoredPos = transform.anchoredPosition;
            anchoredPos.y = y;
            return transform.anchoredPosition = anchoredPos;
        }

        public static Vector2 SetAnchorMinY(this RectTransform transform, float y)
        {
            Vector2 anchorMin = transform.anchorMin;
            anchorMin.y = y;
            return transform.anchorMin = anchorMin;
        }

        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            var dir = point - pivot;
            dir = Quaternion.Euler(angles) * dir;
            point = dir + pivot;
            return point;
        }

        public static void DestroyAllChildren(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                Object.Destroy(child.gameObject);
            }
        }
    }
}