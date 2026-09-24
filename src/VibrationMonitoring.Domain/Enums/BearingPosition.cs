using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationMonitoring.Domain.Enums;

/// <summary>
/// Позиция подшипника на компоненте.
/// </summary>
public enum BearingPosition
{
    Front = 0,
    Rear = 1,

    /// <summary>Любая другая позиция (для многоопорных валов).</summary>
    Other = 2
}
