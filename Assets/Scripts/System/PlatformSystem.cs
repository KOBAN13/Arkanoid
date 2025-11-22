using Input;
using Model;
using R3;
using UnityEngine;
using Zenject;

namespace System
{
    public class PlatformSystem : IInitializable
    {
        private readonly IInputReader _inputReader;
        private readonly PlatformView _platformView;

        public PlatformSystem(IInputReader inputReader, PlatformView platformView)
        {
            _inputReader = inputReader;
            _platformView = platformView;
        }
        
        public void Initialize()
        {
            _inputReader.Move.Subscribe(HandleMove).AddTo(_platformView);
        }
        
        private void HandleMove(Vector2 direction)
        {
            Debug.Log(direction);
            
            _platformView.transform.Translate(direction);
        }
    }
}