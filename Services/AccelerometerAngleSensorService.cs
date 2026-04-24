using CamberGauge.Models;
using CamberGauge.Utilities;
using Microsoft.Maui.Devices.Sensors;

namespace CamberGauge.Services.Real;

public sealed class AccelerometerAngleSensorService : IAngleSensorService
{
    public event EventHandler<SensorAngles>? ReadingChanged;

    public bool IsRunning => Accelerometer.Default.IsMonitoring;

    public void Start()
    {
        if (!Accelerometer.Default.IsSupported || Accelerometer.Default.IsMonitoring)
            return;

        Accelerometer.Default.ReadingChanged += OnReadingChanged;
        Accelerometer.Default.Start(SensorSpeed.UI);
    }

    public void Stop()
    {
        if (!Accelerometer.Default.IsMonitoring)
            return;

        Accelerometer.Default.ReadingChanged -= OnReadingChanged;
        Accelerometer.Default.Stop();
    }

    private void OnReadingChanged(object? sender, AccelerometerChangedEventArgs e)
    {
        var x = e.Reading.Acceleration.X;
        var y = e.Reading.Acceleration.Y;
        var z = e.Reading.Acceleration.Z;

        var camber = AngleMath.GetCamberDegrees(x, y, z);
        var level = AngleMath.GetLevelDegrees(x, y, z);

        ReadingChanged?.Invoke(this, new SensorAngles(camber, level));
    }
}