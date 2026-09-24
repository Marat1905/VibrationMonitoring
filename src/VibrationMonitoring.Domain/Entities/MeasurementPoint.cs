using VibrationMonitoring.Domain.Enums;

namespace VibrationMonitoring.Domain.Entities;

/// <summary>
/// Точка измерения на подшипнике. На одном подшипнике допускается не более одной
/// точки на каждое направление (Vertical/Horizontal/Axial), т.е. максимум 3.
/// </summary>
public class MeasurementPoint : BaseEntity
{
    /// <summary>FK на узел дерева (тип <see cref="NodeType.Point"/>).</summary>
    public Guid NodeId { get; set; }

    /// <summary>Навигационное свойство на узел.</summary>
    public Node Node { get; set; } = null!;

    /// <summary>FK на подшипник.</summary>
    public Guid BearingId { get; set; }

    /// <summary>Навигационное свойство на подшипник.</summary>
    public Bearing Bearing { get; set; } = null!;

    /// <summary>Направление измерения.</summary>
    public MeasurementDirection Direction { get; set; }

    /// <summary>Код датчика/канала, если точка привязана к аппаратному каналу.</summary>
    public string? SensorCode { get; set; }

    /// <summary>Единица измерения (например, "mm/s", "µm"). Может отличаться для разных точек.</summary>
    public string? Unit { get; set; }
}