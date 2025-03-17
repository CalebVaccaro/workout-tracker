using workout_tracker.api.Databases;
using workout_tracker.api.DTOs;

namespace workout_tracker.api.Services;

public interface IUserService
{
    Task<UserDto> GetUser(string id);
    Task<UserDto> CreateUser(User user);
    Task<UserDto> UpdateUser(User user);
    Task<string> DeleteUser(string id);
}

public class UserService(UserDb db) : IUserService
{
    public async Task<UserDto> GetUser(string id)
    {
        var user = db.GetUserAsync(id);
        return await Task.FromResult(UserDto.ToUserDto(user.Result));
    }
    
    public async Task<UserDto> CreateUser(User user)
    {
        await db.CreateUserAsync(user);
        return await Task.FromResult(UserDto.ToUserDto(user));
    }

    public async Task<UserDto> UpdateUser(User user)
    {
        await db.UpdateUserAsync(user);
        return await Task.FromResult(UserDto.ToUserDto(user));
    }
    
    public async Task<string> DeleteUser(string id)
    {
        await db.DeleteUserAsync(id);
        return await Task.FromResult(id);
    }
}