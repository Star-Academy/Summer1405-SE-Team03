using Microsoft.AspNetCore.Mvc;
using MyWebApi.Models;
using MyWebApi.Services.Abstractions;
using SqlKata.Execution;

namespace MyWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController:ControllerBase
{
    private readonly IDbService _dbService;
    
    public StudentsController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] string dbConnectionString)
    {
        try
        {
            var queryFactory = _dbService.GetQueryFactory(dbConnectionString);
            var selectedStudents = queryFactory.Query("student").Get();
            return Ok(selectedStudents);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id, [FromQuery] string dbConnectionString)
    {
        try
        {
            var queryFactory = _dbService.GetQueryFactory(dbConnectionString);
            var selectedStudents = queryFactory.Query("student").Where("studentnumber", id).Get();
            return Ok(selectedStudents);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public IActionResult Create([FromQuery] string dbConnectionString, [FromBody] Student student)
    {
        try
        {
            var queryFactory = _dbService.GetQueryFactory(dbConnectionString);
            var affectedRows = queryFactory.Query("student").Insert(student);
            if (affectedRows == 0)
            {
                return StatusCode(500 , "cant add record");
            }
            return CreatedAtAction(
                nameof(GetById), 
                new { id = student.StudentNumber, dbConnectionString = dbConnectionString }, student);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] int id, [FromQuery] string dbConnectionString, [FromBody] Student student)
    {
        try
        {
            var queryFactory = _dbService.GetQueryFactory(dbConnectionString);
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
                return NotFound($"Student {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id, [FromQuery] string dbConnectionString)
    {
        try
        {
            var queryFactory = _dbService.GetQueryFactory(dbConnectionString);
            var deletedStudents = queryFactory.Query("student").Where("studentnumber", id).Delete();
            if (deletedStudents == 0)
            {
                return NotFound($"Student {id} not found");
            }
            return NoContent();
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}