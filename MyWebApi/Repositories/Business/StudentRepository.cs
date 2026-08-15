using Microsoft.Data.SqlClient;
using MyWebApi.Constants;
using MyWebApi.Exceptions;
using MyWebApi.Models;
using MyWebApi.Repositories.Abstractions;
using MyWebApi.Services.Abstractions;
using Npgsql;
using SqlKata.Execution;

namespace MyWebApi.Repositories.Business;

public class StudentRepository : IStudentRepository
{
    private readonly IDbService _dbService;

    public StudentRepository(IDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<IEnumerable<Student>> GetAllAsync(string? db)
    {
        using var queryFactory = _dbService.GetQueryFactory(db);

        return await queryFactory
            .Query(StudentDbConstants.TableName)
            .GetAsync<Student>();
    }

    public async Task<Student?> GetByIdAsync(int studentNumber, string? db)
    {
        using var queryFactory = _dbService.GetQueryFactory(db);

        return await queryFactory
            .Query(StudentDbConstants.TableName)
            .Where(StudentDbConstants.StudentNumberColumn, studentNumber)
            .FirstOrDefaultAsync<Student>();
    }


    public async Task<int> InsertAsync(Student student, string? db)
    {
        using var queryFactory = _dbService.GetQueryFactory(db);

        try
        {
            return await queryFactory
                .Query(StudentDbConstants.TableName)
                .InsertAsync(new
                {
                    studentnumber = student.StudentNumber,
                    firstname = student.FirstName,
                    ismale = student.IsMale,
                    grade = student.Grade
                });
        }
        catch (PostgresException ex) when (ex.SqlState == DbErrorCodes.Postgres.UniqueViolation)
        {
            throw new DuplicateResourceException(
                $"Student with number {student.StudentNumber} already exists."
            );
        }
        catch (SqlException ex) when (ex.Number is DbErrorCodes.SqlServer.ViolationOfUniqueConstraint or DbErrorCodes.SqlServer.CannotInsertDuplicateKeyRow)
        {
            throw new DuplicateResourceException(
                $"Student with number {student.StudentNumber} already exists."
            );
        }
    }

    public async Task<int> UpdateAsync(Student student, string? db)
    {
        using var queryFactory = _dbService.GetQueryFactory(db);

        return await queryFactory
            .Query(StudentDbConstants.TableName)
            .Where(StudentDbConstants.StudentNumberColumn, student.StudentNumber)
            .UpdateAsync(new
            {
                firstname = student.FirstName,
                ismale = student.IsMale,
                grade = student.Grade
            });
    }

    public async Task<int> DeleteAsync(int studentNumber, string? db)
    {
        using var queryFactory = _dbService.GetQueryFactory(db);

        return await queryFactory
            .Query(StudentDbConstants.TableName)
            .Where(StudentDbConstants.StudentNumberColumn, studentNumber)
            .DeleteAsync();
    }
}