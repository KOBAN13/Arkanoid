using System;
using UnityEngine;

namespace Model
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BallView : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        public Vector2 Velocity
        {
            get => _rigidbody.velocity;
            set => _rigidbody.velocity = value;
        }

        public Vector2 Position => _rigidbody.position;

        public event Action<Collision2D> CollisionEntered;

        private void Awake()
        {
            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody2D>();
            }
        }

        private void Reset()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            CollisionEntered?.Invoke(collision);
        }
    }
}
