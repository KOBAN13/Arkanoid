using R3;
using UnityEngine.SceneManagement;

namespace Ui
{
    public class ArkanoidModule
    {
        private readonly ReactiveProperty<string> _descriptionText = new(string.Empty);
        private readonly ReactiveProperty<float> _canvasGroupAlpha = new(0f);
        private readonly ReactiveProperty<bool> _resultVisible = new(false);
        
        public Observable<string> DescriptionText => _descriptionText;
        public Observable<float> CanvasGroupAlpha => _canvasGroupAlpha;
        public Observable<bool> ResultVisible => _resultVisible;

        public void ShowWin()
            => ShowResult("Победа! Отличная работа!");

        public void ShowLose()
            => ShowResult("Поражение! Попробуйте ещё раз.");

        public void HideResult()
        {
            _canvasGroupAlpha.Value = 0f;
            _resultVisible.Value = false;
            _descriptionText.Value = string.Empty;
        }

        public void Restart()
            => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        private void ShowResult(string description)
        {
            _descriptionText.Value = description;
            _canvasGroupAlpha.Value = 1f;
            _resultVisible.Value = true;
        }
    }
}