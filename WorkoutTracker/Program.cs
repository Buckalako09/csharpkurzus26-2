
using WorkoutTracker.Interfaces;
using WorkoutTracker.Models;
using WorkoutTracker.Services;

List<Exercise> exercises = new();
List<WorkoutPlan> workoutPlans = new();

IExerciseService exerciseService =
    new ExerciseService(exercises, workoutPlans);

// Két gyakorlat létrehozása

Exercise benchPress = exerciseService.AddExercise(
    "Fekvenyomás", "Mell", 4, 8);

Exercise squat = exerciseService.AddExercise(
    "Guggolás", "Láb", 3, 10);

// Gyakorlatok kiírása

Console.WriteLine("Gyakorlatok:");

foreach (Exercise exercise in exercises)
{
    Console.WriteLine(
        $"{exercise.Name} - {exercise.MuscleGroup} - " +
        $"{exercise.DefaultSets}x{exercise.DefaultReps}");
}

// Teszt edzésterv létrehozása

WorkoutPlan workoutPlan = new()
{
    Id = Guid.NewGuid(),
    Name = "Mell nap"
};

workoutPlan.ExerciseIds.Add(benchPress.Id);
workoutPlans.Add(workoutPlan);

// Gyakorlat törlése

Console.WriteLine();
Console.WriteLine("Fekvenyomás törlése...");

bool deleted = exerciseService.DeleteExercise(
    benchPress.Id);

Console.WriteLine($"Törlés sikeres: {deleted}");

Console.WriteLine(
    $"Megmaradt gyakorlatok száma: {exercises.Count}");

Console.WriteLine(
    $"Edzéstervben maradt gyakorlatok: " +
    $"{workoutPlan.ExerciseIds.Count}");



Console.WriteLine();
Console.WriteLine("=== Gyakorlatok listázása ===");

foreach (Exercise exercise in exerciseService.GetAllExercises())
{
    Console.WriteLine(exercise.Name);
}

Console.WriteLine();
Console.WriteLine("=== Keresés: gug ===");

foreach (Exercise exercise in exerciseService.SearchExercises("gug"))
{
    Console.WriteLine(exercise.Name);
}

Console.WriteLine();
Console.WriteLine("=== Szűrés: LÁB ===");

foreach (Exercise exercise in exerciseService.FilterByMuscleGroup("LÁB"))
{
    Console.WriteLine(exercise.Name);
}
