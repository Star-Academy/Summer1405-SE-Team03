using MyNewProjectName.Grammars.Business;
using FluentAssertions;
using Xunit;

namespace MyNewProjectName.Tests.Grammars;
public class SqlServerSyntaxFormatterTests
{
    [Fact]
    public void FormatIdentifier_Should_WrapStringInDoubleQuotes_When_CalledWithValidString()
    {
        var sqlServerSyntaxFormatter = new SqlServerSyntaxFormatter();
        var columnName = "studentnumber";
        var sqlServerResultFormatter =  sqlServerSyntaxFormatter.FormatIdentifier(columnName);
        sqlServerResultFormatter.Should().Be("[studentnumber]");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void GetParameterName_Should_ReturnParameterWithAtPrefixedAtP_When_IndexIsProvided(int index)
    {
        var sqlServerSyntaxFormatter = new SqlServerSyntaxFormatter();
        var sqlServersResultFormatter = sqlServerSyntaxFormatter.GetParameterName(index);
        sqlServersResultFormatter.Should().Be($"@p{index}");
    }

    [Fact]
    public void ParametrStartIndex_Should_BeZero_When_YouWantToAccessTheDatabase()
    {
        var sqlServerSyntaxFormatter = new SqlServerSyntaxFormatter();
        var sqlServerResultFormatter = sqlServerSyntaxFormatter.ParameterStartIndex;
        sqlServerResultFormatter.Should().Be(0);
    }
}