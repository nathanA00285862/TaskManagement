using System.Collections.Generic;
using TaskManagement.Domain;

namespace Exporter.Abstractions;

public interface IExporter
{
    string GetFormatName();
    byte[] Export(IEnumerable<TaskItem> tasks);
}