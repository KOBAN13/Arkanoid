using UnityEngine;

namespace Field.Data
{
    [CreateAssetMenu(fileName = "BlockAnimationSettings", menuName = "Data/BlockAnimationSettings")]
    public class BlockAnimationSettings : ScriptableObject, IBlockAnimationSettings
    {
        [field: SerializeField] public Color FlashColor { get; private set; }
        [field: SerializeField] public Color OriginalColor { get; private set; }
        [field: SerializeField] public float ShakeDuration { get; private set; }
        [field: SerializeField] public float ShakeAmplitude { get; private set; }
        [field: SerializeField] public float PunchDuration { get; private set; }
        [field: SerializeField] public float PunchAmplitude { get; private set; }
        [field: SerializeField] public float ColorDuration { get; private set; }
    }
}