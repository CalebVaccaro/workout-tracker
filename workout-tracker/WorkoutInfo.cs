namespace workout_tracker;

using System;
using System.Collections.Generic;
using System.Linq;

public class WorkoutInfo
{
    private Dictionary<MuscleGroup, List<DateTime>> muscleGroupHistory = new()
    {
        { MuscleGroup.Chest, new List<DateTime>() },
        { MuscleGroup.Legs, new List<DateTime>() },
        { MuscleGroup.Arms, new List<DateTime>() },
        { MuscleGroup.Back, new List<DateTime>() },
        { MuscleGroup.Shoulders, new List<DateTime>() },
        { MuscleGroup.Abs, new List<DateTime>() }
    };

    public void LogWorkout(MuscleGroup muscleGroup, DateTime date)
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