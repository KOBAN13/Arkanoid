namespace Pool
{
    public interface IGenericObjectPool<T>
    {
        void Initialize(T effect);
        T GetObject();
        void ReturnObject(T comboEffectMover);
    }
}