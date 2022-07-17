using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Yarde.GameBoard;

namespace Yarde
{
    public class InputManager : MonoBehaviour
    {
        private bool _isMoving;

        public async void Update()
        {
            if (_isMoving || OnNewTurn == null || Game.Paused)
            {
                return;
            }
            _isMoving = true;

            if (Input.GetKeyDown(KeyCode.A)) { await OnNewTurn.Invoke(Vector3.left); }
            if (Input.GetKeyDown(KeyCode.D)) { await OnNewTurn.Invoke(Vector3.right); }
            if (Input.GetKeyDown(KeyCode.W)) { await OnNewTurn.Invoke(Vector3.forward); }
            if (Input.GetKeyDown(KeyCode.S)) { await OnNewTurn.Invoke(Vector3.back); }

            _isMoving = false;
        }

        public event Func<Vector3, UniTask> OnNewTurn;
    }
}
