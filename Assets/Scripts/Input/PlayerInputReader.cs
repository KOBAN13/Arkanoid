using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class PlayerInputReader : DefaultInputActions.IPlayerActions, IInputReader, IDisposable
    {
        public event Action<Vector3> Move = delegate { };
        public event Action<bool> Jump = delegate { };
        public event Action TakeItem = delegate { };
        public event Action OpenInventory = delegate { };
        
        public Vector3 Direction
        {
            get
            {
                var direction = _playerInput.Player.Move.ReadValue<Vector2>();
                return new Vector3(direction.x, 0f, direction.y);
            }
        }
        
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
            Move?.Invoke(Direction);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _playerInput?.Dispose();
        }
    }
}
