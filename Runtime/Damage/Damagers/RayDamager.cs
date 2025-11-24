using System;
using UnityEngine;

namespace OneM.DamageSystem
{
    /// <summary>
    /// Damager implementation using Raycast to inflict damage.
    /// </summary>
    public sealed class RayDamager : IDamager
    {
        public Collider Collider { get; set; }
        public float CurrentAmount { get; set; }
        public Transform transform { get; set; }

        public event Action<IDamageable> OnDamageInflicted;

        public RayDamager(Transform transform, float damageAmount)
        {
            this.transform = transform;
            CurrentAmount = damageAmount;
        }

        /// <summary>
        /// Tries to hit and inflict damage to a damageable object in the given direction.
        /// </summary>
        /// <param name="origin">The raycast origin.</param>
        /// <param name="direction">The raycast direction.</param>
        /// <param name="distance">The raycast max distance.</param>
        /// <param name="collision">The raycast collision layer.</param>
        /// <returns><inheritdoc cref="TryInflictDamage(IDamageable)"/></returns>
        public bool TryHit(Vector3 origin, Vector3 direction, float distance, LayerMask collision)
        {
            var hasHit = Physics.Raycast(
                origin,
                direction,
                out RaycastHit hit,
                distance,
                collision
            );

            if (!hasHit) return false;

            var damageable = hit.transform.GetComponent<IDamageable>();
            return damageable != null && TryInflictDamage(damageable);
        }

        public bool TryInflictDamage(IDamageable damageable)
        {
            var wasDamageInflicted = damageable.TryTakeDamage(this);
            if (wasDamageInflicted) OnDamageInflicted?.Invoke(damageable);
            return wasDamageInflicted;
        }
    }
}