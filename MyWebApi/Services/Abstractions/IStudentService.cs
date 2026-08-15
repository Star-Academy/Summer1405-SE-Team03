using MyWebApi.Dtos.Student;
using MyWebApi.Dtos.Student.Request;
using MyWebApi.Dtos.Student.Response;

namespace MyWebApi.Services.Abstractions;

public interface IStudentService
{
    Task<IEnumerable<StudentResponse>> GetAllAsync(string? db);

    Task<StudentResponse> GetByIdAsync(int id, string? db);

    Task<StudentResponse> CreateAsync(CreateStudentRequest request, string? db);

    Task<StudentResponse> UpdateAsync(int id, UpdateStudentRequest request, string? db);

    Task DeleteAsync(int id, string? db);
}