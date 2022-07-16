using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Yarde.GameBoard;
using Yarde.Utils.Logger;

namespace Yarde
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float angleIncrement = 5;
        [SerializeField] private int moveDelayInMillis = 10;
        [SerializeField] private float startingHealth = 3;

        [SerializeField] private List<Transform> sides;

        public float HealthPoints { get; private set; }
        public float Points { get; private set; }
        public Vector2 Size => new Vector2(1, 1);

        public int TopSide => FindTopSide();
        public Action OnUpdate { get; set; }

        private int FindTopSide()
        {
            float maxY = 0f;
            Transform maxSide = transform;
            foreach (Transform side in sides)
            {
                if (side.position.y > maxY)
                {
                    maxSide = side;
                    maxY = side.position.y;
                }
            }

            return int.Parse(maxSide.name);
        }

        private void Awake()
        {
            HealthPoints = startingHealth;
            Points = 0;
        }

        public async UniTask Roll(Vector3 direction)
        {
            Vector3 anchor = transform.position + (Vector3.down + direction) * 0.5f;
            Vector3 axis = Vector3.Cross(Vector3.up, direction);

            for (int i = 0; i < 90 / angleIncrement; i++)
            {
                transform.RotateAround(anchor, axis, angleIncrement);
                await UniTask.Delay(moveDelayInMillis);
            }
        }

        public async UniTask HalfRoll(Vector3 direction)
        {
            Vector3 anchor = transform.position + (Vector3.down + direction) * 0.5f;
            Vector3 axis = Vector3.Cross(Vector3.up, direction);

            for (int i = 0; i < 15 / angleIncrement; i++)
            {
                transform.RotateAround(anchor, axis, angleIncrement);
                await UniTask.Delay(moveDelayInMillis);
            }
            
            for (int i = 0; i < 15 / angleIncrement; i++)
            {
                transform.RotateAround(anchor, axis, -angleIncrement);
                await UniTask.Delay(moveDelayInMillis);
            }
        }

        public void TakeDamage(float damage)
        {
            HealthPoints -= damage;

            if (HealthPoints <= 0)
            {
                this.LogError("Game Lost!");
            }
            OnUpdate?.Invoke();
        }
        
        public void AddPoints(int points)
        {
            Points += points;
            OnUpdate?.Invoke();
        }

        public async UniTask OnEnemyKilled(float enemyLevel)
        {
            HealthPoints += enemyLevel;
            Points += enemyLevel;
        }

        public async UniTask CollectItem(CollectibleReward collect)
        {
            HealthPoints += collect.hearts;
            Points += collect.points;
            angleIncrement += collect.speed;
        }
    }
}
