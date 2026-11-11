using WorkoutTracker.Models;

namespace WorkoutTracker.Interfaces;

public interface IExerciseService
{
    Exercise AddExercise(
        string name,
        string muscleGroup,
        int defaultSets,
        int defaultReps);

    bool DeleteExercise(Guid exerciseId);

    List<Exercise> GetAllExercises();

    List<Exercise> SearchExercises(string searchTerm);

    List<Exercise> FilterByMuscleGroup(string muscleGroup);

    Exercise? GetExerciseById(Guid exerciseId);
}
