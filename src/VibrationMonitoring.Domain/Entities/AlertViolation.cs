using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibrationMonitoring.Domain.Enums;

namespace VibrationMonitoring.Domain.Entities;

/// <summary>
/// Конкретное нарушение в рамках оповещения.
/// Привязано к точке измерения и типу нарушения.
/// </summary>
public class AlertViolation : BaseEntity
{
    /// <summary>FK на оповещение.</summary>
    public Guid AlertId { get; set; }

    /// <summary>Навигационное свойство на оповещение.</summary>
    public Alert Alert { get; set; } = null!;

    /// <summary>FK на узел-точку измерения.</summary>
    public Guid MeasurementPointNodeId { get; set; }

    /// <summary>Навигационное свойство на точку измерения.</summary>
    public Node MeasurementPointNode { get; set; } = null!;

    /// <summary>FK на измерение, вызвавшее нарушение. Может быть <c>null</c> для трендовых/пропущенных.</summary>
    public Guid? MeasurementId { get; set; }

    /// <summary>Навигационное свойство на измерение.</summary>
    public Measurement? Measurement { get; set; }

    // --- Параметры нарушения ---

    /// <summary>Тип нарушения.</summary>
    public AlertType ViolationType { get; set; }

    /// <summary>Критичность нарушения.</summary>
    public AlertSeverity ViolationSeverity { get; set; }

    /// <summary>Текущее значение параметра.</summary>
    public double CurrentValue { get; set; }

    /// <summary>Предыдущее значение (при эскалации).</summary>
    public double? PreviousValue { get; set; }

    /// <summary>Пороговое значение, которое было превышено.</summary>
    public double? ThresholdValue { get; set; }

    /// <summary>Процент изменения относительно предыдущего значения.</summary>
    public double? ChangePercentage { get; set; }

    // --- Статус ---

    /// <summary>Когда нарушение было впервые обнаружено.</summary>
    public DateTime FirstDetectedAt { get; set; }

    /// <summary>Когда нарушение было зафиксировано в последний раз.</summary>
    public DateTime DetectedAt { get; set; }

    /// <summary>Признак разрешения.</summary>
    public bool IsResolved { get; private set; }

    /// <summary>Дата разрешения.</summary>
    public DateTime? ResolvedAt { get; private set; }

    /// <summary>Причина разрешения.</summary>
    public string? ResolvedReason { get; private set; }

    /// <summary>Кто разрешил.</summary>
    public string? ResolvedBy { get; private set; }

    /// <summary>Тип разрешения.</summary>
    public ViolationResolutionType? ResolutionType { get; private set; }

    // --- Эскалация ---

    /// <summary>Была ли эскалация.</summary>
    public bool IsEscalated { get; private set; }

    /// <summary>Количество эскалаций.</summary>
    public int EscalationCount { get; private set; }

    /// <summary>История изменений нарушения.</summary>
    public ICollection<ViolationHistory> History { get; private set; } = new List<ViolationHistory>();

    /// <summary>Разрешает нарушение.</summary>
    public void Resolve(string resolvedBy, string reason, ViolationResolutionType? resolutionType = null)
    {
        if (IsResolved) return;

        IsResolved = true;
        ResolvedAt = DateTime.UtcNow;
        ResolvedReason = reason;
        ResolvedBy = resolvedBy;
        ResolutionType = resolutionType;

        AddHistory(ViolationAction.Resolved, resolvedBy, reason);
    }

    /// <summary>Переоткрывает нарушение.</summary>
    public void Reopen()
    {
        if (!IsResolved) return;

        IsResolved = false;
        ResolvedAt = null;
        ResolvedReason = null;
        ResolvedBy = null;
        ResolutionType = null;

        AddHistory(ViolationAction.Reopened, "System", "Нарушение переоткрыто");
    }

    /// <summary>
    /// Эскалирует нарушение при повышении критичности или ухудшении значения.
    /// Возвращает <c>true</c>, если были изменения.
    /// </summary>
    public bool Escalate(AlertSeverity newSeverity, double newValue, double? newThreshold = null)
    {
        bool changed = false;

        if (newSeverity > ViolationSeverity)
        {
            ViolationSeverity = newSeverity;
            changed = true;
        }

        if (newValue > CurrentValue)
        {
            PreviousValue = CurrentValue;
            CurrentValue = newValue;
            DetectedAt = DateTime.UtcNow;
            IsEscalated = true;
            EscalationCount++;

            if (newThreshold.HasValue)
                ThresholdValue = newThreshold.Value;

            AddHistory(ViolationAction.Escalated, "System",
                $"Эскалация: {ViolationType} {PreviousValue:F2} → {newValue:F2}");
            changed = true;
        }

        return changed;
    }

    /// <summary>Добавляет запись в историю нарушений.</summary>
    private void AddHistory(ViolationAction action, string user, string comment)
    {
        History.Add(new ViolationHistory
        {
            ViolationId = Id,
            Action = action,
            User = user,
            Comment = comment,
            Timestamp = DateTime.UtcNow
        });
    }
}
