using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MyWebApi.Models;
using MyWebApi.Services.Abstractions;
using Npgsql;
using SqlKata.Execution;

namespace MyWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IDbService _dbService;

    public StudentsController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] string db)
    {
        try
        {
            var queryFactory = _dbService.GetQueryFactory(db);
            var selectedStudents = queryFactory.Query("student").Get();
            return Ok(selectedStudents);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id, [FromQuery] string db)
    {
        try
        {
            var queryFactory = _dbService.GetQueryFactory(db);
            var selectedStudents = queryFactory.Query("student").Where("studentnumber", id).FirstOrDefault<Student>();
            if (selectedStudents == null)
            {
                return NotFound(new { message = $"Student {id} not found" });
            }

            return Ok(selectedStudents);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public IActionResult Create([FromQuery] string db, [FromBody] Student student)
    {
        try
        {
            if (student == null)
            {
                return BadRequest(new { error = "Student data is required." });
            }

            if (student.StudentNumber <= 0)
            {
                return BadRequest(new { error = "Student number must be greater than zero." });
            }

            if (!string.IsNullOrWhiteSpace(student.FirstName) && student.FirstName.Length > 50)
            {
                return BadRequest(new { error = "First name cannot exceed 50 characters." });
            }

            if (student.Grade < 0 || student.Grade > 20)
            {
                return BadRequest(new { error = "Grade must be between 0 and 20." });
            }

            var queryFactory = _dbService.GetQueryFactory(db);

            bool isDuplicate = queryFactory.Query("student")
                .Where("studentnumber", student.StudentNumber)
                .Exists();

            if (isDuplicate)
            {
                return Conflict(new { error = $"Student with number {student.StudentNumber} already exists." });
            }

            var affectedRows = queryFactory.Query("student").Insert(new
            {
                studentnumber = student.StudentNumber,
                firstname = student.FirstName,
                ismale = student.IsMale,
                grade = student.Grade
            });

            if (affectedRows == 0)
            {
                return StatusCode(500, new { error = "Failed to insert record into the database." });
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = student.StudentNumber, db = db },
                student);
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            return Conflict(new { error = $"Student with number {student.StudentNumber} already exists." });
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            return Conflict(new { error = $"Student with number {student.StudentNumber} already exists." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] int id, [FromQuery] string db, [FromBody] Student student)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest(new { error = "Student number must be greater than zero." });
            }

            if (student == null)
            {
                return BadRequest(new { error = "Student data is required." });
            }

            if (!string.IsNullOrWhiteSpace(student.FirstName) && student.FirstName.Length > 50)
            {
                return BadRequest(new { error = "First name cannot exceed 50 characters." });
            }

            if (student.Grade < 0 || student.Grade > 20)
            {
                return BadRequest(new { error = "Grade must be between 0 and 20." });
            }

            var queryFactory = _dbService.GetQueryFactory(db);

            var affectedRows = queryFactory.Query("student")
                .Where("studentnumber", id)
                .Update(new
                {
                    firstname = student.FirstName,
                    ismale = student.IsMale,
                    grade = student.Grade
                });

            if (affectedRows == 0)
            {
                return NotFound(new { error = $"Student with number {id} not found." });
            }

            student.StudentNumber = id;
            return Ok(student);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id, [FromQuery] string db)
    {
        try
        {
            var queryFactory = _dbService.GetQueryFactory(db);
            var deletedStudents = queryFactory.Query("student").Where("studentnumber", id).Delete();
            if (deletedStudents == 0)
            {
                return NotFound($"Student {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}