namespace CamberGauge.Models;

public sealed class SensorAngles
{
    public SensorAngles(double camberDegrees, double levelDegrees)
    {
        CamberDegrees = camberDegrees;
        LevelDegrees = levelDegrees;
    }

    public double CamberDegrees { get; }

    public double LevelDegrees { get; }
}