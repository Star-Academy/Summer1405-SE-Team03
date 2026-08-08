using System;
using FluentAssertions;
using MyNewProjectName.Compilers.Business;
using MyNewProjectName.Grammars.Abstractions;
using Xunit;

namespace MyNewProjectName.Tests.Compilers;

public class FromClauseBuilderTests
{
    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_FormatterIsNull()
    {
        // Arrange
        IDatabaseSpecificSyntaxFormatter nullFormatter = null;

        // Act
        Action act = () => new FromClauseBuilder(nullFormatter);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("databaseSpecificSyntaxFormatter");
    }
}