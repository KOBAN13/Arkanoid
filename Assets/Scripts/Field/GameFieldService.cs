using System;
using System.Collections.Generic;
using Field.Data;
using Field.Matrix;
using Model;
using Pool;
using R3;
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
        private CompositeDisposable _disposables = new();
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

        private void Start()
        {
            _blockPool.Initialize(_gameFieldSettings.BlockPrefab);

            var height = _matrixService.Field.Height;
            var width = _matrixService.Field.Width;

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    CreateBlock(new Vector2Int(x, y));
                }
            }
        }

        public void CreateBlock(Vector2Int position)
        {
            _matrixService.CreateBlock(position);

            var blockView = _blockPool.GetObject();

            blockView.Initialize();

            blockView.OnDisappear
                .Take(1)
                .Subscribe(_ => DeleteBlock(position))
                .AddTo(_disposables);
            
            blockView.transform.SetParent(_blocksParent, false);

            var blockSize = _gameFieldSettings.BlockSize;
            var blockOffset = _gameFieldSettings.BlockOffset;

            var stepX = blockSize.x + blockOffset.x;
            var stepY = blockSize.y + blockOffset.y;
    
            var blockPositionX = position.x * stepX;
            var blockPositionY = position.y * stepY;

            blockView.transform.localPosition = new Vector3(blockPositionX, blockPositionY, 0f);

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