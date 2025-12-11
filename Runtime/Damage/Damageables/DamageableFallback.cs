using System;
using UnityEngine;
using System.Collections;

namespace OneM.DamageSystem
{
    /// <summary>
    /// Plays a move fallback animation when taking damage.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class DamageableFallback : MonoBehaviour
    {
        [SerializeField, Tooltip("The local Damageable component.")]
        private Damageable damageable;
        [SerializeField, Tooltip("The transform to move when fallback.")]
        private Transform moveTransform;

        [Header("FALLBACK")]
        [Min(0f), Tooltip("The distance to move away.")]
        public float FallbackDistance = 0.4f;
        [Min(0f), Tooltip("The time (in seconds) to move away.")]
        public float FallbackTime = 0.1f;

        public event Action OnMoveStarted;
        public event Action OnMoveFinished;

        public bool IsMoving { get; private set; }

        private void Reset() => FindComponents();
        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnsubscribeEvents();

        public void PlayFallbackAnimation(Transform damager)
        {
            var position = damager.position;
            position.y = moveTransform.position.y;

            var direction = (moveTransform.position - position).normalized;
            var finalPosition = moveTransform.position + direction * FallbackDistance;

            moveTransform.LookAt(position);
            StopAllCoroutines();
            StartCoroutine(FallbackMoveRoutine(finalPosition));
        }

        private void SubscribeEvents() => damageable.OnDamageTaken += HandleDamageTaken;
        private void UnsubscribeEvents() => damageable.OnDamageTaken -= HandleDamageTaken;
        private void HandleDamageTaken(IDamager damager) => PlayFallbackAnimation(damager.transform);

        private void FindComponents()
        {
            damageable = GetComponentInParent<Damageable>();
            moveTransform = damageable.transform;
        }

        private IEnumerator FallbackMoveRoutine(Vector3 finalPosition)
        {
            var currentTime = 0F;
            var initialPosition = moveTransform.position;

            IsMoving = true;
            OnMoveStarted?.Invoke();

            while (currentTime < FallbackTime)
            {
                var step = currentTime / FallbackTime;
                moveTransform.position = Vector3.Lerp(initialPosition, finalPosition, step);

                currentTime += Time.deltaTime;
                yield return null;
            }

            moveTransform.position = finalPosition;

            IsMoving = false;
            OnMoveFinished?.Invoke();
        }
    }
}