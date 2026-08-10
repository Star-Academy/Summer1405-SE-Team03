using System.Data;
using FluentAssertions;
using MyNewProjectName.Presentation.Business;
using Xunit;

namespace MyNewProjectName.Tests.Presentation;

public class StudentQueryResultPresenterTests
{
    private readonly StudentQueryResultPresenter _sut;

    public StudentQueryResultPresenterTests()
    {
        _sut =  new StudentQueryResultPresenter(new DefaultRowFormatter());
    }

    [Theory]
    [InlineData("123", "Ali")]
    [InlineData("456", "Reza")]
    [InlineData("789", "Zahra")]
    public void PresentResults_ShouldWriteFormattedOutputToConsole_WhenDataReaderContainsRows(string studentNumber, string firstName)
    {
        //arrange
        var table = new DataTable();
        table.Columns.Add("studentnumber", typeof(string));
        table.Columns.Add("firstname", typeof(string));
        table.Rows.Add(studentNumber, firstName);
        using var reader = table.CreateDataReader();
        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        
        //act
        _sut.PresentResults(reader);
        
        //assert
        var output = stringWriter.ToString();
        var expectedOutput = $"studentnumber: {studentNumber}, firstname: {firstName}";
        
        output.Should().Contain(expectedOutput);
    }
}