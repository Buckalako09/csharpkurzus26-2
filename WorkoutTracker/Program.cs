using WorkoutTracker.Interfaces;
using WorkoutTracker.Models;
using WorkoutTracker.Services;
using WorkoutTracker.Repositories;
using WorkoutTracker.UI;

string dataFilePath = Path.Combine(
    AppContext.BaseDirectory,
    "Data",
    "workouts.json");

IDataRepository dataRepository =
    new JsonDataRepository(dataFilePath);

Console.WriteLine("=== Korábban mentett adatok ===");

try
{
    var previousData = dataRepository.Load();

    Console.WriteLine(
        $"Mentett gyakorlatok: {previousData.Exercises.Count}");

    Console.WriteLine(
        $"Mentett edzéstervek: {previousData.WorkoutPlans.Count}");

    Console.WriteLine(
        $"Mentett teljesítések: {previousData.WorkoutSessions.Count}");
}
catch (Exception ex)
{
    Console.WriteLine(
        $"Betöltési hiba: {ex.Message}");
}

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



Console.WriteLine();
Console.WriteLine("=== Edzések teljesítésének tesztje ===");

// Új edzésterv létrehozása
WorkoutPlan completedPlan =
    workoutService.CreateWorkoutPlan("Láb nap 2");

// A korábban létrehozott guggolás hozzárendelése
workoutService.AddExerciseToWorkoutPlan(
    completedPlan.Id,
    squat.Id);

// Edzéstervek listázása
Console.WriteLine();
Console.WriteLine("Edzéstervek:");

foreach (WorkoutPlan plan in workoutService.GetAllWorkoutPlans())
{
    Console.WriteLine($"- {plan.Name}");
}

// Gyakorlatok lekérdezése
Console.WriteLine();
Console.WriteLine("Láb nap 2 gyakorlatok:");

List<Exercise> planExercises =
    workoutService.GetExercisesForWorkoutPlan(
        completedPlan.Id);

foreach (Exercise exercise in planExercises)
{
    Console.WriteLine($"- {exercise.Name}");
}

// Az edzésterv teljesítése kétszer
Console.WriteLine();
Console.WriteLine("Edzések teljesítése...");

WorkoutSession firstSession =
    workoutService.CompleteWorkout(completedPlan.Id);

WorkoutSession secondSession =
    workoutService.CompleteWorkout(completedPlan.Id);

Console.WriteLine(
    $"Első edzés rögzítve: {firstSession.WorkoutPlanName}");

Console.WriteLine(
    $"Második edzés rögzítve: {secondSession.WorkoutPlanName}");

// Előzmények
Console.WriteLine();
Console.WriteLine("=== Edzéstörténet ===");

foreach (WorkoutSession session in
    workoutService.GetWorkoutHistory())
{
    Console.WriteLine(
        $"{session.WorkoutPlanName} - " +
        $"{session.CompletedAt:yyyy-MM-dd HH:mm:ss}");
}

Console.WriteLine();
Console.WriteLine(
    $"Teljesített edzések száma: " +
    $"{workoutService.GetWorkoutHistory().Count}");

Console.WriteLine(
    $"Különböző teljesítésazonosítók: " +
    $"{firstSession.Id != secondSession.Id}");



Console.WriteLine();
Console.WriteLine("=== Statisztikák ===");

StatisticsService statisticsService =
    new StatisticsService(workoutSessions);

int completedCount =
    statisticsService.GetCompletedWorkoutCount();

string? mostFrequentExercise =
    statisticsService.GetMostFrequentExercise();

Console.WriteLine(
    $"Teljesített edzések száma: {completedCount}");

Console.WriteLine(
    $"Leggyakrabban végzett gyakorlat: " +
    $"{mostFrequentExercise ?? "Nincs adat"}");



Console.WriteLine();
Console.WriteLine("=== JSON mentés és betöltés ===");

try
{
    dataRepository.Save(
        exercises,
        workoutPlans,
        workoutSessions);

    Console.WriteLine("Adatok sikeresen elmentve!");

    var loadedData = dataRepository.Load();

    Console.WriteLine(
        $"Betöltött gyakorlatok: {loadedData.Exercises.Count}");

    Console.WriteLine(
        $"Betöltött edzéstervek: {loadedData.WorkoutPlans.Count}");

    Console.WriteLine(
        $"Betöltött teljesítések: {loadedData.WorkoutSessions.Count}");

    foreach (Exercise exercise in loadedData.Exercises)
    {
        Console.WriteLine(
            $"- {exercise.Name} ({exercise.MuscleGroup})");
    }
}
catch (Exception ex)
{
    Console.WriteLine(
        $"JSON műveleti hiba: {ex.Message}");
}


Console.WriteLine();
Console.WriteLine("=== InputHelper teszt ===");

string exerciseName =
    InputHelper.ReadRequiredString(
        "Gyakorlat neve: ");

int sets =
    InputHelper.ReadPositiveInt(
        "Sorozatok száma: ");

int menuChoice =
    InputHelper.ReadIntInRange(
        "Menüpont (0-12): ",
        0,
        12);

Console.WriteLine();
Console.WriteLine("=== Megadott adatok ===");

Console.WriteLine(
    $"Gyakorlat: {exerciseName}");

Console.WriteLine(
    $"Sorozatok: {sets}");

Console.WriteLine(
    $"Kiválasztott menüpont: {menuChoice}");

InputHelper.WaitForEnter();
