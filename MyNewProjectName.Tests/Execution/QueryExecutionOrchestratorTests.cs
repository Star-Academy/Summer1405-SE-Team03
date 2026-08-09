using System;
using System.Collections.Generic;
using FluentAssertions;
using MyNewProjectName.Compilers.Abstractions;
using MyNewProjectName.Core;
using MyNewProjectName.Execution.Abstractions;
using MyNewProjectName.Execution.Business;
using NSubstitute;
using Xunit;

namespace MyNewProjectName.Tests.Execution;

public class QueryExecutionOrchestratorTests
{
    private readonly ISqlQueryCompiler _compiler;
    private readonly ICompiledQueryRunner _dbRunner;
    private readonly QueryExecutionOrchestrator _sut;

    public QueryExecutionOrchestratorTests()
    {
        _compiler = Substitute.For<ISqlQueryCompiler>();
        _dbRunner = Substitute.For<ICompiledQueryRunner>();
        _sut = new QueryExecutionOrchestrator(_compiler, _dbRunner);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenCompilerIsNull()
    {
        // Arrange
        // Act
        var action = () => new QueryExecutionOrchestrator(null!, _dbRunner);

        // Assert
        action.Should().Throw<ArgumentNullException>()
            .WithParameterName("compiler");
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenDbRunnerIsNull()
    {
        // Arrange
        // Act
        var action = () => new QueryExecutionOrchestrator(_compiler, null!);

        // Assert
        action.Should().Throw<ArgumentNullException>()
            .WithParameterName("dbRunner");
    }

    [Fact]
    public void ExecuteQuery_ShouldCompileAndRunQueryInCorrectSequence_Whenever()
    {
        // Arrange
        var query = new Query().From("users");
        var compiledQuery = new CompiledQuery("SELECT * FROM \"users\"", new List<object>());

        _compiler.Compile(query).Returns(compiledQuery);

        // Act
        _sut.ExecuteQuery(query);

        // Assert
        Received.InOrder(() =>
        {
            _compiler.Compile(query);
            _dbRunner.QueryRunner(compiledQuery, query);
        });
    }
}