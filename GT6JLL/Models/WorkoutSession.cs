namespace WorkoutTracker.Models;

public class WorkoutSession
{
    public Guid Id { get; set; }

    public Guid WorkoutPlanId { get; set; }

    public string WorkoutPlanName { get; set; } = string.Empty;

    public DateTime CompletedAt { get; set; }

    public List<string> ExerciseNames { get; set; } = new();
}
