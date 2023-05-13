# AB2CMigration

AB2CMigration is an ASP.NET MVC website that allows you to retrieve a list of usernames from B2C and save them to blob storage. The website includes a library named ConnectionB2C.

## Project Structure

- `AB2CMigration` - ASP.NET MVC website
- `ConnectionB2C` - Library for connecting to B2C

## Features

- List Retrieval - You can retrieve a list of usernames from B2C and display them on the website.
- Blob Storage - You can choose to save the list of usernames to blob storage.
- B2C Connection - The project includes a library named ConnectionB2C for connecting to B2C.

## Technologies Used

- ASP.NET MVC framework
- ConnectionB2C library

## How to Run

1. Clone the repository to your local machine.
2. Open the solution in Visual Studio.
3. Build the solution.
4. Run the `AB2CMigration` project.
5. Navigate to the website and retrieve the list of usernames from B2C.

## Options

### Option 1: Direct Retrieval

You can directly use the ConnectionB2C library to retrieve the list of usernames from B2C. The list can then be displayed on the website or saved to blob storage.

### Option 2: Input File

You can also upload an input file containing a list of usernames to the website. The list can then be saved to blob storage.


