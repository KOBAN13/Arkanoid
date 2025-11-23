using Field.Data;
using Model;
using R3;
using UnityEngine;
using Zenject;

namespace System // лучше переименовать в свой namespace, но оставляю как у тебя
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

            // Простой старт по направлению из настроек
            Launch();
            // ЛИБО можешь вызвать снаружи LaunchToPoint(target.position);
        }

        /// <summary>
        /// Ничего не делаем — всю физику считает Rigidbody.
        /// Главное: не переопределять скорость каждый кадр.
        /// </summary>
        public void FixedTick()
        {
            // Пусто
        }

        /// <summary>
        /// Реалистичный отскок с коэффициентом упругости и небольшим трением.
        /// </summary>
        private void HandleCollision(Collision collision)
        {
            if (collision.contactCount == 0)
                return;

            ContactPoint contact = collision.GetContact(0);
            Vector3 normal = contact.normal.normalized;

            // Чуть выталкиваем мяч из контакта, чтобы не застрял
            const float safePush = 0.01f;
            _ballView.SetPosition(contact.point + normal * safePush);

            Vector3 v = _ballView.Velocity;
            if (v.sqrMagnitude <= Mathf.Epsilon)
                return;

            // Разлагаем скорость на нормальную и касательную составляющие
            Vector3 vN = Vector3.Dot(v, normal) * normal;
            Vector3 vT = v - vN;

            // Коэффициенты — подбери под ощущение "реалистичности"
            const float bounciness       = 0.8f;  // 1 = абсолютно упругий удар
            const float tangentDamping   = 0.98f; // трение по поверхности

            // Переворачиваем нормальную составляющую и чуть затухаем касательную
            Vector3 reflected = -vN * bounciness + vT * tangentDamping;

            _ballView.SetVelocity(reflected);
        }

        /// <summary>
        /// Сбрасывает мяч в стартовую точку и останавливает.
        /// </summary>
        private void ResetBall()
        {
            _ballView.SetPosition(_startPoint.position);
            _ballView.Stop(); // предполагаю, что внутри обнуляет Rigidbody.velocity
        }

        /// <summary>
        /// Простой старт: летит прямо по направлению из настроек без прицела в точку.
        /// </summary>
        private void Launch()
        {
            Vector3 dir = _ballSettings.StartDirection.normalized;
            float speed = _ballSettings.StartSpeed;

            _ballView.SetVelocity(dir * speed);
        }

        /// <summary>
        /// Запускает мяч так, чтобы он долетел до конкретной точки
        /// по баллистической траектории с учётом гравитации Unity.
        ///
        /// highArc = false — низкая траектория
        /// highArc = true  — высокая дуга (если решение существует)
        /// </summary>
        public void LaunchToPoint(Vector3 targetPosition, bool highArc = false)
        {
            Vector3 origin = _startPoint.position;
            _ballView.SetPosition(origin);

            Vector3 toTarget = targetPosition - origin;

            float speed = _ballSettings.StartSpeed;
            float g = Mathf.Abs(Physics.gravity.y);

            // Разделяем на горизонтальную (по XZ) и вертикальную (Y) составляющие
            Vector3 toTargetXZ = new Vector3(toTarget.x, 0f, toTarget.z);
            float distanceXZ = toTargetXZ.magnitude;
            float heightDiff = toTarget.y; // target.y - origin.y

            // Точка почти над/под стартом — стреляем вертикально
            if (distanceXZ < 0.001f)
            {
                Vector3 verticalVelocity = Vector3.up * speed;
                _ballView.SetVelocity(verticalVelocity);
                return;
            }

            float speed2 = speed * speed;
            float speed4 = speed2 * speed2;

            // Под корнем из формулы баллистики:
            // v^4 - g (g R^2 + 2 h v^2)
            float underRoot =
                speed4 - g * (g * distanceXZ * distanceXZ + 2f * heightDiff * speed2);

            if (underRoot <= 0f)
            {
                // С текущей скоростью до точки по дуге не долететь.
                // Фоллбек: просто стреляем по прямой в сторону точки.
                Vector3 fallbackDir = toTarget.normalized;
                _ballView.SetVelocity(fallbackDir * speed);
                return;
            }

            float root = Mathf.Sqrt(underRoot);
            float gR = g * distanceXZ;

            // Два решения: "низкая" и "высокая" траектории
            float tanThetaLow  = (speed2 - root) / gR;
            float tanThetaHigh = (speed2 + root) / gR;

            float tanTheta = highArc ? tanThetaHigh : tanThetaLow;
            float angle = Mathf.Atan(tanTheta);

            Vector3 dirXZ = toTargetXZ.normalized;

            // Горизонтальная и вертикальная компоненты начальной скорости
            Vector3 launchVelocity =
                dirXZ * Mathf.Cos(angle) * speed +
                Vector3.up * Mathf.Sin(angle) * speed;

            _ballView.SetVelocity(launchVelocity);
        }
    }
}
