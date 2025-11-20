using UnityEngine;

namespace Field.Data
{
    public interface IGameFieldSettings
    {
        int Width { get; }
        int Height { get; }
        Vector2Int BlockSize { get; }
    }
}