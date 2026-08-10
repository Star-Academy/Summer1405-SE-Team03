using System.Collections.Generic;
using System.Data;
using MyNewProjectName.Presentation.Abstractions;

namespace MyNewProjectName.Presentation.Business;

internal sealed class DefaultRowFormatter : IRowFormatter
{
    public string FormatRow(IDataReader reader)
    {
        var rowValues = new List<string>();
        for (var i = 0; i < reader.FieldCount; i++)
        {
            var columnName = reader.GetName(i);
            var value = reader.GetValue(i);
            rowValues.Add($"{columnName}: {value}");
        }
        
        return string.Join(", ", rowValues);
    }
}