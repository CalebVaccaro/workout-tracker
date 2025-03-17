namespace workout_tracker;

public class Workout
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
    public DateTime Date { get; set; }
}