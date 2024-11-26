using workout_tracker;

// Configuration
var apiKey = AppConfig.GetOpenAIAPIKey();
var prompt = "Tell me what workout routine I should do based on my workout history";

var date = DateTime.Now;
var dateToPrompt = $"please only include workouts for the rest of the week as of {date:yyyy-MM-dd}";
            
// Workout Information
var workoutInfo = new WorkoutInfo();
workoutInfo.LogWorkout("Chest", DateTime.Now.AddDays(-2));
workoutInfo.LogWorkout("Back", DateTime.Now.AddDays(-1));
workoutInfo.LogWorkout("Legs", DateTime.Now);
            
var muscleGroupsNotWorkedThisWeek = workoutInfo.GetMuscleGroupsNotWorkedThisWeek();
Console.WriteLine($"Muscle groups not worked this week: {muscleGroupsNotWorkedThisWeek}");

// LLM Response for Workout Routine
var finalPrompt = $"{prompt}: {muscleGroupsNotWorkedThisWeek}, {dateToPrompt}";
var response = await OpenAIResponse.GetOpenAIResponseAsync(apiKey, finalPrompt);
Console.WriteLine(response);