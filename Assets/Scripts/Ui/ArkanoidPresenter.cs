using System;
using R3;
using Zenject;

namespace Ui
{
    public class ArkanoidPresenter : IInitializable, IDisposable
    {
        private readonly ArkanoidModule _arkanoidModule;
        private readonly ArkanoidView _arkanoidView;
        
        private readonly CompositeDisposable _disposables = new();

        public ArkanoidPresenter(ArkanoidModule arkanoidModule, ArkanoidView arkanoidView)
        {
            _arkanoidModule = arkanoidModule;
            _arkanoidView = arkanoidView;
        }

        public void Initialize()
        {
            _arkanoidView.OnRestartButton.Subscribe(_ => _arkanoidModule.Restart()).AddTo(_disposables);
            
            _arkanoidModule.DescriptionText
                .Subscribe(_arkanoidView.UpdateDescriptionTextDisplay)
                .AddTo(_disposables);
            
            _arkanoidModule.CanvasGroupAlpha
                .Subscribe(_arkanoidView.UpdateCanvasGroupAlpha)
                .AddTo(_disposables);

            _arkanoidModule.ResultVisible
                .Subscribe(_arkanoidView.UpdateCanvasGroupInteractable)
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}