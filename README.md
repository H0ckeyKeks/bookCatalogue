# bookCatalogue

A simple console based book catalogue application built in **C#**, allowing users to **add, view, delete and rate** books. The program is connected to a local PostgreSQL database for storage.

## Features
**Add Books** - Store books in the database

**View Books** - Display a book

**Delete Books** - Remove books from the database (catalogue)

**Rate Books** - Give books a rating from 0 - 5 points per category (there are 7 different categories)

## Technologies used
- C# (.NET 8)
- SQL Server (PostgreSQL) -> for database storage
- Npgsql (PostgreSQL .NET data provider) -> for database operations
- ConsoleTableExt -> for table format in a console application
- Environment variables -> for secure database credentials

## Security
This project uses an environmental variable to store the database password (sensitive data).

### Windows (Command Prompt)
```bash
setx DATABASE_PASSWORD "your_secure_password"
