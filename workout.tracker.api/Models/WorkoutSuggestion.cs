using workout_tracker.api.DTOs;

namespace workout_tracker;

public class WorkoutSuggestion
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Equipment { get; set; }
    public string Sets { get; set; }
    public string Reps { get; set; }
    public string Duration { get; set; }
    public string CaloriesBurned { get; set; }
    public string WorkoutType { get; set; }
    public string MuscleGroup { get; set; }

    public static WorkoutDto ToWorkout(WorkoutSuggestion suggestion)
    {
        return new WorkoutDto()
        {
            Name = suggestion.Title,
            Sets = int.Parse(suggestion.Sets),
            Reps = int.Parse(suggestion.Reps),
            Duration = int.Parse(suggestion.Duration ?? "0"),
            CaloriesBurned = int.Parse(suggestion.CaloriesBurned),
            MuscleGroup = suggestion.MuscleGroup,
            WorkoutType = suggestion.WorkoutType,
            Date = DateTime.UtcNow
        };
    }
}