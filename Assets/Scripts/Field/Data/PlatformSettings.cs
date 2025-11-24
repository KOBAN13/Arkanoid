using UnityEngine;

namespace Field.Data
{
    [CreateAssetMenu(fileName = "PlatformSettings", menuName = "Data/PlatformSettings")]
    public class PlatformSettings : ScriptableObject, IPlatformSettings
    {
        [field: SerializeField] public float MoveSpeed { get; private set; }
    }
}