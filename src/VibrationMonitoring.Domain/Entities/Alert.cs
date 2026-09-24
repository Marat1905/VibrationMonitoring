using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VibrationMonitoring.Domain.Enums;

namespace VibrationMonitoring.Domain.Entities;

/// <summary>
/// Оповещение о нарушении. Агрегирует одно или несколько нарушений
/// по одному компоненту и управляет их жизненным циклом.
/// </summary>
public class Alert : BaseEntity
{
    /// <summary>FK на узел-компонент, к которому относится оповещение.</summary>
    public Guid ComponentNodeId { get; set; }

    /// <summary>Навигационное свойство на компонент.</summary>
    public Node ComponentNode { get; set; } = null!;

    // --- Основные свойства ---

    /// <summary>Текущий уровень критичности (максимум среди активных нарушений).</summary>
    public AlertSeverity Severity { get; private set; }

    /// <summary>Статус жизненного цикла.</summary>
    public AlertStatus Status { get; private set; } = AlertStatus.Active;

    /// <summary>Преобладающий тип нарушения (по приоритету и критичности).</summary>
    public AlertType Type { get; private set; }

    /// <summary>Общее количество нарушений за всю историю оповещения.</summary>
    public int TotalViolationCount { get; private set; }

    /// <summary>Количество активных (неразрешённых) нарушений.</summary>
    public int ActiveViolationCount { get; private set; }

    // --- Денормализованный снимок компонента (для истории и поиска) ---

    /// <summary>Имя компонента на момент создания оповещения.</summary>
    public required string ComponentName { get; set; }

    /// <summary>Путь компонента на момент создания оповещения.</summary>
    public required string ComponentPath { get; set; }

    /// <summary>Дата/время снятия снимка компонента.</summary>
    public DateTime SnapshotAt { get; set; } = DateTime.UtcNow;

    // --- Временные метки ---

    /// <summary>Когда нарушение было впервые зафиксировано.</summary>
    public DateTime FirstDetectedAt { get; set; }

    /// <summary>Последнее обновление.</summary>
    public DateTime LastUpdatedAt { get; set; }

    /// <summary>Когда оповещение было подтверждено.</summary>
    public DateTime? AcknowledgedAt { get; private set; }

    /// <summary>Когда оповещение было разрешено.</summary>
    public DateTime? ResolvedAt { get; private set; }

    // --- Пользователи ---

    /// <summary>Кем обнаружено (по умолчанию "System").</summary>
    public string DetectedBy { get; set; } = "System";

    /// <summary>Кто подтвердил.</summary>
    public string? AcknowledgedBy { get; private set; }

    /// <summary>Кто разрешил.</summary>
    public string? ResolvedBy { get; private set; }

    /// <summary>Комментарий при подтверждении.</summary>
    public string? AcknowledgedComment { get; private set; }

    /// <summary>Комментарий при разрешении.</summary>
    public string? ResolvedComment { get; private set; }

    // --- Эскалация ---

    /// <summary>Признак, что оповещение эскалировалось хотя бы раз.</summary>
    public bool IsEscalated { get; private set; }

    /// <summary>Суммарное количество эскалаций по всем активным нарушениям.</summary>
    public int EscalationCount { get; private set; }

    // --- Коллекции ---

    /// <summary>История изменений статуса и значимых событий.</summary>
    public ICollection<AlertStatusHistory> StatusHistory { get; private set; } = new List<AlertStatusHistory>();

    /// <summary>Нарушения, входящие в это оповещение.</summary>
    public ICollection<AlertViolation> Violations { get; private set; } = new List<AlertViolation>();

    // --- Вычисляемые свойства ---

    /// <summary>Все нарушения разрешены.</summary>
    public bool AllViolationsResolved => Violations.All(v => v.IsResolved);

    /// <summary>Есть хотя бы одно активное нарушение.</summary>
    public bool HasActiveViolations => Violations.Any(v => !v.IsResolved);

    /// <summary>Приоритет типов нарушений при выборе преобладающего типа.</summary>
    private static readonly Dictionary<AlertType, int> TypePriority = new()
    {
        [AlertType.Vibration] = 100,
        [AlertType.Temperature] = 90,
        [AlertType.Acceleration] = 80,
        [AlertType.Kurtosis] = 70,
        [AlertType.VibrationTrend] = 60,
        [AlertType.TemperatureTrend] = 50,
        [AlertType.AccelerationTrend] = 40,
        [AlertType.KurtosisTrend] = 30,
        [AlertType.MissedMeasurement] = 20
    };

    private Alert() { }

    /// <summary>Создаёт новое оповещение по компоненту.</summary>
    public Alert(Node componentNode)
    {
        ComponentNodeId = componentNode.Id;
        ComponentNode = componentNode;
        ComponentName = componentNode.Name;
        ComponentPath = componentNode.Path;
        FirstDetectedAt = DateTime.UtcNow;
        LastUpdatedAt = DateTime.UtcNow;
        Status = AlertStatus.Active;
    }

