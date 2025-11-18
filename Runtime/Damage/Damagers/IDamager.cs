using System;
using UnityEngine;

namespace OneM.DamageSystem
{
    /// <summary>
    /// Interface used on objects able to inflict damage.
    /// </summary>
    public interface IDamager
    {
        public Transform transform { get; }

        /// <summary>
        /// Event fired just after inflict damage to the given damageable instance.
        /// </summary>
        event Action<IDamageable> OnDamageInflicted;

        /// <summary>
        /// The current damage amount to inflict.
        /// </summary>
        public float CurrentAmount { get; set; }

        /// <summary>
        /// Checks whether can inflict damage into the given damagable instance.
        /// </summary>
        /// <param name="damageable">The damageable instance to inflict damage.</param>
        /// <returns><inheritdoc cref="TryInflictDamage()"/></returns>
        bool TryInflictDamage(IDamageable damageable);
    }
}