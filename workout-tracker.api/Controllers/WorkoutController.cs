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
    }
    
    static async Task<IResult> GetWorkout([FromRoute]string id, [FromServices]IWorkoutService workoutService)
    {
        var workout = await workoutService.GetWorkout(id);
        return TypedResults.Ok(workout);
    }
    
    static async Task<IResult> CreateWorkout([FromBody]WorkoutDto workoutDto, [FromServices]IWorkoutService workoutService)
    {
        var muscleGroup = Enum.Parse<MuscleGroup>(workoutDto.MuscleGroup);
        
        var workout = new Workout
        {
            Id = Guid.NewGuid().ToString(),
            UserId = workoutDto.UserId,
            MuscleGroup = muscleGroup,
            Date = workoutDto.Date
        };

        await workoutService.CreateWorkout(workout);
        var workoutDTO = WorkoutDto.ToWorkoutDto(workout);
        
        return TypedResults.Ok(workoutDTO);
    }
    
    static async Task<IResult> GetUserWorkouts([FromRoute]string id, [FromServices]IWorkoutService workoutService)
    {
        var workouts = await workoutService.GetWorkouts(id);
        return TypedResults.Ok(workouts);
    }
}