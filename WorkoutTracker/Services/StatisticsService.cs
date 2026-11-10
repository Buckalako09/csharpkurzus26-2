using WorkoutTracker.Models;

namespace WorkoutTracker.Services;

public class StatisticsService
{
    private readonly List<WorkoutSession> _workoutSessions;

    public StatisticsService(
        List<WorkoutSession> workoutSessions)
    {
        _workoutSessions = workoutSessions;
    }

    public int GetCompletedWorkoutCount()
    {
        return _workoutSessions.Count();
    }

    public string? GetMostFrequentExercise()
    {
        return _workoutSessions
            .SelectMany(session => session.ExerciseNames)
            .Where(exerciseName =>
                !string.IsNullOrWhiteSpace(exerciseName))
            .GroupBy(
                exerciseName => exerciseName,
                StringComparer.OrdinalIgnoreCase)
            .OrderByDescending(group => group.Count())
            .ThenBy(group => group.Key)
            .Select(group => group.Key)
            .FirstOrDefault();
    }
}
