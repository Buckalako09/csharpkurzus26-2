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

    public List<WorkoutPlan> GetAllWorkoutPlans()
    {
        return _workoutPlans
            .OrderBy(plan => plan.Name)
            .ToList();
    }
    public WorkoutPlan? GetWorkoutPlanById(Guid workoutPlanId)
    {
        return _workoutPlans
            .FirstOrDefault(plan =>
                plan.Id == workoutPlanId);
    }

    public List<Exercise> GetExercisesForWorkoutPlan(
        Guid workoutPlanId)
    {
        WorkoutPlan? workoutPlan =
            GetWorkoutPlanById(workoutPlanId);

        if (workoutPlan is null)
        {
            throw new InvalidOperationException(
                "A megadott edzésterv nem létezik.");
        }

        return workoutPlan.ExerciseIds
            .Select(exerciseId =>
                _exercises.FirstOrDefault(
                    exercise => exercise.Id == exerciseId))
            .OfType<Exercise>()
            .ToList();
    }

    public WorkoutSession CompleteWorkout(Guid workoutPlanId)
    {
        WorkoutPlan? workoutPlan =
            GetWorkoutPlanById(workoutPlanId);

        if (workoutPlan is null)
        {
            throw new InvalidOperationException(
                "A megadott edzésterv nem létezik.");
        }

        List<Exercise> exercises =
            GetExercisesForWorkoutPlan(workoutPlanId);

        if (exercises.Count == 0)
        {
            throw new InvalidOperationException(
                "Üres edzéstervet nem lehet teljesítettnek jelölni.");
        }

        WorkoutSession workoutSession = new()
        {
            Id = Guid.NewGuid(),
            WorkoutPlanId = workoutPlan.Id,
            WorkoutPlanName = workoutPlan.Name,
            CompletedAt = DateTime.Now,

            ExerciseNames = exercises
                .Select(exercise => exercise.Name)
                .ToList()
        };

        _workoutSessions.Add(workoutSession);

        return workoutSession;
    }

    public List<WorkoutSession> GetWorkoutHistory()
    {
        return _workoutSessions
            .OrderByDescending(
                session => session.CompletedAt)
            .ToList();
    }
}
