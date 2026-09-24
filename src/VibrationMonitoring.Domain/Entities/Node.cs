using System.ComponentModel;
using System.Diagnostics.Metrics;
using VibrationMonitoring.Domain.Enums;

namespace VibrationMonitoring.Domain.Entities;

/// <summary>
/// Узел иерархического дерева оборудования.
/// Хранит только общую структуру; специфика — в дочерних сущностях
/// (<see cref="Equipment"/>, <see cref="Component"/>, <see cref="Bearing"/>, <see cref="MeasurementPoint"/>).
/// </summary>
public class Node : BaseEntity
{
    /// <summary>
    /// Стабильный бизнес-ключ. Уникален. Используется для интеграции
    /// (RabbitMQ / gRPC) вместо внутреннего <see cref="BaseEntity.Id"/>.
    /// Пример: "RPO/STU-081/ED/BRG-F".
    /// </summary>
    public string Code { get; private set; } = null!;

    /// <summary>Отображаемое имя. Может меняться свободно.</summary>
    public string Name { get; private set; } = null!;

    /// <summary>Тип узла (раздел, оборудование, компонент, подшипник, точка).</summary>
    public NodeType Type { get; private set; }

    /// <summary>Идентификатор родительского узла. <c>null</c> для корня.</summary>
    public Guid? ParentId { get; private set; }

    /// <summary>Навигационное свойство на родителя. <c>null</c> для корня.</summary>
    public Node? Parent { get; private set; }

    /// <summary>Дочерние узлы.</summary>
    public ICollection<Node> Children { get; private set; } = new List<Node>();

    /// <summary>
    /// Materialized path — материализованный путь для быстрого поиска поддеревьев.
    /// Формат: "/{rootId}/{childId}/.../{thisId}/".
    /// Меняется только через <see cref="SetPath"/>, обычно — сервисом дерева.
    /// </summary>
    public string Path { get; private set; } = "/";

    /// <summary>Глубина узла в дереве. Корень = 0.</summary>
    public int Depth { get; private set; }

    /// <summary>Порядок отображения среди соседей одного родителя.</summary>
    public int SortOrder { get; private set; }

    /// <summary>
    /// Код связанного актива во внешней системе (AssetTracker).
    /// Для узлов типа <see cref="NodeType.Component"/> с <c>Kind = Motor</c>
    /// содержит <c>Motor.Code</c>. <c>null</c> для узлов, не связанных с внешней системой.
    /// </summary>
    public string? ExternalAssetCode { get; private set; }

    // --- Специфичные сущности (1:1, опционально в зависимости от Type) ---

    /// <summary>Данные оборудования. Заполнено только для <see cref="NodeType.Equipment"/>.</summary>
    public Equipment? Equipment { get; private set; }

    /// <summary>Данные компонента. Заполнено только для <see cref="NodeType.Component"/>.</summary>
    public Component? Component { get; private set; }

    /// <summary>Данные подшипника. Заполнено только для <see cref="NodeType.Bearing"/>.</summary>
    public Bearing? Bearing { get; private set; }

    /// <summary>Данные точки измерения. Заполнено только для <see cref="NodeType.Point"/>.</summary>
    public MeasurementPoint? MeasurementPoint { get; private set; }

    // --- Общие коллекции ---

    /// <summary>Измерения, привязанные к узлу (обычно к точке измерения).</summary>
    public ICollection<Measurement> Measurements { get; private set; } = new List<Measurement>();


    /// <summary>Конструктор для EF Core.</summary>
    private Node() { }

    /// <summary>Создаёт новый узел дерева.</summary>
    public Node(string code, string name, NodeType type, string? externalAssetCode = null)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code обязателен.", nameof(code));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name обязателен.", nameof(name));

        Code = code;
        Name = name;
        Type = type;
        ExternalAssetCode = externalAssetCode;
    }

    /// <summary>Устанавливает materialized path и глубину. Вызывается сервисом дерева.</summary>
    public void SetPath(string path, int depth)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Path не может быть пустым.", nameof(path));
        if (depth < 0)
            throw new ArgumentOutOfRangeException(nameof(depth));

        Path = path;
        Depth = depth;
    }

    /// <summary>Меняет родителя. Полный пересчёт Path делает сервис дерева.</summary>
    public void ChangeParent(Guid? parentId) => ParentId = parentId;

    /// <summary>Переименование узла. Не влияет на Code.</summary>
    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name обязателен.", nameof(name));
        Name = name;
    }

    /// <summary>Устанавливает порядок отображения среди соседей.</summary>
    public void SetSortOrder(int order) => SortOrder = order;

    /// <summary>Связывает узел с внешним активом (например, с Motor.Code).</summary>
    public void SetExternalAssetCode(string? code) => ExternalAssetCode = code;

    /// <summary>Помечает узел как удалённый. Для всего поддерева использовать сервис дерева.</summary>
    public void MarkDeleted() => IsDeleted = true;
}