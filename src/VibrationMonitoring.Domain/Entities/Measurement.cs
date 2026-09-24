namespace VibrationMonitoring.Domain.Entities;

/// <summary>
/// Единичное измерение параметров на точке измерения.
/// Значения опциональны — заполняются только те, что реально сняты.
/// </summary>
public class Measurement : BaseEntity
{
    /// <summary>FK на узел дерева (обычно точка измерения).</summary>
    public Guid NodeId { get; set; }

    /// <summary>Навигационное свойство на узел.</summary>
    public Node Node { get; set; } = null!;

    /// <summary>Момент измерения (UTC).</summary>
    public DateTime MeasuredAt { get; set; }

    /// <summary>Общая вибрация (СКЗ), мм/с.</summary>
    public double? Vibration { get; set; }

    /// <summary>Температура, °C.</summary>
    public double? Temperature { get; set; }

    /// <summary>Ускорение, м/с².</summary>
    public double? Acceleration { get; set; }

    /// <summary>Куртозис (пиковый фактор).</summary>
    public double? Kurtosis { get; set; }

    /// <summary>Единица измерения вибрации, если отличается от мм/с (например, "µm").</summary>
    public string? Unit { get; set; }

    /// <summary>Признак валидности измерения. <c>false</c>, если оператор забраковал запись.</summary>
    public bool IsValid { get; set; } = true;

    /// <summary>Причина браковки (если <see cref="IsValid"/> = <c>false</c>).</summary>
    public string? InvalidReason { get; set; }

    /// <summary>ФИО оператора, выполнившего измерение.</summary>
    public string MeasuredBy { get; set; } = null!;

    /// <summary>Произвольный комментарий.</summary>
    public string? Comment { get; set; }

    /// <summary>FK на элемент выполнения маршрута, если измерение сделано в рамках обхода.</summary>
    public Guid? ExecutionItemId { get; set; }

    /// <summary>Навигационное свойство на элемент выполнения маршрута.</summary>
    public ExecutionItem? ExecutionItem { get; set; }
}