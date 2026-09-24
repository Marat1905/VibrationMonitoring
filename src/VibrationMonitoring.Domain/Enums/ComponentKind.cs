using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationMonitoring.Domain.Enums;

/// <summary>
/// Уточнение типа компонента. Помогает при интеграции и подборе диагностических правил.
/// </summary>
public enum ComponentKind
{
    Unknown = 0,
    Rotor = 1,
    Motor = 2,
    Pump = 3,
    Fan = 4,
    Gearbox = 5,
    Compressor = 6
}
