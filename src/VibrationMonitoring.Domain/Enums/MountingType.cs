using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationMonitoring.Domain.Enums;

/// <summary>
/// Тип монтажа двигателя. Используется в диагностике (влияет на спектр).
/// В VibrationMonitoring хранится только как часть MotorSnapshot.
/// </summary>
public enum MountingType
{
    Unknown = 0,

    /// <summary>Лапы.</summary>
    Foot = 1,

    /// <summary>Фланец.</summary>
    Flange = 2,

    /// <summary>Лапы + фланец.</summary>
    FootFlange = 3,

    /// <summary>Фланцевое крепление.</summary>
    Face = 4
}
