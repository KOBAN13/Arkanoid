using System;
using Field;
using Input;
using Model;
using R3;
using R3.Triggers;
using Ui;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class GameStateSystem : IInitializable, IDisposable
    {
        private readonly IInputReader _inputReader;
        private readonly GameStateView _gameStateView;
        private readonly GameFieldService _gameFieldService;
        private readonly BallView _ballView;
        private readonly ArkanoidModule _arkanoidModule;

        private readonly CompositeDisposable _disposables = new();

        public GameStateSystem(
            GameFieldService gameFieldService,
            BallView ballView,
            ArkanoidModule arkanoidModule, 
            GameStateView gameStateView, IInputReader inputReader)
        {
            _gameFieldService = gameFieldService;
            _ballView = ballView;
            _arkanoidModule = arkanoidModule;
            _gameStateView = gameStateView;
            _inputReader = inputReader;
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
            _ballView.Stop();
            _arkanoidModule.ShowWin();
            
            Time.timeScale = 0;
        }

        private void HandleLose()
        {
            _ballView.Stop();
            _arkanoidModule.ShowLose();
            _inputReader.Dispose();
            
            Time.timeScale = 0;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}