namespace CamberGauge.Settings;

public static class SensorSettings
{
    public const double LevelToleranceDegrees = 0.03;
    public const double BeepCooldownMs = 500;
    // Lower = smoother/slower, higher = faster/more jittery
    public const double SmoothingAlpha = 0.15;
}