using UnityEngine;

namespace Field.Data
{
    [CreateAssetMenu(fileName = "BlockHealthSettings", menuName = "Data/BlockHealthSettings")]
    public class BlockHealthSettings : ScriptableObject, IBlockHealthSettings
    {
        [field: SerializeField] public float MaxValue { get; private set; }
    }
}