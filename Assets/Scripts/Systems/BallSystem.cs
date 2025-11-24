using Field.Data;
using Input;
using Model;
using R3;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class BallSystem : IInitializable, IFixedTickable, ITickable
    {
        private readonly BallView _ballView;
        private readonly IInputReader _inputReader;
        private readonly IBallSettings _ballSettings;

        private bool _isLaunched;
        private Transform _startPoint;
        
        private Vector3 _lastVelocity;
        private int _perpendicularReflectionCount;

        public BallSystem(BallView ballView, IBallSettings ballSettings, IInputReader inputReader)
        {
            _ballView = ballView;
            _ballSettings = ballSettings;
            _inputReader = inputReader;
        }

        public void Initialize()
        {
            _startPoint = _ballView.StartPoint;
            
            ResetBall();

            _inputReader.OnMouseLeftClick
                .Subscribe(LaunchToPoint)
                .AddTo(_ballView);

            _ballView.OnBallCollision
                .Subscribe(HandleCollision)
                .AddTo(_ballView);
        }

        public void FixedTick()
        {
            _lastVelocity = _ballView.Velocity;
        }
        
        public void Tick()
        {
            if (_isLaunched)
                return;
            
            var line = _ballView.Line;
            
            line.SetPosition(0, _ballView.BallTransform.position);
            
            var mousePosition = _ballView.Camera.ScreenToWorldPoint(new Vector3(
                UnityEngine.Input.mousePosition.x,
                UnityEngine.Input.mousePosition.y,
                10f
            ));

            line.SetPosition(1, mousePosition);
        }
        
        private void HandleCollision(Collision collision)
        {
            if (collision.contactCount == 0 || !_isLaunched)
                return;

            var contact = collision.GetContact(0);
            var normal = contact.normal.normalized;
            
            var incomingVelocity = _lastVelocity;
            
            if (incomingVelocity.sqrMagnitude < 0.05f)
                incomingVelocity = -normal * _ballSettings.StartSpeed;
            
            var reflected = Vector3.Reflect(incomingVelocity, normal);
            
            if (Mathf.Abs(Vector3.Dot(reflected.normalized, normal)) < 0.1f)
            {
                reflected = Quaternion.Euler(0, Random.Range(-10f, 10f), 0) * reflected;
            }
            
            var newVelocity = reflected.normalized * _ballSettings.StartSpeed;
            
            _ballView.SetVelocity(newVelocity);
            
            _lastVelocity = newVelocity;
        }


        private void ResetBall()
        {
            _ballView.transform.localPosition = _startPoint.localPosition;
            _isLaunched = false;

            _lastVelocity = Vector3.zero;
            _perpendicularReflectionCount = 0;
            _ballView.Line.enabled = true;
        }

        private void LaunchToPoint(Vector2 screenPoint)
        {
            if (_isLaunched) 
                return;
            
            _isLaunched = true;
            
            _ballView.Line.enabled = false;
            
            var depth = _ballView.Camera.WorldToScreenPoint(_startPoint.position).z;
            
            var target = _ballView.Camera.ScreenToWorldPoint(
                new Vector3(screenPoint.x, screenPoint.y, depth)
            );

            var origin = _startPoint.position;
            _ballView.SetPosition(origin);
            
            var toTarget = target - origin;
            var speed = _ballSettings.StartSpeed;
            var gravity = Mathf.Abs(Physics.gravity.y);
            var toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);
            var distanceXZ = toTargetXZ.magnitude;
            var heightDiff = toTarget.y;

            if (distanceXZ < Mathf.Epsilon)
            {
                _ballView.SetVelocity(Vector3.up * speed);
                return;
            }

            var v2 = speed * speed;
            var v4 = v2 * v2;
            var underRoot = v4 - gravity * (gravity * distanceXZ * distanceXZ + 2f * heightDiff * v2);

            if (underRoot <= 0f)
            {
                _ballView.SetVelocity(toTarget.normalized * speed);
                return;
            }

            var root = Mathf.Sqrt(underRoot);
            var gR = gravity * distanceXZ;
            var tanTheta = (v2 + root) / gR;
            var angle = Mathf.Atan(tanTheta);

            var dirXZ = toTargetXZ.normalized;

            var launchVelocity =
                dirXZ * Mathf.Cos(angle) * speed +
                Vector3.up * Mathf.Sin(angle) * speed;

            _ballView.SetVelocity(launchVelocity);

            _lastVelocity = launchVelocity;
        }
    }
}
