using Microsoft.Extensions.Configuration;
using S2Search.Backend.Domain.Interfaces;
using S2Search.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Set the static Configuration property
ServiceCollectionExtension.Configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAPIServices();

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