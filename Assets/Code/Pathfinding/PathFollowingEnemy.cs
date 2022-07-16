using System.Collections.Generic;
using UnityEngine;

namespace Yarde
{
    public class PathFollowingEnemy : MonoBehaviour
    {
        [SerializeField] private int movementSpeed;
        [SerializeField] private List<Transform> waypoints;
        private Player dice;
        private int direction = 1;
        private int waypointIndex = 0;

        private void Start()
        {
            transform.position = waypoints[waypointIndex].position;
            dice = FindObjectOfType<Player>();
        }
        
        private void Update()
        {
            FollowPath();
        }

        private void FollowPath()
        {
            if (waypointIndex < waypoints.Count && waypointIndex >= 0)
            {
                var targetPosition = waypoints[waypointIndex].position;
                var delta = movementSpeed * Time.deltaTime;
                var dicePosition = dice.transform.position;
                var axis = dicePosition.x > dicePosition.y ? new Vector3(1, 0) : new Vector3(0, 1);
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(targetPosition.x * axis.x, targetPosition.y * axis.y), delta);
                if (transform.position == targetPosition) waypointIndex += direction;
            }
            else
            {
                direction *= -1;
            }
        }
    }
}