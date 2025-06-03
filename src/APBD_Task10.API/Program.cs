using APBD_Task10;
using APBD_Task10.Models.DTOs;
using APBD_Task10.Repositories;
using APBD_Task10.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel;
using Microsoft.IdentityModel.JsonWebTokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// var jwtBuildData = builder.Configuration.GetSection("Jwt");

var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<DeviceContext>(o => o.UseSqlServer(connectionString));
builder.Services.AddTransient<IDeviceRepository, DeviceRepository>();
builder.Services.AddTransient<IDeviceService, DeviceService>();
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();