namespace VibrationMonitoring.Domain.Entities;

/// <summary>
/// Локальный кэш-снимок двигателя из AssetTracker.
/// Хранит только данные, необходимые для диагностики и расчёта частот.
/// </summary>
public class MotorSnapshot : BaseEntity
{
    /// <summary>Код двигателя в AssetTracker (Motor.Code). Стабильный ключ.</summary>
    public string MotorCode { get; set; } = null!;

    /// <summary>Отображаемое имя.</summary>
    public string DisplayName { get; set; } = null!;

    /// <summary>Марка/модель.</summary>
    public string Type { get; set; } = null!;

    /// <summary>Мощность, кВт.</summary>
    public double PowerKw { get; set; }

    /// <summary>Номинальная частота вращения, об/мин.</summary>
    public int SpeedRpm { get; set; }

    /// <summary>Диаметр вала, мм.</summary>
    public double ShaftDiameterMm { get; set; }

    /// <summary>Тип монтажа.</summary>
    public string? MountingType { get; set; }

    /// <summary>Момент последней синхронизации (UTC).</summary>
    public DateTime SyncedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Снимки подшипников, установленных на двигателе.</summary>
    public ICollection<MotorBearingSnapshot> Bearings { get; set; } = new List<MotorBearingSnapshot>();
}

/// <summary>
/// Снимок связи «двигатель — установленный подшипник» из AssetTracker.
/// </summary>
public class MotorBearingSnapshot : BaseEntity
{
    /// <summary>FK на снимок двигателя.</summary>
    public Guid MotorSnapshotId { get; set; }

    /// <summary>Навигационное свойство на снимок двигателя.</summary>
    public MotorSnapshot MotorSnapshot { get; set; } = null!;

    /// <summary>Позиция подшипника.</summary>
    public Enums.BearingPosition Position { get; set; }

    /// <summary>ExternalId модели подшипника в AssetTracker.</summary>
    public string BearingModelExternalId { get; set; } = null!;

    /// <summary>Код модели подшипника (дублируется для удобства отображения без join).</summary>
    public string BearingModelCode { get; set; } = null!;

    /// <summary>Дата установки.</summary>
    public DateTime? InstalledAt { get; set; }
}