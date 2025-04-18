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
        workout.MapGet("/user/{id:guid}",GetUserWorkouts);
        workout.MapGet("/week/{id:guid}",GetUserWorkoutsThisWeek);
        workout.MapGet("/suggestions/{id:guid}",GetUserWeekSuggestions);
        workout.MapPost("/user/{id:guid}",CreateWorkouts);
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
        var workouts = await workoutService.GetUserWorkouts(id);
        return TypedResults.Ok(workouts);
    }
    
    static async Task<IResult> GetUserWorkoutsThisWeek([FromRoute]string id, [FromServices]IWorkoutService workoutService)
    {
        var workouts = await workoutService.GetUserWorkoutsThisWeek(id);
        return TypedResults.Ok(workouts);
    }
    
    static async Task<IResult> GetUserWeekSuggestions([FromRoute]string id, [FromServices]IWorkoutService workoutService)
    {
        var workouts = await workoutService.GetUserWeekSuggestions(id);
        return TypedResults.Ok(workouts);
    }
    
    static async Task<IResult> CreateWorkouts([FromRoute]string id, [FromBody]List<WorkoutDto> suggestions, [FromServices]IWorkoutService workoutService)
    {
        var workouts = await workoutService.SaveWorkout(id, suggestions);
        return TypedResults.Ok(workouts);
    }
}