using Microsoft.EntityFrameworkCore;
using TvMazeAPI.Repository;
using TvMazeAPI.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<RateLimiter>();
builder.Services.AddSingleton<JsonDeserializer>();
builder.Services.AddSingleton<TvShowValidator>();

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
