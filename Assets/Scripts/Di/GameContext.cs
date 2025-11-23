using System;
using Field.Data;
using Field.Matrix;
using Input;
using Model;
using Pool;
using Systems;
using Ui;
using UnityEngine;
using Zenject;

namespace Di
{
    public class GameContext : MonoInstaller
    {
        [SerializeField] private ArkanoidView _arkanoidView;
        
        [SerializeField] private GameFieldSettings _gameFieldService;
        [SerializeField] private BallSettings _ballSettings;
        [SerializeField] private BlockAnimationSettings _blockAnimationSettings;
        [SerializeField] private BlockHealthSettings _blockHealthSettings;
        
        [SerializeField] private BallView _ballView;
        [SerializeField] private PlatformView _platformView;
        
        public override void InstallBindings()
        {
            BindField();
            BindData();
            BindPool();
            BindInput();
            BindBall();
            BindPlatform();
            BindUi();
        }

        private void BindUi()
        {
            Container.BindInterfacesAndSelfTo<ArkanoidView>().FromInstance(_arkanoidView).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ArkanoidPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<ArkanoidModule>().AsSingle().NonLazy();
        }

        private void BindPlatform()
        {
            Container.BindInterfacesAndSelfTo<PlatformView>().FromInstance(_platformView).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlatformSystem>().AsSingle().NonLazy();
        }

        private void BindBall()
        {
            Container.BindInterfacesAndSelfTo<BallView>().FromInstance(_ballView).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BallSystem>().AsSingle().NonLazy();
        }

        private void BindInput()
        {
            Container.BindInterfacesAndSelfTo<PlayerInput>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerInputReader>().AsSingle().NonLazy();
        }

        private void BindData()
        {
            Container.BindInterfacesAndSelfTo<BallSettings>().FromScriptableObject(_ballSettings).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameFieldSettings>().FromScriptableObject(_gameFieldService).AsSingle().NonLazy();    
            Container.BindInterfacesAndSelfTo<BlockAnimationSettings>().FromScriptableObject(_blockAnimationSettings).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BlockHealthSettings>().FromScriptableObject(_blockHealthSettings).AsSingle().NonLazy();
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