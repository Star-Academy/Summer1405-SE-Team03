using System;
using FluentAssertions;
using MyNewProjectName.Compilers.Business;
using MyNewProjectName.Core;
using MyNewProjectName.Grammars.Abstractions;
using NSubstitute;
using Xunit;

namespace MyNewProjectName.Tests.Compilers;

public class FromClauseBuilderTests
{
    private readonly IDatabaseSpecificSyntaxFormatter _formatterSubstitute;
    private readonly FromClauseBuilder _sut;

    public FromClauseBuilderTests()
    {
        _formatterSubstitute = Substitute.For<IDatabaseSpecificSyntaxFormatter>();
        _sut = new FromClauseBuilder(_formatterSubstitute);
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_FormatterIsNull()
    {
        // act
        var nullAction = () => new FromClauseBuilder(null!);
        // assert
        nullAction.Should().Throw<ArgumentNullException>().WithParameterName("databaseSpecificSyntaxFormatter");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_Should_ThrowInvalidOperationException_When_FormatterIsWhitespace(string invalidFormatter)
    {
        var invalidQuery = new Query().From(invalidFormatter);
        Action act = () => _sut.Build(invalidQuery);
        act.Should().Throw<InvalidOperationException>().WithMessage("Table name cannot be null or empty.");
    }

    [Fact]
    public void Constructor_Should_ThrowArgumentNullException_When_FromClauseBuilderIsValid()
    {
        //arrangr
        var validQuery = new Query().From("student");
        _formatterSubstitute.FormatIdentifier("student").Returns("\"student\"");
        //act
        var validResult = _sut.Build(validQuery);
        //assert
        validResult.Should().Be(" FROM \"student\"");
        _formatterSubstitute.Received(1).FormatIdentifier("student");
    }
}