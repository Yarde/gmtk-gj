using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Yarde
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float angleIncrement = 5;
        [SerializeField] private int moveDelayInMillis = 10;
        
        private bool _isMoving;

        private void Update()
        {
            if (_isMoving)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.A)) { Roll(Vector3.left).Forget(); }
            if (Input.GetKeyDown(KeyCode.D)) { Roll(Vector3.right).Forget(); }
            if (Input.GetKeyDown(KeyCode.W)) { Roll(Vector3.forward).Forget(); }
            if (Input.GetKeyDown(KeyCode.S)) { Roll(Vector3.back).Forget(); }

        }

        private async UniTask Roll(Vector3 direction)
        {
            _isMoving = true;

            Vector3 anchor = transform.position + (Vector3.down + direction) * 0.5f;
            Vector3 axis = Vector3.Cross(Vector3.up, direction);

            for (int i = 0; i < 90 / angleIncrement; i++)
            {
                transform.RotateAround(anchor, axis, angleIncrement);
                await UniTask.Delay(moveDelayInMillis);
            }

            _isMoving = false;
        }
    }
}
