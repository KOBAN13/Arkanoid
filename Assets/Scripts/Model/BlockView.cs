using DG.Tweening;
using Field.Data;
using Stats;
using UnityEngine;
using Zenject;
using R3;

namespace Model
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        private readonly Subject<BlockView> _onDisappear = new();
        
        private Sequence _blockSequence;
        private Sequence _blockBreakSequence;
        private IBlockAnimationSettings _blockAnimationSettings;
        private IBlockHealthSettings _blockHealthSettings;
        private IHealthStats _healthStats;
        
        public Observable<BlockView> OnDisappear => _onDisappear;
        
        [Inject]
        public void Construct(IBlockAnimationSettings blockAnimationSettings, IBlockHealthSettings healthStats)
        {
            _blockAnimationSettings = blockAnimationSettings;
            _blockHealthSettings = healthStats;
            
            Initialize();
        }
        
        public void OnCollisionEnter(Collision other)
        {
            BlockAnimationHit();
            
            _healthStats.SetDamage(1);
        }

        private void BlockAnimationHit()
        {
            transform.DOKill();
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
        
        private void Initialize()
        {
            _healthStats = new BlockHealth(_blockHealthSettings);
            
            _healthStats.OnHealthZero.Subscribe(BlockAnimationBreak).AddTo(this);
        }

        private void BlockAnimationBreak(Unit unit)
        {
            var duration = 0.15f;
            
            transform.DOKill();
            _blockBreakSequence.Kill();
            _blockBreakSequence = DOTween.Sequence();

            _blockBreakSequence
                .Append(transform
                .DOScale(Vector3.zero, duration)
                .SetEase(Ease.InBack));
            
            _blockBreakSequence
                .Join(_renderer.material.DOFade(0f, duration));
            
            _blockBreakSequence
                .OnComplete(() => _onDisappear.OnNext(this));
        }
    }
}