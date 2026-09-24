using VibrationMonitoring.Domain.Enums;

namespace VibrationMonitoring.Domain.Entities;

/// <summary>
/// Специфичные данные компонента (узла типа <see cref="NodeType.Component"/>).
/// </summary>
public class Component : BaseEntity
{
    /// <summary>FK на узел дерева.</summary>
    public Guid NodeId { get; set; }

    /// <summary>Навигационное свойство на узел.</summary>
    public Node Node { get; set; } = null!;

    /// <summary>Уточнённый тип компонента (ротор, двигатель, насос и т.д.).</summary>
    public ComponentKind Kind { get; set; } = ComponentKind.Unknown;

    /// <summary>Код двигателя во внешней системе (AssetTracker.Motor.Code).</summary>
    public string? ExternalAssetCode { get; set; }

    /// <summary>
    /// Локальный кэш-снимок данных двигателя из AssetTracker.
    /// Заполняется при первичной синхронизации и обновляется по событиям.
    /// </summary>
    public MotorSnapshot? MotorSnapshot { get; set; }

    /// <summary>Подшипники, привязанные к этому компоненту.</summary>
    public ICollection<Bearing> Bearings { get; set; } = new List<Bearing>();
}