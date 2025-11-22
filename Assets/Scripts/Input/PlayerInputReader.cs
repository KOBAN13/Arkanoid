using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Input
{
    public class PlayerInputReader : PlayerInput.IMoveActions, IInputReader, IDisposable, IInitializable
    {
        private readonly ReactiveProperty<Vector2> _move = new();
        public Observable<Vector2> Move => _move;
        
        private readonly PlayerInput _playerInput;

        public PlayerInputReader(PlayerInput playerInput)
        {
            _playerInput = playerInput;
        }
        
        public void OnMovePlatform(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _move.Value = direction;
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
    }
}
