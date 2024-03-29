﻿using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarde.GameBoard;
using Yarde.Utils.Extensions;

namespace Yarde
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float angleIncrement = 5;
        [SerializeField] private int moveDelayInMillis = 10;
        [SerializeField] private float startingHealth = 3;
        [SerializeField] private float hpGainMultiplier = 3;
        [SerializeField] private float delayBetweenMovesInSec = 1;
        [SerializeField] private ParticleSystem deathParticles;

        [SerializeField] private List<Transform> sides;
        private readonly List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>();

        public float HealthPoints { get; private set; }
        private float points;
        public Vector2 Size => new Vector2(1, 1);
        public Transform Transform => transform;

        public int TopSide => FindTopSide();
        public event Action OnUpdate;
        public event Action OnKill;
        public event Action<float> OnDamage;
        public event Action OnItemCollected;
        public float MaxHealthPoints { get; private set; }

        private void Awake()
        {
            HealthPoints = startingHealth;
            MaxHealthPoints = startingHealth;
            points = 0;

            foreach (Transform side in sides)
            {
                _spriteRenderers.Add(side.GetComponent<SpriteRenderer>());
            }
        }

        private int FindTopSide()
        {
            float maxY = 0f;
            Transform maxSide = Transform;
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

        public async UniTask Roll(Vector3 direction)
        {
            Vector3 anchor = Transform.position + (Vector3.down + direction) * 0.5f;
            Vector3 axis = Vector3.Cross(Vector3.up, direction);

            FullMove(anchor, axis).Forget();
            await UniTask.Delay(delayBetweenMovesInSec.ToMilliseconds());
        }

        private async UniTask FullMove(Vector3 anchor, Vector3 axis)
        {
            for (int i = 0; i < 90 / angleIncrement; i++)
            {
                Transform.RotateAround(anchor, axis, angleIncrement);
                await UniTask.Delay(moveDelayInMillis);
            }
        }

        public async UniTask HalfRoll(Vector3 direction)
        {
            Vector3 anchor = Transform.position + (Vector3.down + direction) * 0.5f;
            Vector3 axis = Vector3.Cross(Vector3.up, direction);

            HalfMove(anchor, axis).Forget();
            await UniTask.Delay(delayBetweenMovesInSec.ToMilliseconds());
        }

        private async UniTask HalfMove(Vector3 anchor, Vector3 axis)
        {
            var startingTransform = Transform.position;
            
            for (int i = 0; i < 15 / angleIncrement; i++)
            {
                Transform.RotateAround(anchor, axis, angleIncrement);
                await UniTask.Delay(moveDelayInMillis);
            }

            for (int i = 0; i < 15 / angleIncrement; i++)
            {
                Transform.RotateAround(anchor, axis, -angleIncrement);
                await UniTask.Delay(moveDelayInMillis);
            }

            Transform.position = startingTransform;
        }

        public async UniTask TakeDamage(float damage)
        {
            HealthPoints -= damage;

            if (HealthPoints <= 0)
            {
                deathParticles.transform.position = Transform.position;
                deathParticles.Play();
                await UniTask.Delay(1000);
                SceneManager.LoadScene("Scenes/EndScreenFail");
            }

            if (damage >= 1)
            {
                foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
                {
                    spriteRenderer.DOColor(Color.red, 0.1f);
                }

                await UniTask.Delay(100);
                foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
                {
                    spriteRenderer.DOColor(Color.white, 0.1f);
                }

                await UniTask.Delay(100);
            }

            OnDamage?.Invoke(damage);
            OnUpdate?.Invoke();
        }

        public void AddPoints(int points)
        {
            this.points += points;
            OnUpdate?.Invoke();
        }

        public void OnEnemyKilled(float enemyLevel)
        {
            HealthPoints += enemyLevel * hpGainMultiplier;
            points += enemyLevel;
            OnKill?.Invoke();
        }

        public void CollectItem(CollectibleReward collect)
        {
            HealthPoints += collect.hearts;
            points += collect.points;
            angleIncrement += collect.speed;
            OnItemCollected?.Invoke();
        }
    }
}