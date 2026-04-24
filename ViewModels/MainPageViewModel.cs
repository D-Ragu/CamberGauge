using CamberGauge.Models;
using CamberGauge.Services;
using CamberGauge.Services.Mock;
using CamberGauge.Settings;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CamberGauge.ViewModels;

public sealed class MainPageViewModel : INotifyPropertyChanged
{
    private readonly IAngleSensorService _angleSensorService;
    private readonly MockAngleSensorService? _mockAngleSensorService;

    private double _camberZeroOffset;
    private double _lastCamberRaw;
    private DateTime _lastBeep = DateTime.MinValue;

    private bool _hasSmoothedValue;
    private double _smoothedCamber;
    private double _smoothedLevel;
    private bool _wasLevel;

    private string _camberDisplay = "0.00°";
    private string _levelDisplay = "Level Axis: 0.00°";
    private double _levelRaw;

    private double _mockCamberDegrees = -2.0;
    private double _mockLevelDegrees;

    public MainPageViewModel(IAngleSensorService angleSensorService)
    {
        _angleSensorService = angleSensorService;
        _mockAngleSensorService = angleSensorService as MockAngleSensorService;

        _angleSensorService.ReadingChanged += OnSensorReadingChanged;

        StartCommand = new Command(Start);
        StopCommand = new Command(Stop);
        ZeroCommand = new Command(Zero);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ICommand StartCommand { get; }

    public ICommand StopCommand { get; }

    public ICommand ZeroCommand { get; }

    public string SensorModeDisplay =>
        AppFeatureFlags.UseMockSensorData
            ? "Sensor Mode: Mock"
            : "Sensor Mode: Real";

    public bool IsMockMode => _mockAngleSensorService is not null;

    public string CamberDisplay
    {
        get => _camberDisplay;
        private set => SetProperty(ref _camberDisplay, value);
    }

    public double LevelRaw
    {
        get => _levelRaw;
        private set => SetProperty(ref _levelRaw, value);
    }

    public string LevelDisplay
    {
        get => _levelDisplay;
        private set => SetProperty(ref _levelDisplay, value);
    }

    public double MockCamberDegrees
    {
        get => _mockCamberDegrees;
        set
        {
            if (SetProperty(ref _mockCamberDegrees, value))
            {
                if (_mockAngleSensorService is not null)
                    _mockAngleSensorService.MockCamberDegrees = value;

                OnPropertyChanged(nameof(MockCamberDisplay));

                UpdateGauge(value, MockLevelDegrees);
            }
        }
    }

    public double MockLevelDegrees
    {
        get => _mockLevelDegrees;
        set
        {
            if (SetProperty(ref _mockLevelDegrees, value))
            {
                if (_mockAngleSensorService is not null)
                    _mockAngleSensorService.MockLevelDegrees = value;

                OnPropertyChanged(nameof(MockLevelDisplay));

                UpdateGauge(MockCamberDegrees, value);
            }
        }
    }

    public string MockCamberDisplay =>
        $"Mock Camber: {MockCamberDegrees:+0.00;-0.00;0.00}°";

    public string MockLevelDisplay =>
        $"Mock Level: {MockLevelDegrees:+0.00;-0.00;0.00}°";

    private void Start()
    {
        _angleSensorService.Start();
    }

    private void Stop()
    {
        _angleSensorService.Stop();
    }

    private void Zero()
    {
        _angleSensorService.Stop();
        _camberZeroOffset = CalculateZeroOffset(_lastCamberRaw);
        UpdateGauge(_lastCamberRaw, LevelRaw);
    }

    internal static double CalculateZeroOffset(double lastRaw)
    {
        return lastRaw;
    }

    private void OnSensorReadingChanged(object? sender, SensorAngles angles)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            UpdateGauge(angles.CamberDegrees, angles.LevelDegrees);
        });
    }

    private void UpdateGauge(double camberRaw, double levelRaw)
    {
        if (!_hasSmoothedValue)
        {
            _smoothedCamber = camberRaw;
            _smoothedLevel = levelRaw;
            _hasSmoothedValue = true;
        }
        else
        {
            _smoothedCamber = Smooth(_smoothedCamber, camberRaw);
            _smoothedLevel = Smooth(_smoothedLevel, levelRaw);
        }

        _lastCamberRaw = _smoothedCamber;

        var camberAdjusted = _smoothedCamber - _camberZeroOffset;
        var isLevel = Math.Abs(_smoothedLevel) <= SensorSettings.LevelToleranceDegrees;

        LevelRaw = _smoothedLevel;

        CamberDisplay = $"{camberAdjusted:+0.00;-0.00;0.00}°";
        LevelDisplay = $"Level Axis: {_smoothedLevel:+0.00;-0.00;0.00}°";

        if (isLevel && !_wasLevel)
        {
            Beep();
        }

        _wasLevel = isLevel;
    }

    private void Beep()
    {
        if ((DateTime.Now - _lastBeep).TotalMilliseconds < SensorSettings.BeepCooldownMs)
            return;

        _lastBeep = DateTime.Now;

        try
        {
#if IOS
            UIKit.UIImpactFeedbackGenerator feedback =
                new UIKit.UIImpactFeedbackGenerator(UIKit.UIImpactFeedbackStyle.Light);

            feedback.Prepare();
            feedback.ImpactOccurred();
#else
            if (!AppFeatureFlags.UseMockSensorData)
            {
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(80));
            }
#endif
        }
        catch
        {
            // Some emulators/devices do not support haptic feedback.
        }
    }

    private static double Smooth(double previous, double current)
    {
        return previous + SensorSettings.SmoothingAlpha * (current - previous);
    }

    private bool SetProperty<T>(
        ref T backingField,
        T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingField, value))
            return false;

        backingField = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}