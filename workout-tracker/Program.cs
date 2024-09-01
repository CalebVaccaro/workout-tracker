using workout_tracker;

// Configuration
var apiKey = AppConfig.GetOpenAPIKey();
var prompt = "Tell me what workout routine I should do based on my workout history for the rest of the week.";
            
// Workout Information
var workoutInfo = new WorkoutInfo();
workoutInfo.LogWorkout("Chest", DateTime.Now.AddDays(-2));
workoutInfo.LogWorkout("Back", DateTime.Now.AddDays(-1));
workoutInfo.LogWorkout("Legs", DateTime.Now);
            
var muscleGroupsNotWorkedThisWeek = workoutInfo.GetMuscleGroupsNotWorkedThisWeek();
Console.WriteLine($"Muscle groups not worked this week: {muscleGroupsNotWorkedThisWeek}");

// LLM Response for Workout Routine
var response = await OpenAIResponse.GetOpenAIResponseAsync(apiKey, prompt + " " + muscleGroupsNotWorkedThisWeek);
Console.WriteLine(response);