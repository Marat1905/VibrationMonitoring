using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationMonitoring.Domain.Enums;

/// <summary>
/// Причина/способ разрешения нарушения. Нужна для последующей аналитики.
/// </summary>
public enum ViolationResolutionType
{
    /// <summary>Устранено обслуживанием (ремонт, замена).</summary>
    MaintenanceFixed = 0,

    /// <summary>Ложное срабатывание (ошибка датчика, ввод данных).</summary>
    FalsePositive = 1,

    /// <summary>Нормализовалось самостоятельно.</summary>
    SelfResolved = 2,

    /// <summary>Признано несущественным, понижено вручную.</summary>
    Dismissed = 3,

    /// <summary>Разрешено при закрытии родительского оповещения.</summary>
    ResolvedWithAlert = 4
}
