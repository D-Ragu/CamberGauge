using CamberGauge.Models;

namespace CamberGauge.Tests;

public class SensorAnglesTests
{
    [Fact]
    public void Constructor_StoresCamberAndLevelDegrees()
    {
        var angles = new SensorAngles(-2.5, 0.25);

        Assert.Equal(-2.5, angles.CamberDegrees);
        Assert.Equal(0.25, angles.LevelDegrees);
    }
}