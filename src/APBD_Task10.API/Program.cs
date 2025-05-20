using APBD_Task10;
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

app.Run();