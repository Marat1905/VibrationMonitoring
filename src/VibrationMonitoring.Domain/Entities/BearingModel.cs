using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationMonitoring.Domain.Entities;

/// <summary>
/// Локальная реплика справочника моделей подшипников из сервиса обслуживания.
/// Обновляется через gRPC (массовая загрузка) и RabbitMQ (инкрементальные изменения).
/// </summary>
public class BearingModel : BaseEntity
{
    /// <summary>Идентификатор модели во внешней системе (AssetTracker). Стабильный ключ.</summary>
    public string ExternalId { get; set; } = null!;

    /// <summary>Человекочитаемый код/маркировка, например "6205-2RS".</summary>
    public string Code { get; set; } = null!;

    /// <summary>Отображаемое имя.</summary>
    public string Name { get; set; } = null!;

    /// <summary>Производитель.</summary>
    public string? Manufacturer { get; set; }

    // --- Геометрические параметры для расчёта частот (BPFO, BPFI, BSF, FTF) ---

    /// <summary>Внутренний диаметр, мм.</summary>
    public double? InnerDiameterMm { get; set; }

    /// <summary>Наружный диаметр, мм.</summary>
    public double? OuterDiameterMm { get; set; }

    /// <summary>Ширина, мм.</summary>
    public double? WidthMm { get; set; }

    /// <summary>Количество тел качения.</summary>
    public int? BallCount { get; set; }

    /// <summary>Диаметр тела качения, мм.</summary>
    public double? BallDiameterMm { get; set; }

    /// <summary>Диаметр дорожки (pitch diameter), мм.</summary>
    public double? PitchDiameterMm { get; set; }

    /// <summary>Угол контакта, градусы. Для радиальных подшипников = 0.</summary>
    public double? ContactAngleDeg { get; set; }

    /// <summary>Максимальная частота вращения, об/мин.</summary>
    public int? MaxRpm { get; set; }

    /// <summary>Момент последней синхронизации с внешней системой (UTC).</summary>
    public DateTime SyncedAt { get; set; } = DateTime.UtcNow;
}