using VibrationMonitoring.Domain.Enums;

namespace VibrationMonitoring.Domain.Entities;

/// <summary>
/// Элемент маршрута — ссылка на узел дерева (компонент, подшипник или точка измерения).
/// </summary>
public class DiagnosticRouteItem : BaseEntity
{
    /// <summary>Порядковый номер в маршруте.</summary>
    public int Order { get; set; }

    /// <summary>Заметки оператору.</summary>
    public string? Notes { get; set; }

    /// <summary>Ожидаемая длительность, секунды.</summary>
    public int? ExpectedDurationSeconds { get; set; }

    /// <summary>FK на маршрут.</summary>
    public Guid RouteId { get; set; }

    /// <summary>Навигационное свойство на маршрут.</summary>
    public DiagnosticRoute Route { get; set; } = null!;

    /// <summary>FK на узел дерева.</summary>
    public Guid NodeId { get; set; }

    /// <summary>Навигационное свойство на узел.</summary>
    public Node Node { get; set; } = null!;

    /// <summary>Текущий статус прохождения.</summary>
    public PointStatus Status { get; set; } = PointStatus.Pending;
}
