using UnityEngine;

namespace Field.Matrix
{
    public interface IMatrix<T>
    {
        new T this[int x, int y] { get; set; }
        new T this[Vector2Int pos] { get; set; }
    }
}