using System;
using System.Collections.Generic;
using Field.Data;
using Model;
using Pool;
using UnityEngine;
using Zenject;

namespace Field
{
    public class GameFieldService : MonoBehaviour
    {
        [SerializeField] private Transform _blocksParent;
        private IMatrixService _matrixService;
        private IGameFieldSettings _gameFieldSettings;
        private IGenericObjectPool<BlockView> _blockPool;
        private readonly Dictionary<Vector2Int, BlockView> _blocks = new();

        [Inject]
        public void Inject(
            IGameFieldSettings gameFieldSettings, 
            IMatrixService matrixService,
            IGenericObjectPool<BlockView> blockPool
        )
        {
            _gameFieldSettings = gameFieldSettings;
            _matrixService = matrixService;
            _blockPool = blockPool;
        }

        private void Awake()
        {
            _blockPool.Initialize(_gameFieldSettings.BlockPrefab);

            for (var x = 0; x < _gameFieldSettings.Height; x++)
            {
                for (var y = 0; y < _gameFieldSettings.Width; y++)
                {
                    CreateBlock(new Vector2Int(x, y));
                }
            }
        }

        public void CreateBlock(Vector2Int position)
        {
            _matrixService.CreateBlock(position);

            var blockView = _blockPool.GetObject();
            blockView.transform.SetParent(_blocksParent, false);
            blockView.transform.localPosition = new Vector3(position.x * 2f, position.y, 0f);

            _blocks[position] = blockView;
        }

        public void DeleteBlock(Vector2Int position)
        {
            _matrixService.DeleteBlock(position);

            if (_blocks.Remove(position, out var blockView))
            {
                _blockPool.ReturnObject(blockView);
            }
        }
    }
}