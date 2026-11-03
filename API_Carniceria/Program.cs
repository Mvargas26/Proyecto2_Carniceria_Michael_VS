using API_Carniceria.Negocios.ClienteNegocios;
using API_Carniceria.Negocios.EmpleadoNegocios;
using API_Carniceria.Negocios.ProductoNegocios;
using API_Carniceria.Repositories.Cliente_Repository;
using API_Carniceria.Repositories.Empleado_Repository;
using API_Carniceria.Repositories.Producto_Repository;
using API_Carniceria.Repositories.Proveedor_Repository;
using API_Carniceria.Services.Proveedor_Service;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Registramos los repositorios y Negocios en el contenedor de dependencias.
builder.Services.AddSingleton<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<IProveedorNegocios, ProveedorNegocios>();
builder.Services.AddSingleton<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoNegocios, ProductoNegocios>();
builder.Services.AddSingleton<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteNegocios, ClienteNegocios>();
builder.Services.AddSingleton<IEmpleadoRepository, EmpleadoRepository>();
builder.Services.AddScoped<IEmpleadoNegocios, EmpleadoNegocios>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
