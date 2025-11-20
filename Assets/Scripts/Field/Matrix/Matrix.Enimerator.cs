using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Field.Matrix
{
    public partial struct Matrix<T>
    {
        public struct Enumerator : IEnumerator<Vector2Int>
        {
            private readonly T[] _array;
            
            private int _index;
            private readonly int _width;
            private readonly int _length;
            
            public Vector2Int Current => new(_index % _width, _index / _width);
            object IEnumerator.Current => Current;

            internal Enumerator(T[] array, int width, int length)
            {
                _array = array;
                _index = -1;
                _width = width;
                _length = length;
            }

            public bool MoveNext()
            {
                var newIndex = _index + 1;

                if (newIndex >= _length) 
                    return false;

                _index = newIndex;
                return true;
            }

            public void Reset() => _index = -1;

            public void Dispose()
            {
                _index = -1;
            }
        }
    }
}