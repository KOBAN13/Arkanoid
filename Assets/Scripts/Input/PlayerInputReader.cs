using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Input
{
    public class PlayerInputReader : IInputReader, IDisposable, IInitializable, ITickable
    {
        private readonly ReactiveProperty<Vector2> _move = new();
        public Observable<Vector2> Move => _move;
        
        private readonly PlayerInput _playerInput;

        public PlayerInputReader(PlayerInput playerInput)
        {
            _playerInput = playerInput;
        }

        public void Dispose()
        {
            _playerInput?.Dispose();
        }

        public void Initialize()
        {
            EnablePlayerAction();
        }
        
        private void EnablePlayerAction()
        {
            _playerInput.Enable();
        }

        public void Tick()
        {
            var direction = _playerInput.Move.MovePlatform.ReadValue<Vector2>();
            
            _move.Value = direction;
        }
    }
}
