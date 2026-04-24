namespace CamberGauge.Utilities;

public static class AngleMath
{
    // Camber axis (resettable)
    public static double GetCamberDegrees(double x, double y, double z)
    {
        return ToDegrees(Math.Atan2(x, Math.Sqrt(y * y + z * z)));
    }

    // Level/orientation axis (NOT resettable)
    public static double GetLevelDegrees(double x, double y, double z)
    {
        return ToDegrees(Math.Atan2(y, Math.Sqrt(x * x + z * z)));
    }

    public static double Smooth(double previous, double current, double alpha)
    {
        return previous + alpha * (current - previous);
    }

    private static double ToDegrees(double radians)
    {
        return radians * 180.0 / Math.PI;
    }
}