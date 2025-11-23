using R3;
using UnityEngine;

namespace Model
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Camera _camera;
        [SerializeField] private LineRenderer _line;
        
        public Camera Camera => _camera;
        public LineRenderer Line => _line;
        public Vector3 Velocity => _rigidbody.linearVelocity;
        public Transform StartPoint => _startPoint;
        public Transform BallTransform => transform;
        public Observable<Collision> OnBallCollision => _onBallCollision;
        
        private readonly Subject<Collision> _onBallCollision = new();
        
        private void Awake()
        {
            _rigidbody.useGravity = false;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetVelocity(Vector3 velocity)
        {
            _rigidbody.linearVelocity = velocity;
        }

        public void Stop()
        {
            SetVelocity(Vector3.zero);
        }

        private void OnCollisionEnter(Collision collision)
        {
            _onBallCollision?.OnNext(collision);
        }
    }
}