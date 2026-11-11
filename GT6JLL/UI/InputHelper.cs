namespace WorkoutTracker.UI;

public static class InputHelper
{
    public static string ReadRequiredString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);

            string value =
                Console.ReadLine()?.Trim()
                ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            Console.WriteLine(
                "Hiba: az érték nem lehet üres.");
        }
    }

    public static int ReadPositiveInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);

            string input =
                Console.ReadLine()
                ?? string.Empty;

            if (int.TryParse(input, out int value)
                && value > 0)
            {
                return value;
            }

            Console.WriteLine(
                "Hiba: adj meg egy 0-nál nagyobb egész számot.");
        }
    }

    public static int ReadIntInRange(
        string prompt,
        int min,
        int max)
    {
        while (true)
        {
            Console.Write(prompt);

            string input =
                Console.ReadLine()
                ?? string.Empty;

            if (int.TryParse(input, out int value)
                && value >= min
                && value <= max)
            {
                return value;
            }

            Console.WriteLine(
                $"Hiba: {min} és {max} közötti egész számot adj meg.");
        }
    }

    public static void WaitForEnter()
    {
        Console.WriteLine();

        Console.Write(
            "Nyomj Entert a folytatáshoz...");

        Console.ReadLine();
    }
}
