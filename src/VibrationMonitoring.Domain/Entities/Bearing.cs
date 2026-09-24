using VibrationMonitoring.Domain.Enums;

namespace VibrationMonitoring.Domain.Entities;

/// <summary>
/// Установленный подшипник на конкретном компоненте.
/// Отличать от <see cref="BearingModel"/> — справочника моделей (6205-2RS и т.п.).
/// </summary>
public class Bearing : BaseEntity
{
    /// <summary>FK на узел дерева (тип <see cref="NodeType.Bearing"/>).</summary>
    public Guid NodeId { get; set; }

    /// <summary>Навигационное свойство на узел.</summary>
    public Node Node { get; set; } = null!;

    /// <summary>FK на компонент, к которому относится подшипник.</summary>
    public Guid ComponentId { get; set; }

    /// <summary>Навигационное свойство на компонент.</summary>
    public Component Component { get; set; } = null!;

    /// <summary>Позиция подшипника (передний/задний/прочее).</summary>
    public BearingPosition Position { get; set; }

    /// <summary>FK на модель подшипника из локального справочника. Может быть <c>null</c>, если модель ещё не синхронизирована.</summary>
    public Guid? BearingModelId { get; set; }

    /// <summary>Навигационное свойство на модель подшипника.</summary>
    public BearingModel? BearingModel { get; set; }

    /// <summary>Дата установки подшипника.</summary>
    public DateTime? InstalledAt { get; set; }

    /// <summary>Дата снятия подшипника (при замене). <c>null</c> — подшипник действующий.</summary>
    public DateTime? RemovedAt { get; set; }

    /// <summary>Точки измерения на этом подшипнике (до 3 — по одной на направление).</summary>
    public ICollection<MeasurementPoint> Points { get; set; } = new List<MeasurementPoint>();
}