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
        private readonly IBallSettings _ballSettings;
        private Transform _startPoint;

        public BallSystem(BallView ballView, IBallSettings ballSettings)
        {
            _ballView = ballView;
            _ballSettings = ballSettings;
        }

        public void Initialize()
        {
            _startPoint = _ballView.StartPoint;

            _ballView.OnBallCollision
                .Subscribe(HandleCollision)
                .AddTo(_ballView);

            ResetBall();
            Launch();
        }

        public void FixedTick()
        {
            var vel = _ballView.Velocity;
            
            vel = vel.normalized * _ballSettings.StartSpeed;

            _ballView.SetVelocity(vel);
        }

        private void HandleCollision(Collision collision)
        {
            if (collision.contactCount == 0)
                return;

            ContactPoint contact = collision.GetContact(0);
            Vector3 normal = contact.normal;
            
            float safePush = _ballSettings.Skin;
            _ballView.SetPosition(contact.point + normal * safePush);
            
            Vector3 vel = _ballView.Velocity;

            if (vel.sqrMagnitude < 0.0001f)
                vel = -normal;

            Vector3 reflected = Vector3.Reflect(vel, normal);
            reflected.z = 0;
            
            float yAbs = Mathf.Abs(reflected.y);

            if (yAbs < _ballSettings.MinimumVerticalDot)
                reflected.y = Mathf.Sign(reflected.y) * _ballSettings.MinimumVerticalDot;

            if (yAbs > _ballSettings.MaximumVerticalDot)
                reflected.y = Mathf.Sign(reflected.y) * _ballSettings.MaximumVerticalDot;

            float xAbs = Mathf.Abs(reflected.x);
            if (xAbs < _ballSettings.MinimumHorizontalDot)
                reflected.x = Mathf.Sign(reflected.x) * _ballSettings.MinimumHorizontalDot;
            
            reflected.Normalize();
            
            _ballView.SetVelocity(reflected * _ballSettings.StartSpeed);
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
