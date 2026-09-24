using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibrationMonitoring.Domain.Enums;

/// <summary>
/// Действие, зафиксированное в истории нарушения.
/// </summary>
public enum ViolationAction
{
    Created = 0,
    Escalated = 1,
    Resolved = 2,
    Reopened = 3
}