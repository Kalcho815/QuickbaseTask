using Backend;
using Backend.Interfaces;
using Backend.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var config = builder.Configuration;
//also if we were to use ServiceBase, we would register HttpClient here as well

services.AddScoped<IDbManager, SqliteDbManager>();
services.AddScoped<IStatService, ConcreteStatService>();
services.AddScoped<ICountryRepository, CountryRepository>();
services.AddScoped<ICountryManager, CountryManager>();


services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();