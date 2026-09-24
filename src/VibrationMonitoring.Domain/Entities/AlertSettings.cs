using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationMonitoring.Domain.Entities;

/// <summary>
/// Пороговые значения и правила мониторинга для узла.
/// Может быть привязано как к компоненту, так и к точке измерения.
/// Если поле = <c>null</c>, значение наследуется от родительского узла.
/// </summary>
public class AlertSettings : BaseEntity
{
    /// <summary>FK на узел дерева.</summary>
    public Guid NodeId { get; set; }

    /// <summary>Навигационное свойство на узел.</summary>
    public Node Node { get; set; } = null!;

    /// <summary>
    /// FK на родительский узел, от которого наследуются настройки.
    /// <c>null</c> — узел использует собственные (или дефолтные) значения.
    /// </summary>
    public Guid? InheritedFromNodeId { get; set; }

    // --- Пороги вибрации (мм/с) ---
    public double? VibrationWarning { get; set; }
    public double? VibrationCritical { get; set; }

    // --- Пороги температуры (°C) ---
    public double? TemperatureWarning { get; set; }
    public double? TemperatureCritical { get; set; }

    // --- Пороги ускорения (м/с²) ---
    public double? AccelerationWarning { get; set; }
    public double? AccelerationCritical { get; set; }

    // --- Пороги куртозиса ---
    public double? KurtosisWarning { get; set; }
    public double? KurtosisCritical { get; set; }

    /// <summary>Порог тренда в процентах (изменение относительно предыдущего измерения).</summary>
    public double TrendThreshold { get; set; } = 10.0;

    /// <summary>Гистерезис в процентах: значение должно вернуться ниже порога с запасом, чтобы закрыть нарушение.</summary>
    public double HysteresisPercent { get; set; } = 5.0;

    /// <summary>Интервал проверки отсутствия данных, дни.</summary>
    public int CheckIntervalDays { get; set; } = 7;

    // --- Включение проверок ---
    public bool IsVibrationMonitoringEnabled { get; set; } = true;
    public bool IsTemperatureMonitoringEnabled { get; set; } = true;
    public bool IsAccelerationMonitoringEnabled { get; set; }
    public bool IsKurtosisMonitoringEnabled { get; set; }
    public bool IsNoDataMonitoringEnabled { get; set; } = true;
    public bool IsTrendMonitoringEnabled { get; set; } = true;

    /// <summary>Дата, с которой действуют настройки.</summary>
    public DateTime EffectiveFrom { get; set; } = DateTime.UtcNow;

    /// <summary>Дата, по которую действуют настройки. <c>null</c> — бессрочно.</summary>
    public DateTime? EffectiveTo { get; set; }

    /// <summary>Пользователь, изменивший настройки.</summary>
    public string? ChangedBy { get; set; }

    /// <summary>Причина изменения настроек.</summary>
    public string? ChangeReason { get; set; }
}
