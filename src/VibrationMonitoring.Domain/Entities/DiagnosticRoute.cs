using VibrationMonitoring.Domain.Enums;

namespace VibrationMonitoring.Domain.Entities;

/// <summary>
/// Маршрут диагностики — упорядоченный список узлов для обхода оператором.
/// </summary>
public class DiagnosticRoute : BaseEntity
{
    /// <summary>Название маршрута.</summary>
    public string Name { get; set; } = null!;

    /// <summary>Описание.</summary>
    public string? Description { get; set; }

    /// <summary>Ожидаемая суммарная длительность, секунды.</summary>
    public int EstimatedTotalDurationSeconds { get; set; }

    /// <summary>Статус маршрута.</summary>
    public RouteStatus Status { get; set; } = RouteStatus.Draft;

    /// <summary>Кто создал.</summary>
    public string CreatedBy { get; set; } = null!;

    /// <summary>Кто последний редактировал.</summary>
    public string? UpdatedBy { get; set; }

    /// <summary>Элементы маршрута.</summary>
    public ICollection<DiagnosticRouteItem> Items { get; set; } = new List<DiagnosticRouteItem>();

    /// <summary>История выполнений маршрута.</summary>
    public ICollection<RouteExecution> Executions { get; set; } = new List<RouteExecution>();
}
