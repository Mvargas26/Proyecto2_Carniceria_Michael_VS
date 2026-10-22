using Proyecto1_Carniceria_MichaelVS.Datos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Creamos una sola instancia de DatosEnMemoria para toda la app,así los datos no se pierden entre peticiones.
builder.Services.AddSingleton<DatosEnMemoria>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
