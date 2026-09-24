using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationMonitoring.Domain.Enums;

/// <summary>
/// Тип нарушения, вызвавшего оповещение.
/// </summary>
public enum AlertType
{
    Vibration = 0,
    Temperature = 1,
    Acceleration = 2,
    Kurtosis = 3,
    VibrationTrend = 4,
    TemperatureTrend = 5,
    AccelerationTrend = 6,
    KurtosisTrend = 7,
    MissedMeasurement = 8
}
