namespace Field.Data
{
    public interface IBallSettings
    {
        float StartSpeed { get; }
        float DeviationAngle { get; }
        float HorizontalLockThreshold { get; }
        float VerticalLockThreshold { get; }
    }
}