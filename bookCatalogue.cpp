#include <iostream>
#include <libpq-fe.h>

using namespace std;

// Function to handle occuring errors
void finish_with_error(PGconn* conn)
{
    cerr << "Error: " << PQerrorMessage(conn);  // Prints out error message
	PQfinish(conn);                             // Closes the connection and frees the memory
	exit(1);									// Exits the program with exit status of 1
}

int main()
{
    // Connection string
    const char* coninfo = "host=localhost port=5432 dbname=book_archive user=hockey-keks password=postgres";

    // Connection with database
    PGconn* conn = PQconnectdb(coninfo);

    // Checking Connection
    if (PQstatus(conn) != CONNECTION_OK)
    {
        finish_with_error(conn);
    }

    // SQL-Query: Select data from the table
    const char* select_query = "SELECT * FROM book_catalogue";
	PGresult* res = PQexec(conn, select_query);

	// Check if SELECT was successful
    if (PQresultStatus(res) != PGRES_TUPLES_OK)
    {
        PQclear(res);
        finish_with_error(conn);
    }
}
