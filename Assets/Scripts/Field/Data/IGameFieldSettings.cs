using Model;
using UnityEngine;

namespace Field.Data
{
    public interface IGameFieldSettings
    {
        int Width { get; }
        int Height { get; }
        Vector2Int BlockSize { get; }
        Vector2Int BlockOffset { get; }
        BlockView BlockPrefab { get; }
    }
}