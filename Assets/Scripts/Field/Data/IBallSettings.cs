using UnityEngine;

namespace Field.Data
{
    public interface IBallSettings
    {
        float StartSpeed { get; }
        float MinimumVerticalDot { get; }
        Vector3 StartDirection { get; }
        float MaximumVerticalDot { get; }
        float MinimumHorizontalDot { get; }
        float Skin { get; }
    }
}