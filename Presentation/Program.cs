using Application.Repositories;
using Application.UseCase;
using FlappyBackend.Infrastructure.Persistence;
using Infrastructure.Adapters;
using Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Presentation.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
    );
});
builder.Services.AddDbContext<FlappyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FlappyBackend API", Version = "v1" });
});
builder.Services.AddSignalR();

builder.Services.AddScoped<IScoreRepository, ScoreAdapter>();
builder.Services.AddScoped<IAliasRepository, AliasAdapter>();
builder.Services.AddScoped<ISessionRepository, SessionAdapter>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ScoreUseCase>();
builder.Services.AddScoped<AliasUseCase>();
builder.Services.AddScoped<SessionUseCase>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseProblemDetails();
app.UseHttpsRedirection();

app.UseCors();
app.UseAuthorization();

app.MapControllers();
app.MapHub<RankingHub>("/rankingHub");

app.Run();