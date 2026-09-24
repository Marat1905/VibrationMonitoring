using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationMonitoring.Domain.Enums;

/// <summary>
/// Действие, зафиксированное в истории оповещения.
/// </summary>
public enum AlertAction
{
    Created = 0,
    Acknowledged = 1,
    Resolved = 2,
    Reopened = 3,
    Escalated = 4,

    /// <summary>Изменён тип оповещения (например, с Temperature на Vibration).</summary>
    TypeChanged = 5,

    /// <summary>Оповещение автоматически закрыто (по таймауту или при отсутствии данных).</summary>
    AutoResolved = 6
}
