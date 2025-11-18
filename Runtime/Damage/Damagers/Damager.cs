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
        [field: SerializeField, Tooltip("The local collider component used to check collisions.")]
        public Collider Collider { get; set; }
        [SerializeField, Min(0F), Tooltip("The current damage amount to inflict.")]
        private float currentAmount = 1F;
        [Tooltip("The layers to inflict damage.")]
        public LayerMask Layers;
        [Tooltip("Whether to disable this component after damage is inflicted.")]
        public bool DisableAfterInflictDamage;

        public event Action<IDamageable> OnDamageInflicted;

        public float CurrentAmount
        {
            get => currentAmount;
            set => currentAmount = Mathf.Max(value, 0f);
        }

        private readonly Collider[] buffer = new Collider[10];

        private void Reset() => Collider = GetComponent<Collider>();
        private void FixedUpdate() => TryInflictNearbyDamage();

        public void Enable() => SetEnable(true);
        public void Disable() => SetEnable(false);
        public void SetEnable(bool isEnabled) => gameObject.SetActive(isEnabled);

        public bool TryInflictDamage(IDamageable damageable)
        {
            var wasDamageInflicted = damageable.TryTakeDamage(this);
            if (wasDamageInflicted)
            {
                OnDamageInflicted?.Invoke(damageable);
                if (DisableAfterInflictDamage) enabled = false;
            }
            return wasDamageInflicted;
        }

        private void TryInflictNearbyDamage()
        {
            var bounds = Collider.bounds;
            var hits = Physics.OverlapBoxNonAlloc(
                bounds.center,
                bounds.size * 0.5F,
                buffer,
                transform.rotation,
                Layers
            );

            for (var i = 0; i < hits; i++)
            {
                var hasDamageable = buffer[i].TryGetComponent(out IDamageable damageable);
                if (hasDamageable) TryInflictDamage(damageable);
            }
        }
    }
}