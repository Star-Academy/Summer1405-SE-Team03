using MyWebApi.Dtos.Student;
using MyWebApi.Dtos.Student.Request;
using MyWebApi.Dtos.Student.Response;
using MyWebApi.Exceptions;
using MyWebApi.Mapping;
using MyWebApi.Models;
using MyWebApi.Repositories.Abstractions;
using MyWebApi.Services.Abstractions;

namespace MyWebApi.Services.Business;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<StudentService> _logger;

    public StudentService(
        IStudentRepository studentRepository,
        ILogger<StudentService> logger)
    {
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<StudentResponse>> GetAllAsync(string? db)
    {
        var students = await _studentRepository.GetAllAsync(db);

        return students
            .Select(student => student.ToResponse())
            .ToList();
    }

    public async Task<StudentResponse> GetByIdAsync(int id, string? db)
    {
        var student = await _studentRepository.GetByIdAsync(id, db);

        if (student is null)
        {
            throw new ResourceNotFoundException($"Student {id} not found");
        }

        return student.ToResponse();
    }

    public async Task<StudentResponse> CreateAsync(CreateStudentRequest request, string? db)
    {
        if (request is null)
        {
            throw new BadRequestException("Student data is required.");
        }

        var student = request.ToEntity();

        var insertedRows = await _studentRepository.InsertAsync(student, db);

        if (insertedRows == 0)
        {
            throw new InvalidOperationException("Failed to insert record into the database.");
        }

        _logger.LogInformation("Student {StudentNumber} created.", student.StudentNumber);

        return student.ToResponse();
    }

    public async Task<StudentResponse> UpdateAsync(
        int id,
        UpdateStudentRequest request,
        string? db)
    {

        if (request is null)
        {
            throw new BadRequestException("Student data is required.");
        }

        var student = request.ToEntity(id);

        var updatedRows = await _studentRepository.UpdateAsync(student, db);

        if (updatedRows == 0)
        {
            throw new ResourceNotFoundException($"Student with number {id} not found.");
        }

        _logger.LogInformation("Student {StudentNumber} updated.", student.StudentNumber);

        return student.ToResponse();
    }

    public async Task DeleteAsync(int id, string? db)
    {
        var deletedRows = await _studentRepository.DeleteAsync(id, db);

        if (deletedRows == 0)
        {
            throw new ResourceNotFoundException($"Student {id} not found");
        }

        _logger.LogInformation("Student {StudentNumber} deleted.", id);
    }
    
}