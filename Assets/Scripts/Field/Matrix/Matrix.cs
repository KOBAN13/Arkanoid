using System;
using UnityEngine;

namespace Field.Matrix
{
    public readonly partial struct Matrix<T> : IMatrix<T>
    {
        private readonly T[] _array;

        public Vector2Int Size { get; }
        public int Length { get; }
        public int Width => Size.x;
        
        public int Height => Size.y;

        public T this[int x, int y]
        {
            get => _array[x + y * Size.x];
            set => _array[x + y * Size.x] = value;
        }

        public T this[Vector2Int pos]
        {
            get => _array[pos.x + pos.y * Size.x];
            set => _array[pos.x + pos.y * Size.x] = value;
        }
        
        private Matrix(T[] array, Vector2Int size)
        {
            var length = size.x * size.y;

            if (length > array.Length)
                throw new ArgumentOutOfRangeException($"[{nameof(Matrix<T>)}] Length of array is less than matrix size.");
            
            Size = size;
            
            Length = length;
            _array = array;
        }
        

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_array, Width, Length);
        }
    }
}