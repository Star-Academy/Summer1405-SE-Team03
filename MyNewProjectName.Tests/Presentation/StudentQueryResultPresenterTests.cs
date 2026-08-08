using System.Data;
using MyNewProjectName.Presentation.Business;

namespace MyNewProjectName.Tests.Presentation;

public class StudentQueryResultPresenterTests
{
    [Fact]
    public void Constructor_Should_GetReaderAndWriteInConsole_When_WeReachInTheFinal()
    {
        var table = new DataTable();
        table.Columns.Add("studentnumber", typeof(string));
        table.Columns.Add("firstname", typeof(string));
        table.Rows.Add("123", "Ali");
        table.Rows.Add("456", "Reza");

        using var reader = table.CreateDataReader();
        var presenter = new StudentQueryResultPresenter();

        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        // Act
        presenter.PresentResults(reader);

        // Assert
        var output = stringWriter.ToString();
        Assert.Contains("Student Number: 123, Name: Ali", output);
        Assert.Contains("Student Number: 456, Name: Reza", output);
        
}
    }