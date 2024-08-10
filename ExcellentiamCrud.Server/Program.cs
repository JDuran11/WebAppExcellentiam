using ExcellentiamCrud.Server.Models;
using Microsoft.EntityFrameworkCore;
using ExcellentiamCrud.Server.Ports;
using ExcellentiamCrud.Server.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICursoRepositoryPort, CursoRepositoryAdapter>();
builder.Services.AddScoped<IEstudianteRepositoryPort, EstudianteRepositoryAdapter>();

builder.Services.AddDbContext<ExcellentiamTestDbContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("conexionSQL"));
}
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("newPolicy", app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("newPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
