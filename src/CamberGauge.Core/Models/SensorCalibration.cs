namespace CamberGauge.Models;

public sealed class SensorCalibration
{
    public SensorAxis CamberAxis { get; set; } = SensorAxis.X;
    public SensorAxis LevelAxis { get; set; } = SensorAxis.Y;

    public bool InvertCamber { get; set; }
    public bool InvertLevel { get; set; }
}