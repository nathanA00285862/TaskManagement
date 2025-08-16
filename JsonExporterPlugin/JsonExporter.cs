using Exporter.Abstractions;
using System.Text.Json;
using TaskManagement.Domain;
using System.Collections.Generic;

namespace JsonExporterPlugin;

public class JsonExporter : IExporter
{
    public string GetFormatName() => "json";

    public byte[] Export(IEnumerable<TaskItem> tasks)
    {
        var json = JsonSerializer.Serialize(tasks);
        return System.Text.Encoding.UTF8.GetBytes(json);
    }
}