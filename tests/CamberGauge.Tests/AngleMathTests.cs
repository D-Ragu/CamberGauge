using CamberGauge.Helpers;

namespace CamberGauge.Tests;

public class AngleMathTests
{
    /// <summary>
    /// Smooth should move the previous angle towards the current angle by a factor of alpha. In this case, with a previous angle of 0, a current angle of 10, and an alpha of 0.15, the result should be 1.5 (0 + (10 - 0) * 0.15).
    /// </summary>
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