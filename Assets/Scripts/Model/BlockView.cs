using DG.Tweening;
using Field.Data;
using Stats;
using UnityEngine;
using Zenject;

namespace Model
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        
        private Sequence _blockSequence;
        private IBlockAnimationSettings _blockAnimationSettings;
        private IHealthStats _healthStats;
        
        [Inject]
        public void Construct(IBlockAnimationSettings blockAnimationSettings, IHealthStats healthStats)
        {
            _blockAnimationSettings = blockAnimationSettings;
            _healthStats = healthStats;
        }
        
        public void OnCollisionEnter(Collision other)
        {
            BlockAnimation();
        }

        private void BlockAnimation()
        {
            _blockSequence.Kill();
            _blockSequence = DOTween.Sequence();
            
            _blockSequence
                .Append(transform
                    .DOPunchScale(Vector3.one * _blockAnimationSettings.PunchAmplitude, _blockAnimationSettings.PunchDuration, 12));
            
            _blockSequence
                .Join(transform
                    .DOShakePosition(_blockAnimationSettings.ShakeDuration, _blockAnimationSettings.ShakeAmplitude));
            
            _blockSequence
                .Join(_renderer.material
                    .DOColor(_blockAnimationSettings.FlashColor, _blockAnimationSettings.ColorDuration));

            _blockSequence
                .Append(_renderer.material
                    .DOColor(_blockAnimationSettings.OriginalColor, _blockAnimationSettings.ColorDuration));
        }
    }
}