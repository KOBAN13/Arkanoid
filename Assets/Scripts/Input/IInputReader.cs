using R3;
using UnityEngine;

namespace Input
{
    public interface IInputReader
    {
        Observable<Vector2> Move { get; }
    }
}