using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationMonitoring.Domain.Enums;

/// <summary>
/// Тип узла в иерархическом дереве оборудования.
/// </summary>
public enum NodeType
{
    /// <summary>Раздел (например, РПО).</summary>
    Section = 0,

    /// <summary>Оборудование (например, STU-081).</summary>
    Equipment = 1,

    /// <summary>Компонент оборудования (ротор, электродвигатель, насос).</summary>
    Component = 2,

    /// <summary>Подшипник (передний, задний и т.п.).</summary>
    Bearing = 3,

    /// <summary>Точка измерения (вертикальная, горизонтальная, осевая).</summary>
    Point = 4
}
