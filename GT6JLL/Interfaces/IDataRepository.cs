using WorkoutTracker.Models;

namespace WorkoutTracker.Interfaces;

public interface IDataRepository
{
    (
        List<Exercise> Exercises,
        List<WorkoutPlan> WorkoutPlans,
        List<WorkoutSession> WorkoutSessions
    ) Load();

    void Save(
        List<Exercise> exercises,
        List<WorkoutPlan> workoutPlans,
        List<WorkoutSession> workoutSessions);
}
