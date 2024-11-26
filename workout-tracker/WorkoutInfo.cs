namespace workout_tracker;

using System;
using System.Collections.Generic;
using System.Linq;

public class WorkoutInfo
{
    private Dictionary<string, List<DateTime>> muscleGroupHistory = new()
    {
        { "Chest", new List<DateTime>() },
        { "Back", new List<DateTime>() },
        { "Legs", new List<DateTime>() },
        { "Arms", new List<DateTime>() },
        { "Shoulders", new List<DateTime>() },
        { "Abs", new List<DateTime>() }
    };

    public void LogWorkout(string muscleGroup, DateTime date)
    {
        if (muscleGroupHistory.ContainsKey(muscleGroup))
        {
            muscleGroupHistory[muscleGroup].Add(date);
        }
    }

    public string GetMuscleGroupsNotWorkedThisWeek()
    {
        DateTime startOfWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
        var muscleGroupsNotWorkedThisWeek = muscleGroupHistory
            .Where(mg => mg.Value.All(date => date < startOfWeek))
            .Select(mg => mg.Key)
            .ToList();

        return string.Join(", ", muscleGroupsNotWorkedThisWeek);
    }
}