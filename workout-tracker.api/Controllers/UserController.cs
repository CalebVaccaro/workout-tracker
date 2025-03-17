﻿using Microsoft.AspNetCore.Mvc;
using workout_tracker.api.DTOs;
using workout_tracker.api.Services;

namespace workout_tracker.api.Controllers;

public static class UserController
{
    public static void RegisterUserEndpoints(this WebApplication app)
    {
        var user = app.MapGroup("/user");
        user.MapPost("",CreateUser);
        user.MapGet("/{id:guid}", GetUser);
    }
    
    static async Task<IResult> GetUser([FromRoute]string id, [FromServices]IUserService userService)
    {
        var user = await userService.GetUser(id);
        return TypedResults.Ok(user);
    }
    
    static async Task<IResult> CreateUser([FromBody]UserDto userDto, [FromServices]IUserService userService)
    {
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = userDto.Name
        };

        await userService.CreateUser(user);
        var userDTO = UserDto.ToUserDto(user);
        
        return TypedResults.Ok(userDTO);
    }
}