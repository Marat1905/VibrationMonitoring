using VibrationMonitoring.Domain.Enums;

namespace VibrationMonitoring.Domain.Entities;

/// <summary>
/// Запись в истории изменений оповещения.
/// </summary>
public class AlertStatusHistory : BaseEntity
{
    /// <summary>FK на оповещение.</summary>
    public Guid AlertId { get; set; }

    /// <summary>Навигационное свойство на оповещение.</summary>
    public Alert Alert { get; set; } = null!;

    /// <summary>Действие.</summary>
    public AlertAction Action { get; set; }

    /// <summary>Пользователь, выполнивший действие.</summary>
    public string User { get; set; } = null!;

    /// <summary>Комментарий.</summary>
    public string Comment { get; set; } = null!;

    /// <summary>Момент события (UTC).</summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
