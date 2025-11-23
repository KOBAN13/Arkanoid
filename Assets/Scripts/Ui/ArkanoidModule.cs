using R3;

namespace Ui
{
    public class ArkanoidModule
    {
        private readonly ReactiveProperty<string> _descriptionText = new();
        private readonly ReactiveProperty<float> _canvasGroupAlpha = new();
        
        public Observable<string> DescriptionText => _descriptionText;
        public Observable<float> CanvasGroupAlpha => _canvasGroupAlpha;
        
        public void Restart()
        {
            
        }
    }
}