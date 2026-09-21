using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using GetInLineSchool.Repositories;
using GetInLineSchool.Services;
using Microsoft.AspNetCore.Mvc;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<SchoolRepository>();
builder.Services.AddScoped<SchoolService>();
builder.Services.AddScoped<TeacherRepository>();
builder.Services.AddScoped<TeacherService>();

//catches validation errors
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
        .Where(x => x.Value!.Errors.Any())
        .SelectMany(x => x.Value!.Errors.Select(e => new Error
        {
            Key = "Validation",
            Message = e.ErrorMessage
        })).ToList();

        return new BadRequestObjectResult(ServiceResult<object>.Failure(null, errors));
    };
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
