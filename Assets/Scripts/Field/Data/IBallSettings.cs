namespace Field.Data
{
    public interface IBallSettings
    {
        float StartSpeed { get; }
        int DeviationAngle { get; }
        float PerpendicularThreshold { get; }
    }
}