using Field.Data;
using Model;
using R3;
using UnityEngine;
using Zenject;

namespace System
{
    public class BallSystem : IInitializable, IFixedTickable
    {
        private readonly BallView _ballView;
        private Transform _startPoint;
        
        private readonly IBallSettings _ballSettings;

        public BallSystem(BallView ballView, IBallSettings ballSettings)
        {
            _ballView = ballView;
            _ballSettings = ballSettings;
        }

        public void Initialize()
        {
            _startPoint = _ballView.StartPoint;
            
            _ballView.OnBallCollision.Subscribe(HandleCollision).AddTo(_ballView);
            
            ResetBall();
            Launch();
        }

        public void FixedTick()
        {
            var velocity = _ballView.Velocity;
            
            if (velocity.sqrMagnitude > Mathf.Epsilon)
                _ballView.SetVelocity(velocity.normalized * _ballSettings.StartSpeed);
        }

        private void HandleCollision(Collision collision)
        {
            Debug.LogError("Collision");
            
            if (collision.contactCount == 0) 
                return;

            var contact = collision.GetContact(0);
            var normal = contact.normal;

            var direction = Vector3.Reflect(_ballView.Velocity.normalized, normal);
            
            if (Mathf.Abs(direction.y) < _ballSettings.MinimumVerticalDot)
            {
                direction.y = Mathf.Sign(direction.y == 0 ? _ballView.Velocity.y : direction.y)
                              * _ballSettings.MinimumVerticalDot;
                
                direction.Normalize();
            }

            _ballView.SetVelocity(direction * _ballSettings.StartSpeed);
        }

        private void ResetBall()
        {
            if (_startPoint)
                _ballView.SetPosition(_startPoint.position);

            _ballView.Stop();
        }

        private void Launch()
        {
            _ballView.SetVelocity(_ballSettings.StartDirection * _ballSettings.StartSpeed);
        }
    }
}
