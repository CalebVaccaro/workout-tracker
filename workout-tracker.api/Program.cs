using workout_tracker.api.Controllers;
using workout_tracker.api.Databases;
using workout_tracker.api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<UserDb>();
builder.Services.AddSingleton<WorkoutDb>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IWorkoutService, WorkoutService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()){
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.RegisterUserEndpoints();
app.RegisterWorkoutEndpoints();

app.Run();