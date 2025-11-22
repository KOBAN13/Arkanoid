using Field.Data;
using Field.Matrix;
using Input;
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
            BindField();
            BindData();
            BindPool();
            BindInput();
        }

        private void BindInput()
        {
            Container.BindInterfacesAndSelfTo<PlayerInput>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerInputReader>().AsSingle().NonLazy();
        }

        private void BindData()
        {
            Container.BindInterfacesAndSelfTo<GameFieldSettings>().FromScriptableObject(_gameFieldService).AsSingle().NonLazy();
        }

        private void BindField()
        {
            Container.BindInterfacesAndSelfTo<MatrixService>().AsSingle().NonLazy();
        }

        private void BindPool()
        {
            Container.BindInterfacesAndSelfTo<GenericObjectPool<BlockView>>().AsSingle().NonLazy();
        }
    }
}