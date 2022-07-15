using Cysharp.Threading.Tasks;
using UnityEngine;
using Yarde.Utils.Extensions;

namespace Yarde
{
    public class Player : MonoBehaviour
    {
        private bool _isMoving;
        private const float SPEED = 3;

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

            for (int i = 0; i < 90 / SPEED; i++)
            {
                transform.RotateAround(anchor, axis, SPEED);
                await UniTask.Delay(0.01f.ToMilliseconds());
            }

            _isMoving = false;
        }
    }
}
