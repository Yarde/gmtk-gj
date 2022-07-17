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
            if (_isMoving || OnNewTurn == null || Game.Paused) return;
            _isMoving = true;

            var axisX = Input.GetAxis("Horizontal");
            var axisY = Input.GetAxis("Vertical");
            if (Input.anyKeyDown && axisX != 0) await OnNewTurn.Invoke(axisX > 0 ? Vector3.right : Vector3.left);
            if (Input.anyKeyDown && axisY != 0) await OnNewTurn.Invoke(axisY > 0 ? Vector3.forward : Vector3.back);

            _isMoving = false;
        }

        public event Func<Vector3, UniTask> OnNewTurn;
    }
}