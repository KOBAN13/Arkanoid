using Field.Matrix;
using UnityEngine;

namespace Field
{
    public interface IMatrixService
    {
        void CreateBlock(Vector2Int position);
        void DeleteBlock(Vector2Int position);
        EFieldCellType GetCellType(Vector2Int position);
    }
}