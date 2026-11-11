using WorkoutTracker.Interfaces;
using WorkoutTracker.Models;
using WorkoutTracker.Services;

namespace WorkoutTracker.UI;

public class ConsoleMenu
{
    private readonly IExerciseService _exerciseService;
    private readonly IWorkoutService _workoutService;
    private readonly StatisticsService _statisticsService;
    private readonly IDataRepository _dataRepository;

    public ConsoleMenu(
        IExerciseService exerciseService,
        IWorkoutService workoutService,
        StatisticsService statisticsService,
        IDataRepository dataRepository)
    {
        _exerciseService = exerciseService;
        _workoutService = workoutService;
        _statisticsService = statisticsService;
        _dataRepository = dataRepository;
    }

    public void Run()
    {
        while (true)
        {
            PrintMainMenu();

            int choice = InputHelper.ReadIntInRange(
                "Választás: ", 0, 6);

            Console.WriteLine();

            try
            {
                switch (choice)
                {
                    case 1:
                        AddExercise();
                        break;

                    case 2:
                        DeleteExercise();
                        break;

                    case 3:
                        ListExercises();
                        break;

                    case 4:
                        SearchExercises();
                        break;

                    case 5:
                        CreateWorkoutPlan();
                        break;

                    case 6:
                        DeleteWorkoutPlan();
                        break;

                    case 0:
                        SaveData();
                        Console.WriteLine("Adatok mentve. Kilépés...");
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba: {ex.Message}");
            }

            InputHelper.WaitForEnter();
            Console.WriteLine();
        }
    }

    private static void PrintMainMenu()
    {
        Console.WriteLine("====================================");
        Console.WriteLine("         WORKOUT TRACKER");
        Console.WriteLine("====================================");
        Console.WriteLine("1. Gyakorlat hozzáadása");
        Console.WriteLine("2. Gyakorlat törlése");
        Console.WriteLine("3. Gyakorlatok listázása");
        Console.WriteLine("4. Gyakorlat keresése");
        Console.WriteLine("5. Edzésterv létrehozása");
        Console.WriteLine("6. Edzésterv törlése");
        Console.WriteLine("0. Kilépés");
        Console.WriteLine("====================================");
    }

    private void AddExercise()
    {
        string name = InputHelper.ReadRequiredString(
            "Gyakorlat neve: ");

        string muscleGroup = InputHelper.ReadRequiredString(
            "Izomcsoport: ");

        int defaultSets = InputHelper.ReadPositiveInt(
            "Sorozatok száma: ");

        int defaultReps = InputHelper.ReadPositiveInt(
            "Ismétlések száma: ");

        Exercise exercise = _exerciseService.AddExercise(
            name,
            muscleGroup,
            defaultSets,
            defaultReps);

        SaveData();

        Console.WriteLine(
            $"Sikeresen hozzáadva: {exercise.Name}");
    }

    private void DeleteExercise()
    {
        Exercise? exercise = SelectExercise();

        if (exercise is null)
        {
            return;
        }

        bool deleted = _exerciseService.DeleteExercise(
            exercise.Id);

        if (deleted)
        {
            SaveData();
            Console.WriteLine("A gyakorlat törölve.");
        }
        else
        {
            Console.WriteLine("A gyakorlat nem található.");
        }
    }

    private void ListExercises()
    {
        List<Exercise> exercises =
            _exerciseService.GetAllExercises();

        PrintExercises(exercises);
    }

    private void SearchExercises()
    {
        string searchTerm = InputHelper.ReadRequiredString(
            "Keresési kifejezés: ");

        List<Exercise> exercises =
            _exerciseService.SearchExercises(searchTerm);

        PrintExercises(exercises);
    }

    private void CreateWorkoutPlan()
    {
        string name = InputHelper.ReadRequiredString(
            "Edzésterv neve: ");

        WorkoutPlan workoutPlan =
            _workoutService.CreateWorkoutPlan(name);

        SaveData();

        Console.WriteLine(
            $"Edzésterv létrehozva: {workoutPlan.Name}");
    }

    private void DeleteWorkoutPlan()
    {
        WorkoutPlan? workoutPlan = SelectWorkoutPlan();

        if (workoutPlan is null)
        {
            return;
        }

        bool deleted = _workoutService.DeleteWorkoutPlan(
            workoutPlan.Id);

        if (deleted)
        {
            SaveData();
            Console.WriteLine("Az edzésterv törölve.");
        }
        else
        {
            Console.WriteLine("Az edzésterv nem található.");
        }
    }

    private Exercise? SelectExercise()
    {
        List<Exercise> exercises =
            _exerciseService.GetAllExercises();

        if (exercises.Count == 0)
        {
            Console.WriteLine("Nincs választható gyakorlat.");
            return null;
        }

        Console.WriteLine("Gyakorlatok:");

        for (int i = 0; i < exercises.Count; i++)
        {
            Console.WriteLine(
                $"{i + 1}. {exercises[i].Name}");
        }

        int choice = InputHelper.ReadIntInRange(
            "Válassz gyakorlatot: ",
            1,
            exercises.Count);

        return exercises[choice - 1];
    }

    private WorkoutPlan? SelectWorkoutPlan()
    {
        List<WorkoutPlan> workoutPlans =
            _workoutService.GetAllWorkoutPlans();

        if (workoutPlans.Count == 0)
        {
            Console.WriteLine("Nincs választható edzésterv.");
            return null;
        }

        Console.WriteLine("Edzéstervek:");

        for (int i = 0; i < workoutPlans.Count; i++)
        {
            Console.WriteLine(
                $"{i + 1}. {workoutPlans[i].Name}");
        }

        int choice = InputHelper.ReadIntInRange(
            "Válassz edzéstervet: ",
            1,
            workoutPlans.Count);

        return workoutPlans[choice - 1];
    }

    private static void PrintExercises(List<Exercise> exercises)
    {
        if (exercises.Count == 0)
        {
            Console.WriteLine("Nincs találat.");
            return;
        }

        foreach (Exercise exercise in exercises)
        {
            Console.WriteLine(
                $"- {exercise.Name} | " +
                $"Izomcsoport: {exercise.MuscleGroup} | " +
                $"{exercise.DefaultSets} x {exercise.DefaultReps}");
        }
    }

    private void SaveData()
    {
        _dataRepository.Save(
            _exerciseService.GetAllExercises(),
            _workoutService.GetAllWorkoutPlans(),
            _workoutService.GetWorkoutHistory());
    }
}
