namespace WorkoutTracker.Models;

public class WorkoutPlan
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<Guid> ExerciseIds { get; set; } = new();
}
