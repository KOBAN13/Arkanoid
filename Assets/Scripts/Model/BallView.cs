using UnityEngine;

namespace Model
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallView : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        public Vector3 Position => transform.position;
        public Vector3 Velocity => _rigidbody.angularVelocity;

        private void Awake()
        {
            EnsureRigidbody();
            DisableGravity();
        }

        private void OnValidate()
        {
            EnsureRigidbody();
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetVelocity(Vector3 velocity)
        {
            _rigidbody.angularVelocity = velocity;
        }

        public void Stop()
        {
            SetVelocity(Vector3.zero);
        }

        public void AddImpulse(Vector3 impulse)
        {
            if (_rigidbody == null)
            {
                return;
            }

            _rigidbody.AddForce(impulse, ForceMode.VelocityChange);
        }

        public Vector3 Reflect(Vector3 inDirection, Vector3 normal, float speed)
        {
            var reflectedDirection = Vector3.Reflect(inDirection.normalized, normal);
            SetVelocity(reflectedDirection * speed);
            return reflectedDirection;
        }

        private void EnsureRigidbody()
        {
            if (_rigidbody == null)
            {
                TryGetComponent(out _rigidbody);
            }
        }

        private void DisableGravity()
        {
            if (_rigidbody != null)
            {
                _rigidbody.useGravity = false;
            }
        }
    }
}