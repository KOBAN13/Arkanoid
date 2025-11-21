using Field.Data;
using System.Buffers;
using UnityEngine;
using Zenject;

namespace Field.Matrix
{
    public class MatrixService : IMatrixService, IInitializable
    {
        private Matrix<EFieldCellType> _field;
        
        private IGameFieldSettings _gameFieldSettings;

        public MatrixService(IGameFieldSettings gameFieldSettings)
        {
            _gameFieldSettings = gameFieldSettings;
        }

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

        public void Initialize()
        {
             var array = System.Buffers.ArrayPool<EFieldCellType>.Shared
                 .Rent(_gameFieldSettings.Width * _gameFieldSettings.Height);
             
             _field = new Matrix<EFieldCellType>(array, new Vector2Int(_gameFieldSettings.Width, _gameFieldSettings.Height));
        }
    }
}