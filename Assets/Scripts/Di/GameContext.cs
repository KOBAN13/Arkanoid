using Field;
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
        
        [SerializeField] private GameFieldSettings _gameFieldSettings;
        [SerializeField] private BallSettings _ballSettings;
        [SerializeField] private BlockAnimationSettings _blockAnimationSettings;
        [SerializeField] private BlockHealthSettings _blockHealthSettings;
        [SerializeField] private PlatformSettings _platformSettings;
        
        [SerializeField] private BallView _ballView;
        [SerializeField] private PlatformView _platformView;
        [SerializeField] private GameFieldService _gameFieldService;
        [SerializeField] private GameStateView _gameStateView;
        
        public override void InstallBindings()
        {
            BindField();
            BindData();
            BindPool();
            BindInput();
            BindBall();
            BindPlatform();
            BindUi();
            BindGameState();
        }

        private void BindGameState()
        {
            Container.BindInterfacesAndSelfTo<GameStateView>().FromInstance(_gameStateView).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameStateSystem>().AsSingle().NonLazy();
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
            Container.BindInterfacesAndSelfTo<GameFieldSettings>().FromScriptableObject(_gameFieldSettings).AsSingle().NonLazy();    
            Container.BindInterfacesAndSelfTo<BlockAnimationSettings>().FromScriptableObject(_blockAnimationSettings).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BlockHealthSettings>().FromScriptableObject(_blockHealthSettings).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PlatformSettings>().FromScriptableObject(_platformSettings).AsSingle().NonLazy();
        }

        private void BindField()
        {
            Container.BindInterfacesAndSelfTo<GameFieldService>().FromInstance(_gameFieldService).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MatrixService>().AsSingle().NonLazy();
        }

        private void BindPool()
        {
            Container.BindInterfacesAndSelfTo<GenericObjectPool<BlockView>>().AsSingle().NonLazy();
        }
    }
}