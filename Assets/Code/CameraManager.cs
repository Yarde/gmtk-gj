using UnityEngine;
using Yarde.Utils.Extensions;

namespace Yarde
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Transform transformToFollow;
        [SerializeField] private float smoothSpeed = 10f;
        [SerializeField] private Vector3 offset;

        private void FixedUpdate()
        {
            Vector3 destination = transformToFollow.position + offset;
            Vector3 smoothed = Vector3.Lerp(transform.position, destination, smoothSpeed * Time.deltaTime);
            transform.position = smoothed.WithY(transform.position.y);
        }
    }
}
