using System.Data.SQLite;
using Dapper;

namespace workout_tracker.api.Databases;

public class UserDb
{
    private readonly string _connectionString = "Data Source=workout.db";
    
    public UserDb()
    {
        using var connection = new SQLiteConnection(_connectionString);
        connection.Execute("CREATE TABLE IF NOT EXISTS Users (Id TEXT PRIMARY KEY, Name TEXT)");
    }
    
    public async Task<User> GetUserAsync(string id)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();
        var user = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = id });
        return user;
    }

    public async Task CreateUserAsync(User user)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("INSERT INTO Users (Id, Name) VALUES (@Id, @Name)", user);
    }
    
    public async Task UpdateUserAsync(User user)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("UPDATE Users SET Name = @Name WHERE Id = @Id", user);
    }
    
    public async Task DeleteUserAsync(string id)
    {
        using var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync();
        await connection.ExecuteAsync("DELETE FROM Users WHERE Id = @Id", new { Id = id });
    }
}