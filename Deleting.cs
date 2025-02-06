using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace BookCatalogue
{
    internal class Deleting
    {
        public static void DeleteBookMenu()
        {
            Console.WriteLine("\nWhich Book do you want to delete?");

            string deletionTerm = Console.ReadLine();

            DeleteBook(deletionTerm);

        }

        public static void DeleteBook(string deletionTerm)
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
                    Viewing.ViewBooks(deletionTerm);

                    int deleteId;
                    bool validDeleteId = Program.ValidateIntegerInput("\nPlease enter the ID of the book you want to delete: \n" +
                        "If you don't want to delete a book, press e.", out deleteId);
                    
                    
                    while (!validDeleteId)
                    {
                        Console.WriteLine("Do you really want to exit? (Y = Yes, N = No)");
                        string answer = Console.ReadLine().ToUpper();

                        if (answer == "Y")
                        {
                            return;
                        }
                        else
                        {
                            validDeleteId = Program.ValidateIntegerInput("\nPlease enter the ID of the book you want to delete: \n", out deleteId);
                        }

                    }

                    // SQL-Query to find the book
                    string sqlQuery1 = "DELETE FROM \"AuthorBook\" WHERE \"BookId\" = @deleteId";
                    string sqlQuery2 = "DELETE FROM \"Book\" WHERE \"Id\" = @deleteId";

                    // First deletion
                    using (var cmd = new NpgsqlCommand(sqlQuery1, conn))
                    {
                        // Use parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("deleteId", deleteId);
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd2 = new NpgsqlCommand(sqlQuery2, conn))
                    {
                        cmd2.Parameters.AddWithValue("deleteId", deleteId);
                        int deletions = cmd2.ExecuteNonQuery();

                        // Check if book is deleted
                        if (deletions > 0)
                        {
                            // Output if book is deleted
                            Console.WriteLine("Book deleted successfully!\n");
                        }
                        else
                        {
                            // Output if there is no book found
                            Console.WriteLine("No book found with the given Id.\n");
                        }
                    }              
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}

