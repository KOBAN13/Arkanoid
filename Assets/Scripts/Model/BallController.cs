using UnityEngine;

namespace Model
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] private BallView _view;
        [SerializeField] private float _speed = 6f;
        [SerializeField, Range(0f, 1f)] private float _minVerticalDot = 0.2f;
        [SerializeField] private Vector2 _initialDirection = new(0.5f, 1f);

        private void Awake()
        {
            if (_view == null)
            {
                _view = GetComponent<BallView>();
            }
        }

        private void OnEnable()
        {
            if (_view != null)
            {
                _view.CollisionEntered += OnCollision;
            }
        }

        private void OnDisable()
        {
            if (_view != null)
            {
                _view.CollisionEntered -= OnCollision;
            }
        }

        private void Start()
        {
            Launch(_initialDirection);
        }

        public void Launch(Vector2 direction)
        {
            var normalized = NormalizeDirection(direction);
            _view.Velocity = normalized * _speed;
        }

        private void OnCollision(Collision2D collision)
        {
            if (collision.contactCount == 0)
            {
                return;
            }

            var contact = collision.GetContact(0);
            var incoming = _view.Velocity.normalized;
            var reflected = Vector2.Reflect(incoming, contact.normal);
            reflected = NormalizeDirection(reflected);

            _view.Velocity = reflected * _speed;
        }

        private Vector2 NormalizeDirection(Vector2 direction)
        {
            if (direction == Vector2.zero)
            {
                direction = Vector2.up;
            }

            direction.Normalize();

            var verticalDot = Mathf.Abs(Vector2.Dot(direction, Vector2.up));
            if (verticalDot < _minVerticalDot)
            {
                var verticalSign = Mathf.Sign(direction.y);
                if (Mathf.Approximately(verticalSign, 0f))
                {
                    verticalSign = 1f;
                }

                var horizontalSign = Mathf.Sign(direction.x);
                if (Mathf.Approximately(horizontalSign, 0f))
                {
                    horizontalSign = 1f;
                }

                var horizontalComponent = Mathf.Sqrt(Mathf.Clamp01(1f - _minVerticalDot * _minVerticalDot));
                direction = new Vector2(horizontalComponent * horizontalSign, _minVerticalDot * verticalSign);
            }

            return direction.normalized;
        }
    }
}
