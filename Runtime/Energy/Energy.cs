using System;
using UnityEngine;

namespace OneM.DamageSystem
{
    /// <summary>
    /// Energy Component. Use on GameObjects able to have some type of energy.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class Energy : MonoBehaviour
    {
        [SerializeField, Tooltip("The initial energy.")]
        private float initial = 10f;
        [SerializeField, Tooltip("The maximum energy allowed.")]
        private float max = 10f;

        /// <summary>
        /// Event fired when energy has ended.
        /// </summary>
        public event Action OnEnergyEnded;

        /// <summary>
        /// Event fired when energy has changed.
        /// </summary>
        public event Action OnEnergyChanged;

        /// <summary>
        /// Event fired when energy has completed.
        /// </summary>
        public event Action OnEnergyCompleted;

        /// <summary>
        /// The initial energy.
        /// It'll be clamped between 0f and <see cref="Max"/>.
        /// </summary>
        public float Initial
        {
            get => initial;
            set => initial = Mathf.Clamp(value, 0f, Max);
        }

        /// <summary>
        /// The maximum energy allowed.
        /// </summary>
        public float Max
        {
            get => max;
            set => max = Mathf.Max(value, 0f);
        }

        /// <summary>
        /// The current energy.
        /// </summary>
        public float Current
        {
            get => current;
            private set
            {
                current = value;
                if (IsEmpty())
                {
                    current = 0f;
                    OnEnergyEnded?.Invoke();
                }
                else if (IsFull())
                {
                    current = Max;
                    OnEnergyCompleted?.Invoke();
                }

                OnEnergyChanged?.Invoke();
            }
        }

        private float current;

        private void Start() => CompleteToInitial();
        private void OnValidate() => ValidateFields();

        public bool IsFull() => Current > Max || Mathf.Approximately(Current, Max);
        public bool IsEmpty() => Current < 0f || Mathf.Approximately(Current, 0f);

        /// <summary>
        /// Completes the current energy to <see cref="Initial"/>.
        /// </summary>
        [ContextMenu("Complete")]
        public void CompleteToInitial() => Current = Initial;

        /// <summary>
        /// Adds the given amount into the current energy.
        /// </summary>
        /// <param name="amount">The energy amount to add.</param>
        public void Add(float amount) => Current += amount;

        /// <summary>
        /// Adds max energy amount completing it.
        /// </summary>
        public void Complete() => Add(Max);

        /// <summary>
        /// Removes the given amount from the current energy.
        /// </summary>
        /// <param name="amount">The energy amount to remove.</param>
        public void Remove(float amount) => Current -= amount;

        /// <summary>
        /// Removes the max energy amount.
        /// </summary>
        public void RemoveAll() => Remove(Max);

        private void ValidateFields()
        {
            Max = max;
            Initial = initial;
        }
    }
}