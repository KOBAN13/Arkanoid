using UnityEngine;

namespace Field.Data
{
    [CreateAssetMenu(fileName = "GameFieldSettings", menuName = "Data/GameFieldSettings")]
    public class GameFieldSettings : ScriptableObject, IGameFieldSettings
    {
        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Height { get; private set; }
        [field: SerializeField] public Vector2Int BlockSize { get; private set; }
    }
}