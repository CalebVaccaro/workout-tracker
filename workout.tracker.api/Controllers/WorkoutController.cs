using Microsoft.AspNetCore.Mvc;
using workout_tracker.api.DTOs;
using workout_tracker.api.Services;

namespace workout_tracker.api.Controllers;

public static class WorkoutController
{
    public static void RegisterWorkoutEndpoints(this WebApplication app)
    {
        var workout = app.MapGroup("/workout");
        workout.MapPost("",CreateWorkout);
        workout.MapGet("/{id:guid}",GetWorkout);
        workout.MapGet("/user/{id:guid}",GetUserWorkouts);
        workout.MapGet("/week/{id:guid}",GetUserWorkoutsThisWeek);
        workout.MapGet("/suggestions/{id:guid}",GetWorkoutSuggestions);
    }
    
    static async Task<IResult> GetWorkout([FromRoute]string id, [FromServices]IWorkoutService workoutService)
    {
        var workout = await workoutService.GetWorkout(id);
        return TypedResults.Ok(workout);
    }
    
    static async Task<IResult> CreateWorkout([FromBody]WorkoutDto workoutDto, [FromServices]IWorkoutService workoutService)
    {
        var muscleGroup = Enum.Parse<MuscleGroup>(workoutDto.MuscleGroup);

        var workout = WorkoutDto.ToWorkout(workoutDto);

        await workoutService.CreateWorkout(workout);
        var workoutDTO = WorkoutDto.ToWorkoutDto(workout);
        
        return TypedResults.Ok(workoutDTO);
    }
    
    static async Task<IResult> GetUserWorkouts([FromRoute]string id, [FromServices]IWorkoutService workoutService)
    {
        var workouts = await workoutService.GetWorkouts(id);
        return TypedResults.Ok(workouts);
    }
    
    static async Task<IResult> GetUserWorkoutsThisWeek([FromRoute]string id, [FromServices]IWorkoutService workoutService)
    {
        var workouts = await workoutService.GetWorkoutsThisWeek(id);
        return TypedResults.Ok(workouts);
    }
    
    static async Task<IResult> GetWorkoutSuggestions([FromRoute]string id, [FromServices]IWorkoutService workoutService)
    {
        var workouts = await workoutService.GetWorkoutSuggestions(id);
        return TypedResults.Ok(workouts);
    }
}