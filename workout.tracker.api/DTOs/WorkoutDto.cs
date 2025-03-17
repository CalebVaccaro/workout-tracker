namespace workout_tracker.api.DTOs;

public class WorkoutDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string MuscleGroup { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public static WorkoutDto ToWorkoutDto(Workout workout)
    {
        return new WorkoutDto()
        {
            Id = workout.Id,
            UserId = workout.UserId,
            MuscleGroup = workout.MuscleGroup.ToString(),
            Date = workout.Date
        };
    }
    
    public static Workout ToWorkout(WorkoutDto workoutDto)
    {
        var muscleGroup = Enum.Parse<MuscleGroup>(workoutDto.MuscleGroup);
        
        return new Workout()
        {
            Id = workoutDto.Id,
            UserId = workoutDto.UserId,
            MuscleGroup = muscleGroup,
            Date = workoutDto.Date
        };
    }
}