
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


Console.WriteLine();
Console.WriteLine("=== Edzésterv létrehozása ===");

List<WorkoutSession> workoutSessions = new();

IWorkoutService workoutService = new WorkoutService(
    exercises,
    workoutPlans,
    workoutSessions);

WorkoutPlan legDay = workoutService.CreateWorkoutPlan("Láb nap");

Console.WriteLine($"Létrehozva: {legDay.Name}");

Console.WriteLine();
Console.WriteLine("=== Gyakorlat hozzárendelése ===");

workoutService.AddExerciseToWorkoutPlan(
    legDay.Id,
    squat.Id);

Console.WriteLine(
    $"Edzéstervben lévő gyakorlatok: {legDay.ExerciseIds.Count}");

Console.WriteLine();
Console.WriteLine("=== Nem létező gyakorlat ellenőrzése ===");

try
{
    workoutService.AddExerciseToWorkoutPlan(
        legDay.Id,
        Guid.NewGuid());
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Helyesen elutasítva: {ex.Message}");
}

Console.WriteLine();
Console.WriteLine("=== Edzésterv törlése ===");

bool planDeleted = workoutService.DeleteWorkoutPlan(legDay.Id);

Console.WriteLine($"Törlés sikeres: {planDeleted}");

Console.WriteLine(
    $"Megmaradt edzéstervek száma: {workoutPlans.Count}");
