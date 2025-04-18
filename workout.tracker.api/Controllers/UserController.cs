using Microsoft.AspNetCore.Mvc;
using workout_tracker.api.DTOs;
using workout_tracker.api.Services;

namespace workout_tracker.api.Controllers;

public static class UserController
{
    public static void RegisterUserEndpoints(this WebApplication app)
    {
        var user = app.MapGroup("/users");
        user.MapGet("", GetUsers);
        user.MapGet("/{id:guid}", GetUser);
        user.MapPost("", CreateUser);
    }
    
    static async Task<IResult> GetUsers([FromServices]IUserService userService)
    {
        var user = await userService.GetUsers();
        return TypedResults.Ok(user);
    }
    
    static async Task<IResult> GetUser([FromRoute]string id, [FromServices]IUserService userService)
    {
        var user = await userService.GetUser(id);
        return TypedResults.Ok(user);
    }
    
    static async Task<IResult> CreateUser([FromBody]UserDto userDto, [FromServices]IUserService userService)
    {
        var user = UserDto.ToUser(userDto);

        await userService.CreateUser(user);
        var userDTO = UserDto.ToUserDto(user);
        
        return TypedResults.Ok(userDTO);
    }
}