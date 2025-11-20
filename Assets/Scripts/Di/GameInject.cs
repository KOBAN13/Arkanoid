using Field;
using Model;
using Pool;
using Zenject;

namespace Di
{
    public class GameContext : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindField();
            BindPool();
        }

        private void BindField()
        {
            Container.BindInterfacesAndSelfTo<GameFieldService>().AsSingle().NonLazy();
        }

        private void BindPool()
        {
            Container.BindInterfacesAndSelfTo<GenericObjectPool<BlockView>>().AsSingle().NonLazy();
        }
    }
}