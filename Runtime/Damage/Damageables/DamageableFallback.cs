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
        [Min(0f), Tooltip("The distance to move away.")]
        public float FallbackDistance = 1f;
        [Min(0f), Tooltip("The time (in seconds) to move away.")]
        public float FallbackTime = 0.5f;

        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnsubscribeEvents();

        public void PlayFallbackAnimation(Transform damager)
        {
            var position = damager.position;
            position.y = transform.position.y;

            var direction = (transform.position - position).normalized;
            var finalPosition = transform.position + direction * FallbackDistance;

            transform.LookAt(position);
            StopAllCoroutines();
            StartCoroutine(FallbackMoveRoutine(finalPosition));
        }

        private void SubscribeEvents() => damageable.OnDamageTaken += HandleDamageTaken;
        private void UnsubscribeEvents() => damageable.OnDamageTaken -= HandleDamageTaken;
        private void HandleDamageTaken(IDamager damager) => PlayFallbackAnimation(damager.transform);

        private IEnumerator FallbackMoveRoutine(Vector3 finalPosition)
        {
            var currentTime = 0F;
            var initialPosition = transform.position;

            while (currentTime < FallbackTime)
            {
                var step = currentTime / FallbackTime;
                transform.position = Vector3.Lerp(initialPosition, finalPosition, step);

                currentTime += Time.deltaTime;
                yield return null;
            }

            transform.position = finalPosition;
        }
    }
}