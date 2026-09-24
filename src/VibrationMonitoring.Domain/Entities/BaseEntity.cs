using System.ComponentModel.DataAnnotations;

namespace VibrationMonitoring.Domain.Entities;

/// <summary>
/// Базовая сущность с общими полями для всех сущностей домена.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>Первичный ключ. Генерируется автоматически при создании.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Дата/время создания записи (UTC).</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Дата/время последнего изменения (UTC). Проставляется перехватчиком SaveChanges.</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Флаг soft-delete. Записи с <c>true</c> не возвращаются в обычных запросах.</summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Токен оптимистичной блокировки. Используется EF Core для предотвращения
    /// потери изменений при конкурентном доступе.
    /// </summary>
    [Timestamp]
    public byte[]? RowVersion { get; set; }
}