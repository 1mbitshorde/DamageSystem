using System;
using UnityEngine;

namespace OneM.DamageSystem
{
    /// <summary>
    /// Default damageable implementation. It can be damaged by an <see cref="IDamager"/> implementation.
    /// </summary>
    /// <remarks>
    /// It will use the <see cref="Energy"/> component when taking damage.
    /// If none is set, damages will be taken without removing any energy.
    /// </remarks>
    [DisallowMultipleComponent]
    public sealed class Damageable : MonoBehaviour, IDamageable
    {
        [Tooltip("The Energy component to inflict damage.")]
        public Energy Energy;

        [field: SerializeField, Tooltip("Whether is invulnerable to damages.")]
        public bool IsInvulnerable { get; set; }

        public event Action OnDamageTaken;

        private void Reset() => Energy = GetComponentInChildren<Energy>();
        private void Start() { /* To show the component Toggle in the Inspector*/ }

        public bool IsDestroyed() => !enabled;
        public bool IsAbleToTakeDamage() => enabled && !IsInvulnerable;

        public void Respawn()
        {
            enabled = true;
            Energy.CompleteToInitial();
        }

        public void TakeDamage(IDamager damager)
        {
            if (Energy)
            {
                var damage = damager.CurrentAmount;
                Energy.Remove(damage);

                if (Energy.IsEmpty())
                {
                    enabled = false;
                    return;
                }
            }

            OnDamageTaken?.Invoke();
        }
    }
}