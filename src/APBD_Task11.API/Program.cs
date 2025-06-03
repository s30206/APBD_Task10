using System.Text;
using APBD_Task10;
using APBD_Task10.Models.DTOs;
using APBD_Task10.Models.Helpers;
using APBD_Task10.Repositories;
using APBD_Task10.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtBuildData = builder.Configuration.GetSection("Jwt");

builder.Services.Configure<JwtOptions>(jwtBuildData);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtBuildData["Issuer"],
        ValidAudience = jwtBuildData["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtBuildData["Key"])),
        ClockSkew = TimeSpan.FromMinutes(int.Parse(jwtBuildData["ValidInMinutes"]))
    };
});
builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<DeviceContext>(o => o.UseSqlServer(connectionString));
builder.Services.AddTransient<IDeviceRepository, DeviceRepository>();
builder.Services.AddTransient<IDeviceService, DeviceService>();
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ITokenService>(_ => new TokenService(jwtBuildData.Get<JwtOptions>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();