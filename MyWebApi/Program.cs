using Microsoft.AspNetCore.Mvc;
using MyWebApi.Middlewares;
using MyWebApi.Repositories;
using MyWebApi.Repositories.Abstractions;
using MyWebApi.Repositories.Business;
using MyWebApi.Services.Abstractions;
using MyWebApi.Services.Business;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errorMessage = context.ModelState
            .Where(entry => entry.Value?.Errors.Count > 0)
            .SelectMany(entry => entry.Value!.Errors)
            .Select(error => error.ErrorMessage)
            .FirstOrDefault() ?? "Invalid request.";

        return new BadRequestObjectResult(new { error = errorMessage });
    };
});

builder.Services.AddScoped<IDbService, DbService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();