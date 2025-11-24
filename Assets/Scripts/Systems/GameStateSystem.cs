using System;
using Field;
using Model;
using R3;
using R3.Triggers;
using Ui;
using Zenject;

namespace Systems
{
    public class GameStateSystem : IInitializable, IDisposable
    {
        private readonly GameStateView _gameStateView;
        private readonly GameFieldService _gameFieldService;
        private readonly BallView _ballView;
        private readonly ArkanoidModule _arkanoidModule;

        private readonly CompositeDisposable _disposables = new();
        private bool _isGameFinished;

        public GameStateSystem(
            GameFieldService gameFieldService,
            BallView ballView,
            ArkanoidModule arkanoidModule, 
            GameStateView gameStateView
        )
        {
            _gameFieldService = gameFieldService;
            _ballView = ballView;
            _arkanoidModule = arkanoidModule;
            _gameStateView = gameStateView;
        }

        public void Initialize()
        {
            _arkanoidModule.HideResult();

            _gameFieldService.OnAllBlocksDestroyed
                .Subscribe(_ => HandleWin())
                .AddTo(_disposables);
            
            _gameStateView.GameLoseTrigger
                .OnTriggerEnterAsObservable()
                .Subscribe(_ => HandleLose())
                .AddTo(_disposables);
        }

        private void HandleWin()
        {
            if (_isGameFinished)
                return;

            _isGameFinished = true;
            _ballView.Stop();
            _arkanoidModule.ShowWin();
        }

        private void HandleLose()
        {
            if (_isGameFinished)
                return;

            _isGameFinished = true;
            _ballView.Stop();
            _arkanoidModule.ShowLose();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}