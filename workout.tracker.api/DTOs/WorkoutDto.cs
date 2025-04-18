namespace workout_tracker.api.DTOs;

public class WorkoutDto
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string MuscleGroup { get; set; }
    public string WorkoutType { get; set; }
    public string Name { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public string Duration { get; set; }
    public int CaloriesBurned { get; set; }
    public bool Completed { get; set; } = false;
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public static WorkoutDto ToWorkoutDto(Workout workout)
    {
        return new WorkoutDto()
        {
            Id = workout.Id,
            UserId = workout.UserId,
            MuscleGroup = workout.MuscleGroup.ToString(),
            WorkoutType = workout.WorkoutType.ToString(),
            Date = workout.Date,
            Name = workout.Name,
            Sets = workout.Sets,
            Reps = workout.Reps,
            Duration = workout.Duration,
            CaloriesBurned = workout.CaloriesBurned,
        };
    }
    
    public static Workout ToWorkout(WorkoutDto workoutDto)
    {
        var muscleGroup = Enum.Parse<MuscleGroup>(workoutDto.MuscleGroup);
        var workoutType = Enum.Parse<WorkoutType>(workoutDto.WorkoutType);
        
        return new Workout()
        {
            Id = workoutDto.Id,
            UserId = workoutDto.UserId,
            MuscleGroup = muscleGroup,
            WorkoutType = workoutType,
            Date = workoutDto.Date,
            Name = workoutDto.Name,
            Sets = workoutDto.Sets,
            Reps = workoutDto.Reps,
            Duration = workoutDto.Duration,
            CaloriesBurned = workoutDto.CaloriesBurned,
        };
    }
}