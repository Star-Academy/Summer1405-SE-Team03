using System.Data;

namespace MyNewProjectName.Presentation.Abstractions;

public interface IRowFormatter
{
    string FormatRow(IDataReader reader);
}