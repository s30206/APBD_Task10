using APBD_Task10;
using APBD_Task10.Models.DTOs;
using APBD_Task10.Repositories;
using APBD_Task10.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Database");
builder.Services.AddDbContext<DeviceContext>(o => o.UseSqlServer(connectionString));
builder.Services.AddTransient<IDeviceRepository, DeviceRepository>();
builder.Services.AddTransient<IDeviceService, DeviceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/devices", async (IDeviceService service, CancellationToken token) =>
{
    try
    {
        var result = await service.GetAllDevices(token);
        return result != null ? Results.Ok(result) : Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/devices/{id:int}", async (int id, IDeviceService service, CancellationToken token) =>
{
    try
    {
        var result = await service.GetDeviceById(id, token);
        return result != null ? Results.Ok(result) : Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/employess", async (IDeviceService service, CancellationToken token) =>
{
    try
    {
        var result = await service.GetAllEmployees(token);
        return result != null ? Results.Ok(result) : Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/api/employees/{id:int}", async (int id, IDeviceService service, CancellationToken token) =>
{
    try
    {
        var result = await service.GetEmployeeById(id, token);
        return result != null ? Results.Ok(result) : Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapDelete("/api/devices/{id:int}", async (int id, IDeviceService service, CancellationToken token) =>
{
    try
    {
        var result = await service.DeleteDeviceById(id, token);
        return result ? Results.NoContent() : Results.NotFound();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapPost("/api/devices/", async (InsertDeviceRequestDTO request, IDeviceService service, CancellationToken token) =>
{
    try
    {
        var result = await service.AddDevice(request, token);
        return result ? Results.Created("/api/devices/", request) : Results.BadRequest();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Run();