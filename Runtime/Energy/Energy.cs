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
        [SerializeField, Tooltip("The minimum energy allowed.")]
        private float min = 0f;
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
        /// It'll be clamped between <see cref="Min"/> and <see cref="Max"/>.
        /// </summary>
        public float Initial
        {
            get => initial;
            set => initial = Mathf.Clamp(value, Min, Max);
        }

        /// <summary>
        /// The minimum energy allowed.
        /// It cannot be greater than <see cref="Max"/>.
        /// </summary>
        public float Min
        {
            get => min;
            set => min = Mathf.Min(value, Max);
        }

        /// <summary>
        /// The maximum energy allowed.
        /// It cannot be lower than <see cref="Min"/>.
        /// </summary>
        public float Max
        {
            get => max;
            set => max = Mathf.Max(value, Min);
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
                    current = Min;
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
        public bool IsEmpty() => Current < Min || Mathf.Approximately(Current, Min);

        /// <summary>
        /// Completes the current energy to <see cref="Initial"/>.
        /// </summary>
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
            Min = min;
            Initial = initial;
        }
    }
}