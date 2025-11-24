using Field.Data;
using Input;
using Model;
using R3;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class PlatformSystem : IInitializable, ITickable
    {
        private readonly IInputReader _inputReader;
        private readonly PlatformView _platformView;
        private readonly IPlatformSettings _platformSettings;
        
        private Vector3 _delta;

        public PlatformSystem(IInputReader inputReader, PlatformView platformView, IPlatformSettings platformSettings)
        {
            _inputReader = inputReader;
            _platformView = platformView;
            _platformSettings = platformSettings;
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
            var move = _delta * _platformSettings.MoveSpeed * Time.deltaTime;
            var origin = _platformView.transform.position;
            
            if (Mathf.Approximately(move.x, 0f))
                return;

            var directionX = Mathf.Sign(move.x) * _platformView.ColliderSizeX / 2f;
            var direction = new Vector3(directionX, 0, 0);
            
            if (!Physics.Raycast(origin, direction,Mathf.Abs(directionX)))
            {
                _platformView.transform.Translate(move, Space.World);
            }
        }
    }
}