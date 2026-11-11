
using WorkoutTracker.Interfaces;
using WorkoutTracker.Models;
using WorkoutTracker.Repositories;
using WorkoutTracker.Services;
using WorkoutTracker.UI;

string dataFilePath = Path.Combine(
    AppContext.BaseDirectory,
    "Data",
    "workouts.json");

IDataRepository dataRepository =
    new JsonDataRepository(dataFilePath);

List<Exercise> exercises;
List<WorkoutPlan> workoutPlans;
List<WorkoutSession> workoutSessions;

try
{
    (exercises, workoutPlans, workoutSessions) =
        dataRepository.Load();
}
catch (Exception ex)
{
    Console.WriteLine(
        $"Az adatok betöltése sikertelen: {ex.Message}");

    return;
}

IExerciseService exerciseService =
    new ExerciseService(exercises, workoutPlans);

IWorkoutService workoutService =
    new WorkoutService(
        exercises,
        workoutPlans,
        workoutSessions);

StatisticsService statisticsService =
    new StatisticsService(workoutSessions);

ConsoleMenu consoleMenu = new(
    exerciseService,
    workoutService,
    statisticsService,
    dataRepository);

consoleMenu.Run();
