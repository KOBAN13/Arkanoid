using System;
using Field.Data;
using R3;
using UnityEngine;

namespace Stats
{
    public class BlockHealth : IHealthStats
    {
        public int MaxValue { get; private set; }
        public int CurrentHealth => _currentHealth.Value;
        public Observable<Unit> OnHealthZero => _onHealthZero;

        private readonly IBlockHealthSettings _config;

        private readonly ReactiveProperty<int> _currentHealth = new();
        private readonly Subject<Unit> _onHealthZero = new();
        
        public BlockHealth(IBlockHealthSettings config)
        {
            _config = config;
            
            MaxValue = _currentHealth.Value = _config.MaxValue;
        }

        public void ResetHealthStat()
        {
            MaxValue = _currentHealth.Value = _config.MaxValue;
        }

        public void SetDamage(int value)
        {
            if (value < 0)
                throw new ArgumentException("Value must be greater than 0");
            
            _currentHealth.Value = Mathf.Clamp(_currentHealth.Value - value, 0, MaxValue);

            if (_currentHealth.Value != 0)
                return;
            
            _onHealthZero.OnNext(Unit.Default);
        }

        public void AddHealth(int value)
        {
            if (value < 0)
                throw new ArgumentException("Value must be greater than 0");
            
            _currentHealth.Value = Mathf.Clamp(value + _currentHealth.Value, 0, MaxValue);
        }
    }
}