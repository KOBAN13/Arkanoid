using UnityEngine;

namespace Field.Data
{
    [CreateAssetMenu(fileName = "BlockHealthSettings", menuName = "Data/BlockHealthSettings")]
    public class BlockHealthSettings : ScriptableObject, IBlockHealthSettings
    {
        [field: SerializeField] public int MaxValue { get; private set; }
    }
}