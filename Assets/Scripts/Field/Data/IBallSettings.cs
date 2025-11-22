using UnityEngine;

namespace Field.Data
{
    public interface IBallSettings
    {
        float StartSpeed { get; }
        float MinimumVerticalDot { get; }
        Vector3 StartDirection { get; }
    }
}