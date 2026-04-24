using CamberGauge.Models;

namespace CamberGauge.Services.Mock;

public sealed class MockAngleSensorService : IAngleSensorService
{
    private IDispatcherTimer? _timer;

    public event EventHandler<SensorAngles>? ReadingChanged;

    public bool IsRunning { get; private set; }

    public double MockCamberDegrees { get; set; } = -2.0;

    public double MockLevelDegrees { get; set; } = 0.0;

    public void Start()
    {
        if (IsRunning)
            return;

        IsRunning = true;

        _timer = Application.Current!.Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(100);
        _timer.Tick += OnTimerTick;
        _timer.Start();
    }

    public void Stop()
    {
        if (!IsRunning)
            return;

        IsRunning = false;

        if (_timer is null)
            return;

        _timer.Stop();
        _timer.Tick -= OnTimerTick;
        _timer = null;
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        var camberWobble = Random.Shared.NextDouble() * 0.08 - 0.04;
        var levelWobble = Random.Shared.NextDouble() * 0.08 - 0.04;

        ReadingChanged?.Invoke(
            this,
            new SensorAngles(
                MockCamberDegrees + camberWobble,
                MockLevelDegrees + levelWobble));
    }
}