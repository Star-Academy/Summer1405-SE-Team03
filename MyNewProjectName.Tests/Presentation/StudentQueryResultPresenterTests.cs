using System.Data;
using FluentAssertions;
using MyNewProjectName.Presentation.Business;
using Xunit;

namespace MyNewProjectName.Tests.Presentation;

public class StudentQueryResultPresenterTests
{
    [Theory]
    [InlineData("123", "Ali")]
    [InlineData("456", "Reza")]
    [InlineData("789", "Zahra")]
    public void PresentResults_Should_WriteFormattedOutputToConsole_When_DataReaderContainsRows(string studentNumber, string firstName)
    {
        // Arrange
        var table = new DataTable();
        table.Columns.Add("studentnumber", typeof(string));
        table.Columns.Add("firstname", typeof(string));
        table.Rows.Add(studentNumber, firstName);

        using var reader = table.CreateDataReader();
        var presenter = new StudentQueryResultPresenter();

        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        // Act
        presenter.PresentResults(reader);

        // Assert
        var output = stringWriter.ToString();
        var expectedOutput = $"Student Number: {studentNumber}, Name: {firstName}";
        
        output.Should().Contain(expectedOutput);
    }
}