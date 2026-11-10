using WorkoutTracker.Interfaces;
using WorkoutTracker.Models;

namespace WorkoutTracker.Services;

public class WorkoutService : IWorkoutService
{
    private readonly List<Exercise> _exercises;
    private readonly List<WorkoutPlan> _workoutPlans;
    private readonly List<WorkoutSession> _workoutSessions;

    public WorkoutService(
        List<Exercise> exercises,
        List<WorkoutPlan> workoutPlans,
        List<WorkoutSession> workoutSessions)
    {
        _exercises = exercises;
        _workoutPlans = workoutPlans;
        _workoutSessions = workoutSessions;
    }

    public WorkoutPlan CreateWorkoutPlan(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Az edzésterv neve nem lehet üres.");
        }

        bool alreadyExists = _workoutPlans.Any(plan =>
            plan.Name.Equals(
                name.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (alreadyExists)
        {
            throw new InvalidOperationException(
                "Már létezik ilyen nevű edzésterv.");
        }

        WorkoutPlan workoutPlan = new()
        {
            Id = Guid.NewGuid(),
            Name = name.Trim()
        };

        _workoutPlans.Add(workoutPlan);

        return workoutPlan;
    }

    public bool DeleteWorkoutPlan(Guid workoutPlanId)
    {
        WorkoutPlan? workoutPlan = _workoutPlans
            .FirstOrDefault(plan =>
                plan.Id == workoutPlanId);

        if (workoutPlan is null)
        {
            return false;
        }

        _workoutPlans.Remove(workoutPlan);

        return true;
    }

    public void AddExerciseToWorkoutPlan(
        Guid workoutPlanId,
        Guid exerciseId)
    {
        WorkoutPlan? workoutPlan = _workoutPlans
            .FirstOrDefault(plan =>
                plan.Id == workoutPlanId);

        if (workoutPlan is null)
        {
            throw new InvalidOperationException(
                "A megadott edzésterv nem létezik.");
        }

        Exercise? exercise = _exercises
            .FirstOrDefault(item =>
                item.Id == exerciseId);

        if (exercise is null)
        {
            throw new InvalidOperationException(
                "A megadott gyakorlat nem létezik.");
        }

        if (workoutPlan.ExerciseIds.Contains(exerciseId))
        {
            throw new InvalidOperationException(
                "Ez a gyakorlat már szerepel az edzéstervben.");
        }

        workoutPlan.ExerciseIds.Add(exerciseId);
    }

    // A következő fejlesztési adagban készülnek el.

    public List<WorkoutPlan> GetAllWorkoutPlans()
    {
        throw new NotImplementedException();
    }

    public WorkoutPlan? GetWorkoutPlanById(Guid workoutPlanId)
    {
        throw new NotImplementedException();
    }

    public List<Exercise> GetExercisesForWorkoutPlan(Guid workoutPlanId)
    {
        throw new NotImplementedException();
    }

    public WorkoutSession CompleteWorkout(Guid workoutPlanId)
    {
        throw new NotImplementedException();
    }

    public List<WorkoutSession> GetWorkoutHistory()
    {
        throw new NotImplementedException();
    }
}
