
using WorkoutTracker.Interfaces;
using WorkoutTracker.Models;

namespace WorkoutTracker.Services;

public class ExerciseService : IExerciseService
{
    private readonly List<Exercise> _exercises;
    private readonly List<WorkoutPlan> _workoutPlans;

    public ExerciseService(
        List<Exercise> exercises,
        List<WorkoutPlan> workoutPlans)
    {
        _exercises = exercises;
        _workoutPlans = workoutPlans;
    }

    public Exercise AddExercise(
        string name,
        string muscleGroup,
        int defaultSets,
        int defaultReps)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "A gyakorlat neve nem lehet üres.");
        }

        if (string.IsNullOrWhiteSpace(muscleGroup))
        {
            throw new ArgumentException(
                "Az izomcsoport nem lehet üres.");
        }

        if (defaultSets <= 0 || defaultReps <= 0)
        {
            throw new ArgumentException(
                "A sorozat- és ismétlésszámnak pozitívnak kell lennie.");
        }

        bool alreadyExists = _exercises.Any(exercise =>
            exercise.Name.Equals(
                name.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (alreadyExists)
        {
            throw new InvalidOperationException(
                "Már létezik ilyen nevű gyakorlat.");
        }

        Exercise exercise = new(
            Guid.NewGuid(),
            name.Trim(),
            muscleGroup.Trim(),
            defaultSets,
            defaultReps);

        _exercises.Add(exercise);

        return exercise;
    }

    public bool DeleteExercise(Guid exerciseId)
    {
        Exercise? exercise = GetExerciseById(exerciseId);

        if (exercise is null)
        {
            return false;
        }

        _exercises.Remove(exercise);

        foreach (WorkoutPlan workoutPlan in _workoutPlans)
        {
            workoutPlan.ExerciseIds
                .RemoveAll(id => id == exerciseId);
        }

        return true;
    }

    public List<Exercise> GetAllExercises()
    {
        return _exercises
            .OrderBy(exercise => exercise.Name)
            .ToList();
    }

    public Exercise? GetExerciseById(Guid exerciseId)
    {
        return _exercises.FirstOrDefault(
            exercise => exercise.Id == exerciseId);
    }

    public List<Exercise> SearchExercises(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return GetAllExercises();
        }

        string normalizedTerm = searchTerm.Trim();

        return _exercises
            .Where(exercise =>
                exercise.Name.Contains(
                    normalizedTerm,
                    StringComparison.OrdinalIgnoreCase)
                ||
                exercise.MuscleGroup.Contains(
                    normalizedTerm,
                    StringComparison.OrdinalIgnoreCase))
            .OrderBy(exercise => exercise.Name)
            .ToList();
    }

    public List<Exercise> FilterByMuscleGroup(string muscleGroup)
    {
        if (string.IsNullOrWhiteSpace(muscleGroup))
        {
            return new List<Exercise>();
        }

        string normalizedMuscleGroup = muscleGroup.Trim();

        return _exercises
            .Where(exercise =>
                exercise.MuscleGroup.Equals(
                    normalizedMuscleGroup,
                    StringComparison.OrdinalIgnoreCase))
            .OrderBy(exercise => exercise.Name)
            .ToList();
    }
}
