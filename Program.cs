using EmpHeirarchy.BLL.Services;
using EmpHeirarchy.DAL.Models;
using EmpHeirarchy.REPO.Repos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TempoDbContext>(options =>
     options.UseSqlServer(builder.Configuration.GetConnectionString("connectionString")));

builder.Services.AddScoped<TempoDbContext>();
builder.Services.AddScoped<EmployeeRepo>();
builder.Services.AddScoped<EmployeeServices>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
