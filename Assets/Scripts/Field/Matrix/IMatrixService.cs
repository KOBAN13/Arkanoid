using UnityEngine;

namespace Field.Matrix
{
    public interface IMatrixService
    {
        Matrix<EFieldCellType> Field { get; }
        
        void CreateBlock(Vector2Int position);
        void DeleteBlock(Vector2Int position);
        EFieldCellType GetCellType(Vector2Int position);
    }
}