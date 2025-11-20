using Field.Matrix;
using UnityEngine;

namespace Field
{
    public class GameFieldService : IGameFieldService
    {
        private Matrix<EFieldCellType> _field;
        
        public void CreateBlock(Vector2Int position)
        {
            InitializeCell(position, EFieldCellType.Block);
        }

        public void DeleteBlock(Vector2Int position)
        {
            DestoryCell(position);
        }

        public EFieldCellType GetCellType(Vector2Int position)
        {
            return _field[position];
        }

        private void InitializeCell(Vector2Int position, EFieldCellType cellType)
        {
            _field[position] = cellType;
        }

        private void DestoryCell(Vector2Int position)
        {
            _field[position] = EFieldCellType.Empty;
        }
    }
}