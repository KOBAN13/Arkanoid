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
        private readonly Subject<Vector2> _onMouseLeftClick = new();
        public Observable<Vector2> OnMouseLeftClick => _onMouseLeftClick;
        public Observable<Vector2> Move => _move;

        private readonly PlayerInput _playerInput;

        public PlayerInputReader(PlayerInput playerInput)
        {
            _playerInput = playerInput;
        }
        
        ~PlayerInputReader()
        {
            _playerInput?.Dispose();
        }

        public void Dispose()
        {
            _playerInput.Move.MouseLeftClick.performed -= MouseLeftClick;
            
            _playerInput.Dispose();
        }

        public void Initialize()
        {
            _playerInput.Enable();
            
            _playerInput.Move.MouseLeftClick.performed += MouseLeftClick;
        }

        private void MouseLeftClick(InputAction.CallbackContext obj)
        {
            var mousePosition = _playerInput.Move.MousePosition.ReadValue<Vector2>();
            _onMouseLeftClick.OnNext(mousePosition);
        }

        public void Tick()
        {
            var direction = _playerInput.Move.MovePlatform.ReadValue<Vector2>();
            
            _move.Value = direction;
        }
    }
}
