
using UnityEngine;

namespace Field.Data
{
    public interface IBlockAnimationSettings
    {
        Color FlashColor { get; }
        Color OriginalColor { get; }
        
        float ShakeDuration { get; }
        float ShakeAmplitude { get; }
        float PunchDuration { get; }
        float PunchAmplitude { get; }
        float ColorDuration { get; }
        float BreakDuration { get; }
    }
}