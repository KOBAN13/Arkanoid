using Input;
using Model;
using R3;
using UnityEngine;
using Zenject;

namespace System
{
    public class PlatformSystem : IInitializable, ITickable
    {
        private readonly IInputReader _inputReader;
        private readonly PlatformView _platformView;

        private float _moveSpeed = 5f;
        private Vector3 _delta;

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
            _delta = new Vector3(direction.x, direction.y, 0);
        }

        public void Tick()
        {
            _platformView.transform.Translate(_delta * _moveSpeed * Time.deltaTime);
        }
    }
}