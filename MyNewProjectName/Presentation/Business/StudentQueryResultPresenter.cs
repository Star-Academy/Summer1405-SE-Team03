using System;
using System.Data;
using MyNewProjectName.Presentation.Abstractions;

namespace MyNewProjectName.Presentation.Business;

internal sealed class StudentQueryResultPresenter : IQueryResultPresenter
{
    private readonly IRowFormatter _rowFormatter;

    public StudentQueryResultPresenter(IRowFormatter rowFormatter)
    {
        _rowFormatter = rowFormatter ?? throw new ArgumentNullException(nameof(rowFormatter));
    }

    public void PresentResults(IDataReader reader)
    {
        while (reader.Read())
        {
            Console.WriteLine(_rowFormatter.FormatRow(reader));
        }
    }
}