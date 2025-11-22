using R3;

namespace Stats
{
    public interface IHealthStats
    {
        float MaxValue { get; }
        float CurrentHealth { get; }
        Observable<Unit> OnHealthZero { get; }
        
        void ResetHealthStat();
        void SetDamage(float value);
        void AddHealth(float value);
    }
}