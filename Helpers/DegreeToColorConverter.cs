using CamberGauge.Settings;
using System.Globalization;

namespace CamberGauge.Utilities;

public class DegreeToColorConverter : IValueConverter
{
    public double Tolerance { get; set; } = 0.35;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not double degrees)
            return Colors.DarkRed;

        return Math.Abs(degrees) <= SensorSettings.LevelToleranceDegrees
            ? Application.Current!.Resources["Success"] as Color ?? Color.FromRgb(48, 209, 88)
            : Application.Current!.Resources["Danger"] as Color ?? Color.FromRgb(255, 69, 58);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}