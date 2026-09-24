using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationMonitoring.Domain.Enums;

/// <summary>
/// Направление измерения вибрации на подшипнике.
/// На одном подшипнике допускается не более одной точки каждого направления.
/// </summary>
public enum MeasurementDirection
{
    Vertical = 0,
    Horizontal = 1,
    Axial = 2
}
