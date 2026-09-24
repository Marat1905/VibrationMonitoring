using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationMonitoring.Domain.Enums;

/// <summary>
/// Жизненный цикл оповещения.
/// </summary>
public enum AlertStatus
{
    /// <summary>Активно, никто не взял в работу.</summary>
    Active = 0,

    /// <summary>Взято в работу (подтверждено оператором).</summary>
    Acknowledged = 1,

    /// <summary>Разрешено (закрыто).</summary>
    Resolved = 2
}
