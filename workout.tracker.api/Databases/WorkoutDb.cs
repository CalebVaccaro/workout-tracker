﻿using System.Data.SQLite;
using Dapper;

namespace workout_tracker.api.Databases;

public class WorkoutDb
{
    private readonly string _connectionString = "Data Source=workout.db";
    
    public WorkoutDb()
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS Workouts (
                Id TEXT PRIMARY KEY, 
                UserId TEXT, 
                MuscleGroup TEXT, 
                Date TEXT
            )");
    }
    
    public async Task<Workout> GetWorkoutAsync(string id)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();
        var workout = await connection.QueryFirstOrDefaultAsync<Workout>("SELECT * FROM Workouts WHERE Id = @Id", new { Id = id });
        return workout;
    }

    public async Task CreateWorkoutAsync(Workout workout)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("INSERT INTO Workouts (Id, UserId, MuscleGroup, Date) VALUES (@Id, @UserId, @MuscleGroup, @Date)", workout);
    }
    
    public async Task DeleteWorkoutAsync(string id)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("DELETE FROM Workouts WHERE Id = @Id", new { Id = id });
    }

    public async Task<IEnumerable<Workout>> GetWorkoutsAsync(string id)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();
        var workouts = await connection.QueryAsync<Workout>("SELECT * FROM Workouts WHERE UserId = @Id", new { Id = id });
        return workouts;
    }
}