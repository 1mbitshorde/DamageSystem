using System;
using UnityEngine;

namespace OneM.DamageSystem
{
    /// <summary>
    /// Interface used on objects able to be damaged.
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// Whether this instance is invulnerable to damage.
        /// </summary>
        bool IsInvulnerable { get; set; }

        /// <summary>
        /// The collider component used to check collisions.
        /// </summary>
        Collider Collider { get; set; }

        /// <summary>
        /// The Energy component to inflict damage.
        /// </summary>
        Energy Energy { get; set; }

        /// <summary>
        /// Event fired just after receive damage by the given damager.
        /// </summary>
        event Action<IDamager> OnDamageTaken;

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