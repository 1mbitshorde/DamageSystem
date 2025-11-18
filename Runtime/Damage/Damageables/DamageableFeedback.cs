using UnityEngine;
using System.Linq;

namespace OneM.DamageSystem
{
    /// <summary>
    /// Shows damageable feedback by updating all renderers colors using the <see cref="color"/>.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class DamageableFeedback : MonoBehaviour
    {
        [SerializeField, Tooltip("The local Damageable component.")]
        private Damageable damageable;
        [Tooltip("The color to update when taking damage.")]
        public Color color = Color.red * 0.5F;
        [Tooltip("The animation total frames.")]
        public uint totalFrames = 60;
        [Tooltip("The time in seconds between the animation frames.")]
        public float framesTime = 0.2f;

        [Space]
        [SerializeField, Tooltip("Renderers to update using the damage color.")]
        private Renderer[] renderers;

        private void Reset() => FindComponents();
        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnsubscribeEvents();

        public bool IsPlaying() => !damageable.IsInvulnerable;

        public async void PlayDamageAnimation()
        {
            var originalColors = GetColors();
            damageable.IsInvulnerable = true;

            for (uint i = 0; i < totalFrames; i++)
            {
                SetColor(color);
                await Awaitable.WaitForSecondsAsync(framesTime);

                SetColors(originalColors);
                await Awaitable.WaitForSecondsAsync(framesTime);
            }

            damageable.IsInvulnerable = false;
        }

        private void FindComponents()
        {
            damageable = GetComponentInParent<Damageable>();
            renderers = GetComponentsInChildren<Renderer>();
        }

        private void SubscribeEvents() => damageable.OnDamageTaken += HandleDamageTaken;
        private void UnsubscribeEvents() => damageable.OnDamageTaken -= HandleDamageTaken;
        private void HandleDamageTaken(IDamager _) => PlayDamageAnimation();

        private Color[] GetColors() => renderers.Select(renderer => renderer.material.color).ToArray();

        private void SetColors(Color[] colors)
        {
            var isInvalid = colors.Length != renderers.Length;
            if (isInvalid) return;

            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = colors[i];
            }
        }

        private void SetColor(Color color)
        {
            foreach (var renderer in renderers)
            {
                renderer.material.color = color;
            }
        }
    }
}