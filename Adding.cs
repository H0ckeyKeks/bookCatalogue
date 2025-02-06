using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using BookCatalogue;

namespace BookCatalogue
{
    public class Adding
    {
        public static void AddBookMenu()
        {
            // Getting the Title
            Console.WriteLine("\nBook title: ");
            string title = Console.ReadLine();

            // Getting the number of pages
            int pages;
            bool validPages = Program.ValidateIntegerInput("\nNumber of pages: ", out pages);
            while (!validPages)
            {
                validPages = Program.ValidateIntegerInput("\nNumber of pages: ", out pages);
            }

            // Getting the publisher
            Console.WriteLine("\nPublisher: ");
            string publisher = Console.ReadLine();

            // Getting the year of the release
            int releaseYear;
            bool validReleaseYear = Program.ValidateIntegerInput("\nYear of release: ", out releaseYear);
            while (!validReleaseYear)
            {
                validReleaseYear = Program.ValidateIntegerInput("\nYear of release: ", out releaseYear);
            }

            // Getting a short summary
            Console.WriteLine("\nSummary: ");
            string summary = Console.ReadLine();

            // Get ISBN
            Console.WriteLine("\nISBN: ");
            string isbn = Console.ReadLine();

            // Getting name of the author
            Console.WriteLine("\nAuthor's first name: ");
            string firstName = Console.ReadLine();

            Console.WriteLine("\nAuthor's last name: ");
            string lastName = Console.ReadLine();

            var book = new Book
            {
                Title = title,
                Pages = pages,
                Publisher = publisher,
                ReleaseYear = releaseYear,
                Summary = summary,
                ISBN = isbn
            };

            var author = new Author
            {
                FirstName = firstName,
                LastName = lastName
            };

            AddBook(book, author);
        }

        public static void AddBook(Book book, Author author)
        {
            // Making sure that the database password is not shown in the code:
            // 1. Creating an environmental variable by using setx DB_PASSWORD "your_password" in Command Prompt or Powershell
            // 2. Reading the environmental variable in the code
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
                    Console.WriteLine("Connection Established!");

                    // Insert a book into the "Book" table
                    string bookSql = "INSERT INTO \"Book\" (\"Title\", \"Pages\", \"Publisher\", \"ReleaseYear\", \"Summary\", \"ISBN\") " +
                                     "VALUES (@title, @pages, @publisher, @releaseYear, @summary, @isbn) RETURNING \"Id\"";

                    int bookId;
                    using (var cmd = new NpgsqlCommand(bookSql, conn))
                    {
                        cmd.Parameters.AddWithValue("title", book.Title);
                        cmd.Parameters.AddWithValue("pages", book.Pages);
                        cmd.Parameters.AddWithValue("publisher", book.Publisher);
                        cmd.Parameters.AddWithValue("releaseYear", book.ReleaseYear);
                        cmd.Parameters.AddWithValue("summary", book.Summary);
                        cmd.Parameters.AddWithValue("isbn", book.ISBN);

                        bookId = (int)cmd.ExecuteScalar();
                        Console.WriteLine($"Book added with ID: {bookId}");
                    }

                    // Check if the Author already exists - if not, insert a new author
                    int authorId;
                    string authorSql = "SELECT \"Id\" FROM \"Author\" WHERE \"FirstName\" = @firstName AND \"LastName\" = @lastName";

                    using (var cmd = new NpgsqlCommand(authorSql, conn))
                    {
                        cmd.Parameters.AddWithValue("firstName", author.FirstName);
                        cmd.Parameters.AddWithValue("lastName", author.LastName);

                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            // Author exists and gets the existing ID
                            authorId = (int)result;
                        }
                        else
                        {
                            // Author doesn't exist: enter new author
                            string insertAuthorSql = "INSERT INTO \"Author\" (\"FirstName\", \"LastName\") " + "VALUES (@firstName, @lastName) RETURNING \"Id\"";

                            using (var insertCmd = new NpgsqlCommand(insertAuthorSql, conn))
                            {
                                insertCmd.Parameters.AddWithValue("firstName", author.FirstName);
                                insertCmd.Parameters.AddWithValue("lastName", author.LastName);

                                authorId = (int)insertCmd.ExecuteScalar();
                                // ExecuteScalar is a method from the NpgsqlCommand
                                // It executes the query and returns the first column of the first row in the result set
                                // If there are no rows returned, it returns null

                                Console.WriteLine($"New author added with ID: {authorId}");
                            }

                        }
                    }

                    // Connect Author and Book inside the "AuthorBook" table
                    string authorBooksSql = "INSERT INTO \"AuthorBook\" (\"AuthorId\", \"BookId\") " + "VALUES (@authorId, @bookId)";

                    using (var cmd = new NpgsqlCommand(authorBooksSql, conn))
                    {
                        cmd.Parameters.AddWithValue("authorId", authorId);
                        cmd.Parameters.AddWithValue("bookId", bookId);

                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Author and Book are now connected.");
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
