using workout_tracker.api.Databases;
using workout_tracker.api.DTOs;

namespace workout_tracker.api.Services;

public interface IWorkoutService
{
    Task<WorkoutDto> GetWorkout(string id);
    Task<WorkoutDto> CreateWorkout(Workout workout);
    Task<string> DeleteWorkout(string id);
    Task<List<WorkoutDto>> GetWorkouts(string id);
    Task<string> GetWorkoutsThisWeek(string id);
    Task<string> GetWorkoutSuggestions(string id);
}

public class WorkoutService(WorkoutDb db, IOpenAIService openAiService) : IWorkoutService
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
    
    public async Task<string> GetWorkoutsThisWeek(string id)
    {
        var workouts = await db.GetWorkoutsAsync(id);
        var startOfWeek = DateTime.UtcNow.AddDays(-(int)DateTime.UtcNow.DayOfWeek);
        var workoutsThisWeek = workouts.Where(w => w.Date >= startOfWeek).ToList();

        var muscleGroupHistory = new Dictionary<MuscleGroup, List<DateTime>>();
        foreach (var workout in workoutsThisWeek)
        {
            if (!muscleGroupHistory.ContainsKey(workout.MuscleGroup))
            {
                muscleGroupHistory[workout.MuscleGroup] = new List<DateTime>();
            }
            muscleGroupHistory[workout.MuscleGroup].Add(workout.Date);
        }

        var allMuscleGroups = Enum.GetValues(typeof(MuscleGroup)).Cast<MuscleGroup>();
        var muscleGroupsWorkedThisWeek = muscleGroupHistory.Keys.ToList();
        var muscleGroupsNotWorkedThisWeek = allMuscleGroups
            .Where(mg => !muscleGroupHistory.ContainsKey(mg))
            .ToList();

        var workedOutString = $"Muscle groups worked out this week: {string.Join(", ", muscleGroupsWorkedThisWeek)}";
        var notWorkedOutString = $"Muscle groups not worked out this week: {string.Join(", ", muscleGroupsNotWorkedThisWeek)}";

        return $"{workedOutString},{notWorkedOutString}";
    }
    
    public async Task<string> GetWorkoutSuggestions(string id)
    {
        var prompt = "Tell me what workout routine I should do based on my workout history; muscle groups not worked this week";
        var muscleGroupHistory = await GetWorkoutsThisWeek(id);
        var date = DateTime.Now;
        var dateToPrompt = $"please only include workouts for the rest of the week as of {date:yyyy-MM-dd}";
        var finalPrompt = $"{prompt}: {muscleGroupHistory}, {dateToPrompt}";
        return await openAiService.GetOpenAIResponseAsync(finalPrompt);
    }
}