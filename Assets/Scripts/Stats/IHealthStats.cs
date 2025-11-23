using R3;

namespace Stats
{
    public interface IHealthStats
    {
        int MaxValue { get; }
        int CurrentHealth { get; }
        Observable<Unit> OnHealthZero { get; }
        
        void ResetHealthStat();
        void SetDamage(int value);
        void AddHealth(int value);
    }
}