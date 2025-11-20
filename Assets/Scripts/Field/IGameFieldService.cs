using Field.Matrix;
using Model;
using UnityEngine;

namespace Field
{
    public interface IGameFieldService
    {
        void Initialize(Matrix<EFieldCellType> field, BlockView blockPrefab, Transform blocksParent);
        void CreateBlock(Vector2Int position);
        void DeleteBlock(Vector2Int position);
        EFieldCellType GetCellType(Vector2Int position);
    }
}
