using MyWebApi.Models;

namespace MyWebApi.Repositories.Abstractions;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> GetAllAsync(string? db);

    Task<Student?> GetByIdAsync(int studentNumber, string? db);

    Task<bool> ExistsAsync(int studentNumber, string? db);

    Task<int> InsertAsync(Student student, string? db);

    Task<int> UpdateAsync(Student student, string? db);

    Task<int> DeleteAsync(int studentNumber, string? db);
}