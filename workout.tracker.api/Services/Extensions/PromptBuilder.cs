namespace workout_tracker.api.Services.Extensions;

public static class PromptBuilder
{
    public static string BuildWorkoutSuggestionPrompt(
        string userWorkoutHistory,
        DateTime referenceDate,
        string workoutType = null,
        int suggestionsCount = 5,
        string duration = "1 hour")
    {
        var basePrompt = "You are a fitness coach. Tell me what workout routine I should do based on my workout history; muscle groups not worked this week";

        var datePrompt = $"Please only include workouts for the rest of the week as of {referenceDate:yyyy-MM-dd}.";

        var muscleGroups = string.Join(", ", Enum.GetNames<MuscleGroup>());
        var workoutTypes = string.Join(", ", Enum.GetNames<WorkoutType>());

        var constraintsPrompt = $"Workouts must be associated with the muscleGroup choices: {muscleGroups} and workoutType choices: {workoutTypes}.";

        var suggestionsPrompt = $"Number of suggestions: {suggestionsCount}.";
        var durationPrompt = $"Duration: {duration}.";
        
        var specificWorkoutType = $"workoutType: {workoutType}, if provided only exercises of that type should be included.";

        var jsonOutputPrompt = 
            "Return only valid JSON. Do NOT include ```json or any code block formatting. " +
            "Just return the raw JSON array. Do not include any explanations.\n\n" +
            "[\n" +
            "  {\n" +
            "    \"title\": \"Dumbbell Bench Press\",\n" +
            "    \"description\": \"Lie on a bench and press dumbbells upward to strengthen your chest.\",\n" +
            "    \"imageUrl\": \"https://example.com/images/benchpress.png\",\n" +
            "    \"videoUrl\": \"https://example.com/videos/benchpress.mp4\",\n" +
            "    \"equipment\": \"Dumbbells, Bench\",\n" +
            "    \"sets\": \"3\",\n" +
            "    \"reps\": \"12\",\n" +
            "    \"duration\": \"0\",\n" +
            "    \"caloriesBurned\": \"120\",\n" +
            $"    \"workoutType\": \"(Choose only one of WorkoutType) {workoutTypes}\",\n" +
            $"    \"muscleGroup\": \"(Choose only one MuscleGroup) {muscleGroups}\"\n" +
            "  }\n" +
            "]";

        return $"{basePrompt}: {userWorkoutHistory}, {datePrompt} {constraintsPrompt} {suggestionsPrompt} {durationPrompt}, {specificWorkoutType}, {jsonOutputPrompt}";
    }
}
