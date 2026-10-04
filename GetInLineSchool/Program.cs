using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using GetInLineSchool.Repositories;
using GetInLineSchool.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

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

//JWT
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
var key = Encoding.ASCII.GetBytes(jwtSecret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),//checks original and used secret key
            ValidIssuer = "GetInLineSchool",
            ValidAudience = "front-end",
            RoleClaimType = "role"
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", p => p.RequireClaim(ClaimTypes.Role, "1"));  
    options.AddPolicy("Director", p => p.RequireClaim(ClaimTypes.Role, "2"));
    options.AddPolicy("Accounter", p => p.RequireClaim(ClaimTypes.Role, "3"));
    options.AddPolicy("Teacher", p => p.RequireClaim(ClaimTypes.Role, "4"));
    options.AddPolicy("Student", p => p.RequireClaim(ClaimTypes.Role, "5"));
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
