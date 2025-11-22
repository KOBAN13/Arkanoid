using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class PlayerInputReader : DefaultInputActions.IPlayerActions, IInputReader, IDisposable
    {
        private readonly ReactiveProperty<Vector2> _move = new();
        public Observable<Vector2> Move => _move;
        
        private readonly PlayerInput _playerInput;

        public PlayerInputReader(PlayerInput playerInput)
        {
            _playerInput = playerInput;
        }

        public void EnablePlayerAction()
        {
            _playerInput.Enable();
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _move.Value = new Vector3(direction.x, direction.y, 0f);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            
        }

        public void Dispose()
        {
            _playerInput?.Dispose();
        }
    }
}
