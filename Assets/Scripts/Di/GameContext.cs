using Field.Data;
using Field.Matrix;
using Model;
using Pool;
using UnityEngine;
using Zenject;

namespace Di
{
    public class GameContext : MonoInstaller
    {
        [SerializeField] private GameFieldSettings _gameFieldService;
        
        public override void InstallBindings()
        {
            BindData();
            BindField();
            BindPool();
        }

        private void BindData()
        {
            Container.BindInterfacesAndSelfTo<GameFieldSettings>().AsSingle().NonLazy();
        }

        private void BindField()
        {
            Container.BindInterfacesAndSelfTo<MatrixService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameFieldSettings>().AsSingle().NonLazy();
        }

        private void BindPool()
        {
            Container.BindInterfacesAndSelfTo<GenericObjectPool<BlockView>>().AsSingle().NonLazy();
        }
    }
}