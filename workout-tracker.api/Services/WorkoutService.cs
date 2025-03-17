using workout_tracker.api.Databases;
using workout_tracker.api.DTOs;

namespace workout_tracker.api.Services;

public interface IWorkoutService
{
    Task<WorkoutDto> GetWorkout(string id);
    Task<WorkoutDto> CreateWorkout(Workout workout);
    Task<string> DeleteWorkout(string id);
    Task<List<WorkoutDto>> GetWorkouts(string id);
}

public class WorkoutService(WorkoutDb db) : IWorkoutService
{
    public async Task<WorkoutDto> GetWorkout(string id)
    {
        var user = db.GetWorkoutAsync(id);
        return await Task.FromResult(WorkoutDto.ToWorkoutDto(user.Result));
    }

    public async Task<WorkoutDto> CreateWorkout(Workout workout)
    {
        await db.CreateWorkoutAsync(workout);
        return await Task.FromResult(WorkoutDto.ToWorkoutDto(workout));
    }

    public async Task<string> DeleteWorkout(string id)
    {
        await db.DeleteWorkoutAsync(id);
        return await Task.FromResult(id);
    }
    
    public async Task<List<WorkoutDto>> GetWorkouts(string id)
    {
        var workouts = db.GetWorkoutsAsync(id);
        return await Task.FromResult(workouts.Result.Select(WorkoutDto.ToWorkoutDto).ToList());
    }
}