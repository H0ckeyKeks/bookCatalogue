using Npgsql;

namespace BookCatalogue
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool keepRunning = true;

            while (keepRunning)
            {
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("╔═════════════════════════╗");
                Console.WriteLine("║     Book Catalogue      ║");
                Console.WriteLine("╚═════════════════════════╝");
                Console.WriteLine("");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("1. Add a Book");
                Console.WriteLine("2. Delete a Book");
                Console.WriteLine("3. View Books");
                Console.WriteLine("4. Rate a Book");
                Console.WriteLine("5. Exit");
                Console.WriteLine("");
                Console.ResetColor();
                Console.WriteLine("Your Choice: ");

                string? choice = Console.ReadLine();


                switch (choice)
                {
                    case "1":
                        Adding.AddBookMenu();
                        break;

                    case "2":
                        Deleting.DeleteBookMenu();
                        break;
                    
                    case "3":
                        Viewing.ViewBooksMenu();
                        break;
                    
                    case "4":
                        Rating.RateBooksMenu();
                        break;
                    
                    case "5":
                        keepRunning = false;
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice, please try again.");
                        Console.ResetColor();
                        break;
                }
            }
            
        }

        // Methods for errorhandling
        public static bool ValidateIntegerInput(string prompt, out int validInput)
        {
            // Setting a default value for validInput
            // Neccessary for returning a value in case of failure and because out parameters must be assigned
            // a value before the method exists
            validInput = 0;

            // Prompt for user input
            Console.WriteLine(prompt);
            string userInput = Console.ReadLine();

            // Check if input can be parsed into an integer -> if so: valid input => return true
            if (int.TryParse(userInput, out validInput))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Invalid input! Please enter a valid number.");
                return false;
            }

        }

        public static bool ValidateDoubleInput(string prompt, out double validDoubleInput)
        {
            // Setting a default value for validInput
            // Neccessary for returning a value in case of failure and because out parameters must be assigned
            // a value before the method exists
            validDoubleInput = 0;

            // Prompt for user input
            Console.WriteLine(prompt);
            string userInput = Console.ReadLine();

            // Check if input can be parsed into an integer -> if so: valid input => return true
            if (double.TryParse(userInput, out validDoubleInput))
            {
                return true;
            }
            else
            {
                Console.WriteLine("Invalid input! Please enter a valid floating-point number.");
                return false;
            }

        }
    }
}
