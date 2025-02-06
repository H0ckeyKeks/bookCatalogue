using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace BookCatalogue
{
    public class Viewing
    {
        public static void ViewBooksMenu()
        {
            Console.WriteLine("\nEnter a search term");

            string searchTerm = Console.ReadLine();

            ViewBooks(searchTerm);

        }

        public static List<int> ViewBooks(string searchTerm)
        {
            // Creating a list of bookIds
            List<int> bookIds = new List<int>();

            string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

            if (string.IsNullOrEmpty(dbPassword))
            {
                Console.WriteLine("Password is not set in environment variables.");
                return bookIds;
            }

            string connString = $"Host=localhost;Username=postgres;Password={dbPassword}; Database=book_archive";

            // Opening the connection to the database
            using (var conn = new NpgsqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("Connection Established!");

                    // SQL-Query to find the book
                    string sqlQuery = "SELECT * FROM \"Book\"" +
                    " JOIN \"AuthorBook\" ON \"Book\".\"Id\" = \"AuthorBook\".\"BookId\"" +
                    " JOIN \"Author\" ON \"AuthorBook\".\"AuthorId\" = \"Author\".\"Id\"" +
                    " WHERE lower(\"Book\".\"Title\") LIKE @searchTerm" +
                    " OR lower(\"Author\".\"FirstName\") LIKE @searchTerm" +
                    " OR lower(\"Author\".\"LastName\") LIKE @searchTerm" +
                    " OR lower(\"Book\".\"Publisher\") LIKE @searchTerm" +
                    " OR lower(\"Book\".\"Summary\") LIKE @searchTerm";

                    using (var cmd = new NpgsqlCommand(sqlQuery, conn))
                    {
                        // Use parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("searchTerm", $"%{searchTerm.ToLower()}%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            // Read and print each row
                            while (reader.Read())
                            {
                                int bookId = int.Parse(reader["Id"].ToString());
                                bookIds.Add(bookId);

                                Console.WriteLine($"Book ID: {reader["Id"]}");
                                Console.WriteLine($"Title: {reader["Title"]}");
                                Console.WriteLine($"Publisher: {reader["Publisher"]}");
                                Console.WriteLine($"Summary: {reader["Summary"]}");
                                Console.WriteLine($"Author: {reader["Firstname"]} {reader["Lastname"]}");
                                Console.WriteLine($"Rating: {reader["Rating"]}");
                                Console.WriteLine("----------------------------------------");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

            }

            return bookIds;
        }
    }
}

