namespace WorkoutTracker.Models;

public record Exercise(
    Guid Id,
    string Name,
    string MuscleGroup,
    int DefaultSets,
    int DefaultReps
);
