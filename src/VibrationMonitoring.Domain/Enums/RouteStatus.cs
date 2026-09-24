using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationMonitoring.Domain.Enums;

/// <summary>
/// Статус маршрута диагностики.
/// </summary>
public enum RouteStatus
{
    Draft = 0,
    Active = 1,
    Archived = 2
}
