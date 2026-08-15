using Microsoft.AspNetCore.Mvc;
using MyWebApi.Dtos.Student;
using MyWebApi.Dtos.Student.Request;
using MyWebApi.Dtos.Student.Response;
using MyWebApi.Services.Abstractions;

namespace MyWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentResponse>>> GetAll([FromQuery] string? db)
    {
        var students = await _studentService.GetAllAsync(db);

        return Ok(students);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentResponse>> GetById(
        [FromRoute] int id,
        [FromQuery] string? db)
    {
        var student = await _studentService.GetByIdAsync(id, db);

        return Ok(student);
    }

    [HttpPost]
    public async Task<ActionResult<StudentResponse>> Create(
        [FromQuery] string? db,
        [FromBody] CreateStudentRequest request)
    {
        var student = await _studentService.CreateAsync(request, db);

        return CreatedAtAction(
            nameof(GetById),
            new { id = student.StudentNumber, db },
            student
        );
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<StudentResponse>> Update(
        [FromRoute] int id,
        [FromQuery] string? db,
        [FromBody] UpdateStudentRequest request)
    {
        var student = await _studentService.UpdateAsync(id, request, db);

        return Ok(student);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        [FromRoute] int id,
        [FromQuery] string? db)
    {
        await _studentService.DeleteAsync(id, db);

        return NoContent();
    }
}