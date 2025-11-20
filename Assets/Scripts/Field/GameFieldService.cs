using System.Collections.Generic;
using Field.Matrix;
using Model;
using Pool;
using UnityEngine;

namespace Field
{
    public class GameFieldService : IGameFieldService
    {
        private Matrix<EFieldCellType> _field;
        private readonly IGenericObjectPool<BlockView> _blockPool;
        private readonly Dictionary<Vector2Int, BlockView> _blocks = new();
        private Transform _blocksParent;

        public GameFieldService(IGenericObjectPool<BlockView> blockPool)
        {
            _blockPool = blockPool;
        }

        public void Initialize(Matrix<EFieldCellType> field, BlockView blockPrefab, Transform blocksParent)
        {
            _field = field;
            _blocksParent = blocksParent;

            _blockPool.Initialize(blockPrefab);
        }

        public void CreateBlock(Vector2Int position)
        {
            InitializeCell(position, EFieldCellType.Block);

            var blockView = _blockPool.GetObject();
            blockView.transform.SetParent(_blocksParent, false);
            blockView.transform.localPosition = new Vector3(position.x, position.y, 0f);

            _blocks[position] = blockView;
        }

        public void DeleteBlock(Vector2Int position)
        {
            DestoryCell(position);

            if (_blocks.Remove(position, out var blockView))
            {
                _blockPool.ReturnObject(blockView);
            }
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