    /// <summary>
    /// Добавляет новое нарушение или эскалирует существующее.
    /// Возвращает <c>true</c>, если были фактические изменения.
    /// </summary>
    public bool AddOrUpdateViolation(AlertViolation newViolation)
    {
        var existing = Violations.FirstOrDefault(v =>
            v.MeasurementPointNodeId == newViolation.MeasurementPointNodeId &&
            v.ViolationType == newViolation.ViolationType &&
            !v.IsResolved);

        bool changed;
        if (existing != null)
        {
            changed = existing.Escalate(
                newViolation.ViolationSeverity,
                newViolation.CurrentValue,
                newViolation.ThresholdValue);
        }
        else
        {
            newViolation.AlertId = Id;
            newViolation.FirstDetectedAt = DateTime.UtcNow;
            newViolation.DetectedAt = DateTime.UtcNow;
            Violations.Add(newViolation);
            TotalViolationCount++;
            changed = true;
        }

        if (changed)
        {
            Recalculate();
            LastUpdatedAt = DateTime.UtcNow;
        }

        return changed;
    }

    /// <summary>Разрешает конкретное нарушение по Id.</summary>
    public void ResolveViolation(Guid violationId, string resolvedBy, string reason, ViolationResolutionType type)
    {
        var violation = Violations.FirstOrDefault(v => v.Id == violationId && !v.IsResolved);
        if (violation is null) return;

        violation.Resolve(resolvedBy, reason, type);
        Recalculate();
        LastUpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Автоматически разрешает все активные нарушения.</summary>
    public void AutoResolveViolations(ViolationResolutionType type, string reason = "Автоматическое разрешение")
    {
        foreach (var v in Violations.Where(v => !v.IsResolved))
            v.Resolve("System", reason, type);

        Recalculate();
        LastUpdatedAt = DateTime.UtcNow;
    }

    /// <summary>Подтверждает оповещение (взятие в работу).</summary>
    public void Acknowledge(string user, string comment)
    {
        if (Status != AlertStatus.Active)
            throw new InvalidOperationException("Подтвердить можно только активное оповещение.");

        Status = AlertStatus.Acknowledged;
        AcknowledgedBy = user;
        AcknowledgedAt = DateTime.UtcNow;
        AcknowledgedComment = comment;
        LastUpdatedAt = DateTime.UtcNow;

        AddHistory(AlertAction.Acknowledged, user, comment);
    }

    /// <summary>Разрешает оповещение и все его активные нарушения.</summary>
    public void Resolve(string user, string comment, ViolationResolutionType type)
    {
        if (Status == AlertStatus.Resolved)
            throw new InvalidOperationException("Оповещение уже разрешено.");

        Status = AlertStatus.Resolved;
        ResolvedBy = user;
        ResolvedAt = DateTime.UtcNow;
        ResolvedComment = comment;
        LastUpdatedAt = DateTime.UtcNow;

        foreach (var v in Violations.Where(v => !v.IsResolved))
            v.Resolve(user, $"Разрешено вместе с оповещением: {comment}", type);

        Recalculate();
        AddHistory(AlertAction.Resolved, user, $"{comment} ({type})");
    }

    /// <summary>Переоткрывает оповещение и все его нарушения.</summary>
    public void Reopen(string user, string comment)
    {
        if (Status == AlertStatus.Active)
            throw new InvalidOperationException("Оповещение уже активно.");

        Status = AlertStatus.Active;
        AcknowledgedBy = null;
        AcknowledgedAt = null;
        AcknowledgedComment = null;
        ResolvedBy = null;
        ResolvedAt = null;
        ResolvedComment = null;
        LastUpdatedAt = DateTime.UtcNow;

        foreach (var v in Violations)
            v.Reopen();

        Recalculate();
        AddHistory(AlertAction.Reopened, user, comment);
    }

    /// <summary>
    /// Пересчитывает Severity, Type, счётчики и эскалацию
    /// на основе текущего состояния коллекции нарушений.
    /// </summary>
    private void Recalculate()
    {
        ActiveViolationCount = Violations.Count(v => !v.IsResolved);

        var active = Violations.Where(v => !v.IsResolved).ToList();

        if (active.Count == 0)
        {
            // Нет активных нарушений — Severity = Info, эскалация сбрасывается.
            Severity = AlertSeverity.Info;
            IsEscalated = false;
            EscalationCount = 0;
            return;
        }

        Severity = active.Max(v => v.ViolationSeverity);
        IsEscalated = active.Any(v => v.IsEscalated);
        EscalationCount = active.Sum(v => v.EscalationCount);

        var top = active
            .Where(v => v.ViolationSeverity == Severity)
            .OrderByDescending(v => TypePriority.GetValueOrDefault(v.ViolationType))
            .ThenByDescending(v => v.CurrentValue)
            .FirstOrDefault();

        if (top != null && Type != top.ViolationType)
        {
            var oldType = Type;
            Type = top.ViolationType;
            AddHistory(AlertAction.TypeChanged, "System",
                $"Тип изменён: {oldType} → {Type}");
        }
    }

    /// <summary>Добавляет запись в историю статусов.</summary>
    private void AddHistory(AlertAction action, string user, string comment)
    {
        StatusHistory.Add(new AlertStatusHistory
        {
            AlertId = Id,
            Action = action,
            User = user,
            Comment = comment,
            Timestamp = DateTime.UtcNow
        });
    }
}
