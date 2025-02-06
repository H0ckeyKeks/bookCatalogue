using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace BookCatalogue
{
    internal class Rating
    {
        public static void RateBooksMenu()
        {
            Console.WriteLine("\nWhich Book do you want to rate? Enter a seach term: ");

            string ratingTerm = Console.ReadLine();

            RateBook(ratingTerm);

        }

        public static void RateBook(string ratingTerm)
        {
            string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

            if (string.IsNullOrEmpty(dbPassword))
            {
                Console.WriteLine("Password is not set in environment variables.");
                return;
            }

            string connString = $"Host=localhost;Username=postgres;Password={dbPassword}; Database=book_archive";

            // Opening the connection to the database
            using (var conn = new NpgsqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // Searching for the book
                    Viewing.ViewBooks(ratingTerm);

                    int ratingId;
                    bool validRatingId = Program.ValidateIntegerInput("\nPlease enter the ID of the book you want to rate: \n" +
                        "If you don't want to rate a book, press e.", out ratingId);
                    while (!validRatingId)
                    {
                        Console.WriteLine("Do you really want to exit? (Y = Yes, N = No)");
                        string answer = Console.ReadLine().ToUpper();

                        if (answer == "Y")
                        {
                            return;
                        }
                        else
                        {
                            validRatingId = Program.ValidateIntegerInput("\nPlease enter the ID of the book you want to rate: \n", out ratingId);
                        }
                        
                    }

                    double rating = RatingCatalogue();
                        

                    // SQL-Query to add the rating to the database
                    string sqlQuery = "UPDATE \"Book\" SET \"Rating\" = @rating WHERE \"Id\" = @ratingId";

                    using (var cmd = new NpgsqlCommand(sqlQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("ratingId", ratingId);
                        cmd.Parameters.AddWithValue("rating", rating);
                        int booksRated = cmd.ExecuteNonQuery();

                        if (booksRated > 0)
                        {
                            Console.WriteLine("Rating successfully added!\n");
                        }
                        else
                        {
                            Console.WriteLine("No rating was updated.\n");
                        }                           
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        public static double RatingCatalogue()
        {
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("╔══════════════════════════╗");
            Console.WriteLine("║     Rating Catalogue     ║");
            Console.WriteLine("╚══════════════════════════╝");
            Console.WriteLine("");
            Console.ResetColor();

            // Criteria for rating the plot
            Console.WriteLine("\nPlease enter the points you want to give for each criteria (0 - 5). Floating-point numbers are also allowed: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1. THE PLOT");
            Console.ResetColor();

            double plotPoints;
            bool validPlotPoints = Program.ValidateDoubleInput("\n- Is it exciting and well structured?\n" +
                                                                "- Does the story maintain interest until the end?\n" +
                                                                "- Are the plot twists and conflicts believable and meaningful?\n" +
                                                                "- Is the plot resolved well or does it remain unclear?\n", out plotPoints);
            while (!validPlotPoints)
            {
                validPlotPoints = Program.ValidateDoubleInput("\nNumber of pages: ", out plotPoints);
            }
            Console.WriteLine("");

            // Criteria for rating the characters
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("2. THE CHARACTERS");
            Console.ResetColor();

            double charPoints;
            bool validCharPoints = Program.ValidateDoubleInput("\n- Are the characters well-developed and multi-dimensional?\n" +
                                                                "- Do they come across as believable and realistic?\n" +
                                                                "- Do they grow/change throughout the story?\n" +
                                                                "- Are relationships between the characters authentic and interesting?\n", out charPoints);
            while (!validCharPoints)
            {
                validCharPoints = Program.ValidateDoubleInput("\nNumber of pages: ", out charPoints);
            }
            Console.WriteLine("");

            // Criteria for rating the writing style
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("3. THE WRITING STYLE");
            Console.ResetColor();

            double writingPoints;
            bool validWritingPoints = Program.ValidateDoubleInput("\n- Is it pleasant and easy to understand?\n" +
                                                                "- Does the author use creative and concise language?\n" +
                                                                "- Does the style contribute to the atmosphere and mood of the book?\n" +
                                                                "- Is there a good balance between dialogue, descriptions and inner monologues?\n", out writingPoints);
            while (!validWritingPoints)
            {
                validWritingPoints = Program.ValidateDoubleInput("\nNumber of pages: ", out writingPoints);
            }
            Console.WriteLine("");

            // Criteria for the atmosphere and setting
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("4. THE ATMOSPHERE AND SETTING");
            Console.ResetColor();

            double atmPoints;
            bool validAtmPoints = Program.ValidateDoubleInput("\n- Is the setting discribed in detail and vividly?\n" +
                                                                "- Does the atmosphere help to reinforce the mood of the story?\n" +
                                                                "- Does the setting feel authentic and believable, or does it seem artificial?\n", out atmPoints);
            while (!validAtmPoints)
            {
                validAtmPoints = Program.ValidateDoubleInput("\nNumber of pages: ", out atmPoints);
            }
            Console.WriteLine("");

            // Criteria for the tension and pacing
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("5. THE TENSION AND PACING");
            Console.ResetColor();

            double tensionPoints;
            bool validTensionPoints = Program.ValidateDoubleInput("\n- Does the book maintain consistent tension or are there long, dull passages?\n" +
                                                                "- How well is the pacing of the story regulated (not too fast, but also not too slow)?\n" +
                                                                "- Are there exciting high points and quiet moments in the right balance?\n", out tensionPoints);
            while (!validTensionPoints)
            {
                validTensionPoints = Program.ValidateDoubleInput("\nNumber of pages: ", out tensionPoints);
            }
            Console.WriteLine("");

            // Criteria for the emotional impact
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("6. THE EMOTIONAL IMPACT");
            Console.ResetColor();

            double emoPoints;
            bool validEmoPoints = Program.ValidateDoubleInput("\n- Does the book have an emotional impact on the reader?\n" +
                                                                "- Do you feel connected to the characters or moved by their story?\n" +
                                                                "- Is the emotion conveyed authentically or does it feel forced?\n", out emoPoints);
            while (!validEmoPoints)
            {
                validEmoPoints = Program.ValidateDoubleInput("\nNumber of pages: ", out emoPoints);
            }
            Console.WriteLine("");

            // Criteria for the overall effect
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("7. THE OVERALL EFFECT");
            Console.ResetColor();

            double effectPoints;
            bool validEffectPoints = Program.ValidateDoubleInput("\n- Would you recommend this book to others?\n" +
                                                                "- Did you feel you gained someting valuable from reading it?\n" +
                                                                "- Would you read it again?\n" +
                                                                "- Was reading it fun?\n", out effectPoints);
            while (!validEffectPoints)
            {
                validEffectPoints = Program.ValidateDoubleInput("\nNumber of pages: ", out effectPoints);
            }
            Console.WriteLine("");

            // Calculating the average rating
            double averagePoints = (plotPoints + charPoints + writingPoints + atmPoints + tensionPoints + emoPoints + effectPoints) / 7;

            double roundedAverage = Math.Round(averagePoints, 2);
            return roundedAverage;
        }
    }
}

