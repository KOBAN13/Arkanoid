using UnityEngine;

namespace Field.Data
{
    [CreateAssetMenu(fileName = "BallSettings", menuName = "Data/BallSettings")]
    public class BallSettings : ScriptableObject, IBallSettings
    {
        [field: SerializeField] public float StartSpeed { get; private set; }
        [field: SerializeField] public float MinimumVerticalDot { get; private set; }
        [field: SerializeField] public Vector3 StartDirection { get; private set; }
    }
}