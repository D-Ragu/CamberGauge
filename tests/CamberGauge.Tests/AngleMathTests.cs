using CamberGauge.Helpers;

namespace CamberGauge.Tests;

public class AngleMathTests
{
    [Fact]
    public void Smooth_MovesPreviousTowardCurrent()
    {
        var result = AngleMath.Smooth(0, 10, 0.15);

        Assert.Equal(1.5, result, precision: 3);
    }

    [Fact]
    public void Smooth_WithAlphaOne_ReturnsCurrent()
    {
        var result = AngleMath.Smooth(2, 10, 1);

        Assert.Equal(10, result, precision: 3);
    }

    [Fact]
    public void Smooth_WithAlphaZero_ReturnsPrevious()
    {
        var result = AngleMath.Smooth(2, 10, 0);

        Assert.Equal(2, result, precision: 3);
    }
}