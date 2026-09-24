namespace VibrationMonitoring.Domain.Entities
{
    /// <summary>
    /// Одно выполнение маршрута диагностики (историческая запись).
    /// </summary>
    public class RouteExecution : BaseEntity
    {
        /// <summary>FK на маршрут.</summary>
        public Guid RouteId { get; set; }

        /// <summary>Навигационное свойство на маршрут.</summary>
        public DiagnosticRoute Route { get; set; } = null!;

        /// <summary>Кто выполнял.</summary>
        public string ExecutedBy { get; set; } = null!;

        /// <summary>Когда начато.</summary>
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        /// <summary>Когда завершено. <c>null</c> — ещё выполняется.</summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>Элементы выполнения.</summary>
        public ICollection<ExecutionItem> Items { get; set; } = new List<ExecutionItem>();

        // --- Вычисляемые свойства (для DTO, не транслируются в SQL) ---

        /// <summary>Длительность.</summary>
        public TimeSpan Duration => (CompletedAt ?? DateTime.UtcNow) - StartedAt;

        /// <summary>Завершено ли выполнение.</summary>
        public bool IsCompleted => CompletedAt.HasValue;

        /// <summary>Общее количество элементов.</summary>
        public int TotalItems => Items.Count;

        /// <summary>Количество завершённых элементов.</summary>
        public int CompletedItems => Items.Count(i => i.Status == PointStatus.Completed);

        /// <summary>Прогресс в процентах.</summary>
        public double ProgressPercentage => TotalItems > 0 ? CompletedItems * 100.0 / TotalItems : 0;
    }
}
