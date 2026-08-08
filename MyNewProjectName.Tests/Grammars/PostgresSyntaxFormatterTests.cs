using MyNewProjectName.Grammars.Business;
using FluentAssertions;
using Xunit;

namespace MyNewProjectName.Tests.Grammars;
public class PostgresSyntaxFormatterTests
{
    [Fact]
    public void FormatIdentifier_Should_WrapStringInDoubleQuotes_When_CalledWithValidString()
    {
        var postgresSyntaxFormatter = new PostgresSyntaxFormatter();
        var columnName = "studentnumber";
        var postgresResultFormatter =  postgresSyntaxFormatter.FormatIdentifier(columnName);
        postgresResultFormatter.Should().Be("\"studentnumber\"");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void GetParameterName_Should_ReturnParameterWithDollarPrefix_When_IndexIsProvided(int index)
    {
        var postgresSyntaxFormatter = new PostgresSyntaxFormatter();
        var postgresResultFormatter = postgresSyntaxFormatter.GetParameterName(index);
        postgresResultFormatter.Should().Be($"${index}");
    }

    [Fact]
    public void ParametrStartIndex_Should_BeOne_When_YouWantToAccessTheDatabase()
    {
        var postgresSyntaxFormatter = new PostgresSyntaxFormatter();
        var postgresResultFormatter = postgresSyntaxFormatter.ParameterStartIndex;
        postgresResultFormatter.Should().Be(1);
    }
}
