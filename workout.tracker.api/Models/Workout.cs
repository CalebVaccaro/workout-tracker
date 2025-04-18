namespace workout_tracker;

public class Workout
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public int Duration { get; set; }
    public int CaloriesBurned { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
    public WorkoutType WorkoutType { get; set; }
    public bool Completed { get; set; }
    public DateTime Date { get; set; }
}