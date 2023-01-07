using UnityEngine;

namespace Yarde.GameBoard
{
    public class BoardObject : MonoBehaviour
    {
        private const float ERROR_CORRECTION = 0.01f;
        [SerializeField] private Vector2 size = Vector2.one;
        [SerializeField] public AudioClip soundOnPlayerHit;

        public Vector2 Size => size;

        public bool CheckCollision(Vector3 otherPosition, Vector2 otherSize) =>
            CheckCollision(transform.position, size, otherPosition, otherSize);

        public bool CheckCollision(Vector3 thisPosition, Vector2 thisSize, Vector3 otherPosition, Vector2 otherSize)
        {
            float distanceX = Mathf.Abs(otherPosition.x - thisPosition.x);
            float distanceZ = Mathf.Abs(otherPosition.z - thisPosition.z);

            // when calculating distance there is floating point approximation error so I added small error correction to it
            bool collidedX = distanceX + ERROR_CORRECTION < (otherSize.x + thisSize.x) / 2;
            bool collidedZ = distanceZ + ERROR_CORRECTION < (otherSize.y + thisSize.y) / 2;

            // game is in 2D plane so we skip Y
            bool collision = collidedX && collidedZ;
            return collision;
        }
    }
}