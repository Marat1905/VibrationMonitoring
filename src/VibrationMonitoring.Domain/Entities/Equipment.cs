namespace VibrationMonitoring.Domain.Entities;

/// <summary>
/// Специфичные данные оборудования (узла типа <see cref="Enums.NodeType.Equipment"/>).
/// </summary>
public class Equipment : BaseEntity
{
    /// <summary>FK на узел дерева.</summary>
    public Guid NodeId { get; set; }

    /// <summary>Навигационное свойство на узел.</summary>
    public Node Node { get; set; } = null!;

    /// <summary>Производитель.</summary>
    public string? Manufacturer { get; set; }

    /// <summary>Модель.</summary>
    public string? Model { get; set; }

    /// <summary>Заводской/серийный номер.</summary>
    public string? SerialNumber { get; set; }

    /// <summary>Дата ввода в эксплуатацию.</summary>
    public DateTime? CommissionedAt { get; set; }
}