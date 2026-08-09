using MyNewProjectName.Core;
using FluentAssertions;
using Xunit;

namespace MyNewProjectName.Tests.Core;
public class CompiledQueryTests
{
    [Fact]
    public void Constructor_ShouldSetProperties_WhenInitializedWithValidParameters()
    {
        var expectedSql = "SELECT * FROM \"student\"";
        var expectedBindings = new List<object>{1 , "test"};
        var compileQuery = new CompiledQuery(expectedSql, expectedBindings);
        compileQuery.SqlQuery.Should().Be(expectedSql);
        compileQuery.Bindings.Should().BeEquivalentTo(expectedBindings);
    }
}