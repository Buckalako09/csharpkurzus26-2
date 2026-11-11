using WorkoutTracker.Models;

namespace WorkoutTracker.Interfaces;

public interface IWorkoutService
{
    WorkoutPlan CreateWorkoutPlan(string name);

    bool DeleteWorkoutPlan(Guid workoutPlanId);

    void AddExerciseToWorkoutPlan(
        Guid workoutPlanId,
        Guid exerciseId);

    List<WorkoutPlan> GetAllWorkoutPlans();

    WorkoutPlan? GetWorkoutPlanById(Guid workoutPlanId);

    List<Exercise> GetExercisesForWorkoutPlan(
        Guid workoutPlanId);

    WorkoutSession CompleteWorkout(Guid workoutPlanId);

    List<WorkoutSession> GetWorkoutHistory();
}
