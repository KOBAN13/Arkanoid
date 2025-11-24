using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class ArkanoidView : MonoBehaviour
    {
        [field: SerializeField] private CanvasGroup _canvasGroup;
        [field: SerializeField] private TextMeshProUGUI _descriptionText;
        [field: SerializeField] private Button _restartButton;
        
        public readonly Subject<Unit> OnRestartButton = new();

        private void Start()
        {
            InitializeUi();
        }

        private void InitializeUi()
        {
            _restartButton.onClick
                .AsObservable()
                .Subscribe(_ => OnRestartButtonClick())
                .AddTo(this);
        }
        
        private void OnRestartButtonClick()
        {
            OnRestartButton.OnNext(Unit.Default);
        }
        
        public void UpdateDescriptionTextDisplay(string description)
            => _descriptionText.text = description;
        
        public void UpdateCanvasGroupAlpha(float alpha)
            => _canvasGroup.alpha = alpha;
        
        public void UpdateCanvasGroupInteractable(bool isVisible)
        {
            _canvasGroup.interactable = isVisible;
            _canvasGroup.blocksRaycasts = isVisible;
        }
    }
}