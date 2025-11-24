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

        private readonly CompositeDisposable _disposables = new();
        private readonly Dictionary<Vector2Int, BlockView> _blocks = new();

        private readonly Subject<Unit> _onAllBlocksDestroyed = new();
        public Observable<Unit> OnAllBlocksDestroyed => _onAllBlocksDestroyed;

        private int _aliveBlocks;

        private float _stepX;
        private float _stepY;
        private Vector2 _startOffset;
        private int _gridWidth;
        private int _gridHeight;

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

            var blockSize = _gameFieldSettings.BlockSize;
            var blockOffset = _gameFieldSettings.BlockOffset;

            _stepX = blockSize.x + blockOffset.x;
            _stepY = blockSize.y + blockOffset.y;

            var mainCamera = Camera.main;
            var worldHeight = mainCamera.orthographicSize * 2f;
            var worldWidth = worldHeight * mainCamera.aspect;
            
            var maxWidth = Mathf.FloorToInt(worldWidth / _stepX);
            var maxHeight = Mathf.FloorToInt(worldHeight / _stepY);

            var originalWidth = _matrixService.Field.Width;
            var originalHeight = _matrixService.Field.Height;

            _gridWidth = Mathf.Min(originalWidth, maxWidth);
            _gridHeight = Mathf.Min(originalHeight, maxHeight);
            
            var fullRowWidth = _gridWidth * _stepX;
            
            var top = worldHeight / 2f;
            
            _startOffset = new Vector2(
                -fullRowWidth / 2f + _stepX / 2f,
                top - _stepY / 2f                
            );
            
            for (var x = 0; x < _gridWidth; x++)
            {
                for (var y = 0; y < _gridHeight; y++)
                {
                    CreateBlock(new Vector2Int(x, y));
                }
            }
        }

        private void CreateBlock(Vector2Int position)
        {
            _matrixService.CreateBlock(position);

            var blockView = _blockPool.GetObject();

            blockView.OnDisappear
                .Subscribe(_ => DeleteBlock(position))
                .AddTo(_disposables);

            blockView.transform.SetParent(_blocksParent, false);
            
            var x = _startOffset.x + position.x * _stepX;
            var y = _startOffset.y - position.y * _stepY;

            blockView.transform.localPosition = new Vector3(x, y, 0f);

            _blocks[position] = blockView;
            _aliveBlocks++;
        }

        private void DeleteBlock(Vector2Int position)
        {
            _matrixService.DeleteBlock(position);

            if (_blocks.Remove(position, out var blockView))
            {
                _blockPool.ReturnObject(blockView);
            }

            _aliveBlocks = Mathf.Max(0, _aliveBlocks - 1);

            if (_aliveBlocks == 0)
            {
                _onAllBlocksDestroyed.OnNext(Unit.Default);
            }
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
            _onAllBlocksDestroyed.Dispose();
        }
    }
}
