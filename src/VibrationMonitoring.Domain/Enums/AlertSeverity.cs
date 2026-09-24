using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationMonitoring.Domain.Enums;

/// <summary>
/// Уровень критичности нарушения/оповещения.
/// Порядок важен: сравнение идёт через операторы &gt; и &lt;.
/// </summary>
public enum AlertSeverity
{
    /// <summary>Информационное сообщение, не требует действий.</summary>
    Info = 0,

    /// <summary>Предупреждение — значения приближаются к критическим.</summary>
    Warning = 1,

    /// <summary>Критическое — требуется немедленное вмешательство.</summary>
    Critical = 2,

    /// <summary>Аварийное — остановка оборудования.</summary>
    Emergency = 3
}
