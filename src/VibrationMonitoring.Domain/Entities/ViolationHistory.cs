using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibrationMonitoring.Domain.Enums;

namespace VibrationMonitoring.Domain.Entities;

/// <summary>
/// Запись в истории изменений нарушения.
/// </summary>
public class ViolationHistory : BaseEntity
{
    /// <summary>FK на нарушение.</summary>
    public Guid ViolationId { get; set; }

    /// <summary>Навигационное свойство на нарушение.</summary>
    public AlertViolation Violation { get; set; } = null!;

    /// <summary>Действие.</summary>
    public ViolationAction Action { get; set; }

    /// <summary>Пользователь, выполнивший действие.</summary>
    public string User { get; set; } = null!;

    /// <summary>Комментарий.</summary>
    public string Comment { get; set; } = null!;

    /// <summary>Момент события (UTC).</summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
