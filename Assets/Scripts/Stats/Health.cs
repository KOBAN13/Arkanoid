using System;
using Field.Data;
using R3;
using UnityEngine;

namespace Stats
{
    public class BlockHealth : IHealthStats
    {
        public float MaxValue { get; private set; }
        public float CurrentHealth => _currentHealth.Value;
        public Observable<Unit> OnHealthZero => _onHealthZero;

        private readonly IBlockHealthSettings _config;

        private readonly ReactiveProperty<float> _currentHealth = new();
        private readonly ReactiveProperty<float> _amountHealthPercentage = new();
        private readonly Subject<Unit> _onHealthZero = new();
        
        public BlockHealth(IBlockHealthSettings config)
        {
            _config = config;
            
            MaxValue = _currentHealth.Value = _config.MaxValue;
            _amountHealthPercentage.Value = 1f;
        }

        public void ResetHealthStat()
        {
            MaxValue = _currentHealth.Value = _config.MaxValue;
            _amountHealthPercentage.Value = 1f;
        }

        public void SetDamage(float value)
        {
            if (value < 0)
                throw new ArgumentException("Value must be greater than 0");
            
            _currentHealth.Value = Mathf.Clamp(_currentHealth.Value - value, 0f, MaxValue);

            _amountHealthPercentage.Value = Mathf.Clamp(_amountHealthPercentage.Value - value / MaxValue, 0f, 1f);

            if (_currentHealth.Value != 0f)
                return;
            
            _onHealthZero.OnNext(Unit.Default);
        }

        public void AddHealth(float value)
        {
            if (value < 0)
                throw new ArgumentException("Value must be greater than 0");
            
            _currentHealth.Value = Mathf.Clamp(value + _currentHealth.Value, 0f, MaxValue);

            _amountHealthPercentage.Value = Mathf.Clamp(_currentHealth.Value / MaxValue, 0f, 1f);
        }
    }
}