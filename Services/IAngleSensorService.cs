using System;
using CamberGauge.Models;

namespace CamberGauge.Services
{
    // Made public so it is at least as accessible as MainPageViewModel
    public interface IAngleSensorService
    {
        event EventHandler<SensorAngles> ReadingChanged;
        void Start();
        void Stop();
    }
}
