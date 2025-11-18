using System;
using UnityEngine;

namespace OneM.DamageSystem
{
    /// <summary>
    /// Creates an intersection point when a damage is made.
    /// <para> Use <see cref="OnIntersected"/> event.</para>
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Damager))]
    public sealed class DamagerIntersector : MonoBehaviour
    {
        [SerializeField] private Damager damager;

        /// <summary>
        /// Event fired when an intersection is made.
        /// <para>The first param is the intersection point.</para>
        /// </summary>
        public event Action<Vector3, IDamageable> OnIntersected;

        private void Reset() => damager = GetComponent<Damager>();
        private void OnEnable() => damager.OnDamageInflicted += HandleDamageInflicted;
        private void OnDisable() => damager.OnDamageInflicted -= HandleDamageInflicted;

        private void HandleDamageInflicted(IDamageable damageable)
        {
            var intersection = damager.Collider.ClosestPoint(damageable.Collider.bounds.center);
            OnIntersected?.Invoke(intersection, damageable);
        }
    }
}