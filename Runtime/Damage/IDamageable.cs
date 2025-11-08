using System;

namespace OneM.DamageSystem
{
    /// <summary>
    /// Interface used on objects able to be damaged.
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// Event fired just after receive damage.
        /// </summary>
        event Action OnDamageTaken;

        /// <summary>
        /// Whether this instance is invulnerable to damage.
        /// </summary>
        bool IsInvulnerable { get; set; }

        /// <summary>
        /// Whether is able to take damage.
        /// </summary>
        /// <returns>True if able to take damage. False otherwise.</returns>
        bool IsAbleToTakeDamage();

        /// <summary>
        /// Tries to take damage from the given damager instance.
        /// </summary>
        /// <param name="damager">The damager instance.</param>
        /// <returns>Whether damage was taken.</returns>
        bool TryTakeDamage(IDamager damager)
        {
            var canTakeDamage = IsAbleToTakeDamage();
            if (canTakeDamage) TakeDamage(damager);
            return canTakeDamage;
        }

        /// <summary>
        /// Takes damage from the given damager instance.
        /// </summary>
        /// <param name="damager"><inheritdoc cref="TryTakeDamage(IDamager)" path="/param[@name='damager']"/></param>
        void TakeDamage(IDamager damager);

        /// <summary>
        /// Respawns this object reseting it.
        /// </summary>
        void Respawn();
    }
}