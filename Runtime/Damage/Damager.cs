using System;
using UnityEngine;

namespace OneM.DamageSystem
{
    /// <summary>
    /// Default damager implementation. It can damage up to 10 <see cref="IDamageable"/> implementations.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class Damager : MonoBehaviour, IDamager
    {
        [SerializeField, Tooltip("The local collider component used to check collisions.")]
        private Collider damageCollider;
        [SerializeField, Min(0F), Tooltip("The current damage to inflict.")]
        private float current = 1F;
        [Tooltip("The layers to inflict damage.")]
        public LayerMask layers;
        [Tooltip("Whether to disable this component after damage is inflicted.")]
        public bool disableAfterInflictDamage = true;

        public event Action OnDamageInflicted;

        public float Current
        {
            get => current;
            set => current = Mathf.Max(value, 0f);
        }

        private readonly Collider[] buffer = new Collider[10];

        private void Reset() => damageCollider = GetComponent<Collider>();
        private void FixedUpdate() => TryInflictNearbyDamage();

        public bool TryInflictDamage(IDamageable damageable)
        {
            var wasDamageInflicted = damageable.TryTakeDamage(this);
            if (wasDamageInflicted)
            {
                OnDamageInflicted?.Invoke();
                if (disableAfterInflictDamage) enabled = false;
            }
            return wasDamageInflicted;
        }

        private void TryInflictNearbyDamage()
        {
            var bounds = damageCollider.bounds;
            var hits = Physics.OverlapBoxNonAlloc(
                bounds.center,
                bounds.size * 0.5F,
                buffer,
                transform.rotation,
                layers
            );

            for (var i = 0; i < hits; i++)
            {
                var hasDamageable = buffer[i].TryGetComponent(out IDamageable damageable);
                if (hasDamageable) TryInflictDamage(damageable);
            }
        }
    }
}