using UnityEngine;

namespace Yarde.GameBoard
{
    public class BoardObject : MonoBehaviour
    {
        private const float ERROR_CORRECTION = 0.01f;
        [SerializeField] private Vector2 size = Vector2.one;

        public bool CheckCollision(Vector3 other, Vector2 otherSize)
        {
            Vector3 position = transform.position;
            float distanceX = Mathf.Abs(other.x - position.x);
            float distanceZ = Mathf.Abs(other.z - position.z);
            // when calculating distance there is floating point approximation error so I added small error correction to it
            bool collidedX = distanceX + ERROR_CORRECTION < (otherSize.x + size.x) / 2;
            bool collidedZ = distanceZ + ERROR_CORRECTION < (otherSize.y + size.y) / 2;

            // game is in 2D plane so we skip Y
            bool collision = collidedX && collidedZ;
            return collision;
        }
    }
}
