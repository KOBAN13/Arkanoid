using Model;
using UnityEngine;
using Zenject;

namespace System
{
    public class BallSystem : IInitializable, IFixedTickable
    {
        [SerializeField] private BallView _ballView;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private float _startSpeed = 8f;
        [SerializeField] private Vector3 _startDirection = new Vector3(0.6f, 1f, 0f);
        [SerializeField] private float _minimumVerticalDot = 0.2f;
        
        public void Initialize()
        {
            ResetBall();
            Launch();
        }
        
        public void FixedTick()
        {
            var velocity = _ballView.Velocity;
            
            if (velocity.sqrMagnitude <= Mathf.Epsilon)
            {
                return;
            }

            _ballView.SetVelocity(velocity.normalized * _startSpeed);
        }
        

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.contactCount == 0)
            {
                return;
            }

            var contact = collision.GetContact(0);
            var nextDirection = Vector3.Reflect(_ballView.Velocity.normalized, contact.normal);

            if (Mathf.Abs(nextDirection.y) < _minimumVerticalDot)
            {
                nextDirection.y = Mathf.Sign(nextDirection.y == 0 ? _ballView.Velocity.y : nextDirection.y) * _minimumVerticalDot;
                nextDirection = nextDirection.normalized;
            }

            _ballView.SetVelocity(nextDirection * _startSpeed);
        }

        public void ResetBall()
        {
            if (_startPoint != null)
            {
                _ballView.SetPosition(_startPoint.position);
            }

            _ballView.Stop();
        }

        public void Launch()
        {
            var direction = _startDirection == Vector3.zero ? Vector3.up : _startDirection.normalized;
            _ballView.SetVelocity(direction * _startSpeed);
        }
    }
}