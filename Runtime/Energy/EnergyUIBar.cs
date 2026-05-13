#if PACKAGE_UGUI
using UnityEngine;
using UnityEngine.UI;

namespace OneM.DamageSystem
{
    /// <summary>
    /// Draws an Energy Bar using a <see cref="Slider"/> Component.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Slider))]
    public sealed class EnergyUIBar : MonoBehaviour
    {
        [SerializeField, Tooltip("The local Slider component.")]
        private Slider slider;
        [SerializeField, Tooltip("The image representing the Energy Bar.")]
        private Graphic bar;
        [SerializeField, Tooltip("The Energy component.")]
        private Energy energy;

        [Space]
        [Tooltip("The Gradient used to update the Energy Bar color.")]
        public Gradient color;

        /// <summary>
        /// The Energy Component.
        /// </summary>
        public Energy Energy
        {
            get => energy;
            set
            {
                UnsubscribeEvents();

                energy = value;

                SubscribeEvents();
                InitializeSlider();
            }
        }

        private void Reset() => Setup();
        private void Awake() => InitializeSlider();
        private void OnEnable() => SubscribeEvents();
        private void OnDisable() => UnsubscribeEvents();
        private void OnDrawGizmos() => SetSliderValue(slider.value);

        private void SubscribeEvents()
        {
            if (Energy) Energy.OnEnergyChanged += HandleEnergyChanged;
        }

        private void UnsubscribeEvents()
        {
            if (Energy) Energy.OnEnergyChanged -= HandleEnergyChanged;
        }

        private void HandleEnergyChanged() => SetSliderValue(Energy.Current);

        private void InitializeSlider()
        {
            if (Energy == null) return;

            slider.minValue = 0f;
            slider.maxValue = Energy.Max;

            SetSliderValue(Energy.Current);
        }

        private void SetSliderValue(float value)
        {
            slider.value = value;
            if (bar) bar.color = color.Evaluate(slider.normalizedValue);
        }

        private void Setup()
        {
            slider = GetComponent<Slider>();
            bar = slider.fillRect.GetComponent<Graphic>();

            SetupColorGradient();
        }

        private void SetupColorGradient()
        {
            color ??= new Gradient();

            var gradientColor = bar != null ? bar.color : Color.white;
            var colorKeys = new GradientColorKey[] { new(gradientColor, 0F) };
            var alphaKeys = new GradientAlphaKey[] { new(1F, 0F) };

            color.SetKeys(colorKeys, alphaKeys);
        }
    }
}
#endif