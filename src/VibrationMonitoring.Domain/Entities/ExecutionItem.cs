using VibrationMonitoring.Domain.Enums;

namespace VibrationMonitoring.Domain.Entities
{
    /// <summary>
    /// Элемент выполнения маршрута — привязка к элементу маршрута + результат.
    /// </summary>
    public class ExecutionItem : BaseEntity
    {
        /// <summary>FK на выполнение.</summary>
        public Guid ExecutionId { get; set; }

        /// <summary>Навигационное свойство на выполнение.</summary>
        public RouteExecution Execution { get; set; } = null!;

        /// <summary>FK на элемент маршрута.</summary>
        public Guid RouteItemId { get; set; }

        /// <summary>Навигационное свойство на элемент маршрута.</summary>
        public DiagnosticRouteItem RouteItem { get; set; } = null!;

        /// <summary>Когда было выполнено измерение.</summary>
        public DateTime? MeasuredAt { get; set; }

        /// <summary>Статус прохождения.</summary>
        public PointStatus Status { get; set; } = PointStatus.Pending;

        /// <summary>Заметки оператора.</summary>
        public string? Notes { get; set; }

        /// <summary>FK на измерение, если оно было создано.</summary>
        public Guid? MeasurementId { get; set; }

        /// <summary>Навигационное свойство на измерение.</summary>
        public Measurement? Measurement { get; set; }
    }
}
