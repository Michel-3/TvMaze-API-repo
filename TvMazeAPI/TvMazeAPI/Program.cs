using Microsoft.EntityFrameworkCore;
using TvMazeAPI.Core.Services.Implementations;
using TvMazeAPI.Core.Services.Interfaces;
using TvMazeAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<IRateLimiter, RateLimiter>();
builder.Services.AddSingleton<IJsonDeserializer, JsonDeserializer>();
builder.Services.AddSingleton<ITvShowValidator, TvShowValidator>();
builder.Services.AddScoped<IDataCheckService, DataCheckService>();
builder.Services.AddScoped<IActorCalculator, ActorCalculator>();
builder.Services.AddScoped<ActorRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TvMazeDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("TvMazeDb")));

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
