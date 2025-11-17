using System;

namespace OneM.DamageSystem
{
    /// <summary>
    /// Interface used on objects able to inflict damage.
    /// </summary>
    public interface IDamager
    {
        /// <summary>
        /// Event fired just after inflict damage.
        /// </summary>
        event Action OnDamageInflicted;

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