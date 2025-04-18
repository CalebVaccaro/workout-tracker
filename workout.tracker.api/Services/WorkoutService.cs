using System.Text.Json;
using Newtonsoft.Json;
using workout_tracker.api.Databases;
using workout_tracker.api.DTOs;
using workout_tracker.api.Services.Extensions;

namespace workout_tracker.api.Services;

public interface IWorkoutService
{
    Task<WorkoutDto> GetWorkout(string id);
    Task<WorkoutDto> CreateWorkout(Workout workout);
    Task<string> DeleteWorkout(string id);
    Task<List<WorkoutDto>> GetUserWorkouts(string id);
    Task<string> GetUserWorkoutsThisWeek(string id);
    Task<List<WorkoutDto>> GetUserWeekSuggestions(string id);
    Task<List<WorkoutDto>> SaveWorkout(string id, List<WorkoutDto> suggestions);
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
    
    public async Task<List<WorkoutDto>> GetUserWorkouts(string id)
    {
        var workouts = db.GetWorkoutsAsync(id);
        return await Task.FromResult(workouts.Result.Select(WorkoutDto.ToWorkoutDto).ToList());
    }
    
    public async Task<string> GetUserWorkoutsThisWeek(string id)
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

        return $"{workedOutString} / {notWorkedOutString}";
    }
    
    public async Task<List<WorkoutDto>> GetUserWeekSuggestions(string id)
    {
        var muscleGroupHistory = await GetUserWorkoutsThisWeek(id);
        var finalPrompt = PromptBuilder.BuildWorkoutSuggestionPrompt(
            userWorkoutHistory: muscleGroupHistory,
            referenceDate: DateTime.Now,
            suggestionsCount: 5,
            duration: "1 hour"
        );
        
        var chatResponse = await openAiService.ChatCompletion(finalPrompt);
        var workoutSuggestions = JsonConvert.DeserializeObject<List<WorkoutSuggestion>>(chatResponse);
        var workoutDtos = workoutSuggestions?.Select(WorkoutSuggestion.ToWorkout);
        return workoutDtos?.ToList() ?? new List<WorkoutDto>();
    }

    public async Task<List<WorkoutDto>> SaveWorkout(string userId, List<WorkoutDto> workoutDtos)
    {
        var workouts = new List<WorkoutDto>();
        var workoutsToSave = workoutDtos.Where(e => e.Completed)
            .Select(WorkoutDto.ToWorkout).ToList();
        foreach (var workout in workoutsToSave)
        {
            workout.Id = Guid.NewGuid().ToString();
            workout.UserId = userId;
            await db.CreateWorkoutAsync(workout);
            workouts.Add(WorkoutDto.ToWorkoutDto(workout));
        }
        return workouts;
    }
}