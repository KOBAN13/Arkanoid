using Field.Matrix;
using UnityEngine;

namespace Field
{
    public interface IGameFieldService
    {
        void CreateBlock(Vector2Int position);
        void DeleteBlock(Vector2Int position);
        EFieldCellType GetCellType(Vector2Int position);
    }
}