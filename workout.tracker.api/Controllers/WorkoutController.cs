using Microsoft.AspNetCore.Mvc;
using workout_tracker.api.DTOs;
using workout_tracker.api.Services;

namespace workout_tracker.api.Controllers;

public static class WorkoutController
{
    public static void RegisterWorkoutEndpoints(this WebApplication app)
    {
        var workout = app.MapGroup("/workouts");
        workout.MapPost("",CreateWorkout);
        workout.MapGet("/{id:guid}",GetWorkout);
        workout.MapGet("/user/{userId:guid}",GetUserWorkouts);
        workout.MapGet("/week/{userId:guid}",GetUserWorkoutsThisWeek);
        workout.MapGet("/suggestions/{userId:guid}/{suggestionsCount}",GetUserWeekSuggestions);
        workout.MapPost("/user/{userId:guid}",CreateWorkouts);
    }
    
    static async Task<IResult> GetWorkout([FromRoute]string id, [FromServices]IWorkoutService workoutService)
    {
        var workout = await workoutService.GetWorkout(id);
        return TypedResults.Ok(workout);
    }
    
    static async Task<IResult> CreateWorkout([FromBody]WorkoutDto workoutDto, [FromServices]IWorkoutService workoutService)
    {
        var workout = WorkoutDto.ToWorkout(workoutDto);

        await workoutService.CreateWorkout(workout);
        var workoutDTO = WorkoutDto.ToWorkoutDto(workout);
        
        return TypedResults.Ok(workoutDTO);
    }
    
    static async Task<IResult> GetUserWorkouts([FromRoute]string userId, [FromServices]IWorkoutService workoutService)
    {
        var workouts = await workoutService.GetUserWorkouts(userId);
        return TypedResults.Ok(workouts);
    }
    
    static async Task<IResult> GetUserWorkoutsThisWeek([FromRoute]string userId, [FromServices]IWorkoutService workoutService)
    {
        var workouts = await workoutService.GetUserWorkoutsThisWeek(userId);
        return TypedResults.Ok(workouts);
    }
    
    static async Task<IResult> GetUserWeekSuggestions([FromRoute]string userId, [FromRoute]int suggestionsCount, [FromRoute]string workoutType, [FromServices]IWorkoutService workoutService)
    {
        var workouts = await workoutService.GetUserWeekSuggestions(userId, suggestionsCount, workoutType);
        return TypedResults.Ok(workouts);
    }
    
    static async Task<IResult> CreateWorkouts([FromRoute]string userId, [FromBody]List<WorkoutDto> workoutDtos, [FromServices]IWorkoutService workoutService)
    {
        var workouts = await workoutService.SaveWorkouts(userId, workoutDtos);
        return TypedResults.Ok(workouts);
    }
}