using System.Text.Json;

using WorkoutTracker.Interfaces;
using WorkoutTracker.Models;

namespace WorkoutTracker.Repositories;

public class JsonDataRepository : IDataRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions;

    public JsonDataRepository(string filePath)
    {
        _filePath = filePath;

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };
    }

    public (
        List<Exercise> Exercises,
        List<WorkoutPlan> WorkoutPlans,
        List<WorkoutSession> WorkoutSessions
    ) Load()
    {
        if (!File.Exists(_filePath))
        {
            return (
                new List<Exercise>(),
                new List<WorkoutPlan>(),
                new List<WorkoutSession>());
        }

        string json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return (
                new List<Exercise>(),
                new List<WorkoutPlan>(),
                new List<WorkoutSession>());
        }

        using JsonDocument document =
            JsonDocument.Parse(json);

        JsonElement root = document.RootElement;

        List<Exercise> exercises =
            DeserializeList<Exercise>(root, "Exercises");

        List<WorkoutPlan> workoutPlans =
            DeserializeList<WorkoutPlan>(root, "WorkoutPlans");

        List<WorkoutSession> workoutSessions =
            DeserializeList<WorkoutSession>(root, "WorkoutSessions");

        return (
            exercises,
            workoutPlans,
            workoutSessions);
    }

    public void Save(
        List<Exercise> exercises,
        List<WorkoutPlan> workoutPlans,
        List<WorkoutSession> workoutSessions)
    {
        string? directoryPath =
            Path.GetDirectoryName(_filePath);

        if (!string.IsNullOrWhiteSpace(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        object data = new
        {
            Exercises = exercises,
            WorkoutPlans = workoutPlans,
            WorkoutSessions = workoutSessions
        };

        string json = JsonSerializer.Serialize(
            data,
            _jsonOptions);

        File.WriteAllText(_filePath, json);
    }

    private List<T> DeserializeList<T>(
        JsonElement root,
        string propertyName)
    {
        if (!root.TryGetProperty(
                propertyName,
                out JsonElement property))
        {
            return new List<T>();
        }

        return JsonSerializer.Deserialize<List<T>>(
                   property.GetRawText(),
                   _jsonOptions)
               ?? new List<T>();
    }
}
