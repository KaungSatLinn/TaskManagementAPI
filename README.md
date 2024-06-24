# TaskManagementAPI

A simple RESTful API for managing tasks, built with ASP.NET Core using .NET 8.

## Features

* **CRUD Operations:** Create, read, update, and delete tasks.
* **Task Filtering:** Filter tasks by completion status or priority.

## Getting Started

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) 
* [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or access to a SQL Server instance)
* An IDE like [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)
* [GitHub Desktop](https://desktop.github.com/) (Optional)

### Installation & Setup

You can set up this project using either GitHub Desktop or Visual Studio 2022:

#### Clone the Repository

1. **Using GitHub Desktop:**
   - Open GitHub Desktop and click "File" -> "Clone Repository."
   - Search for "KaungSatLinn/TaskManagementAPI" and click "Clone."
   - Choose a local path for the repository.

#### Open the Project

1. **Using Visual Studio 2022:**
   - Open the `TaskManagementAPI.sln` file from the cloned directory.

#### Database Setup (Both Options)

1. **Connection String:** 
   - In Visual Studio 2022, open the `appsettings.json` file.
   - Modify the `ConnectionStrings.DefaultConnection` value to match your SQL Server credentials (server name, database name, user ID, and password). 
2. **Create Database and Table:**
   - Open SQL Server Management Studio (SSMS).
   - Connect to your SQL Server instance.
   - Execute the `Create DB & Tables.sql` script located in the project's root directory, [DB Script](TaskManagementAPI/DB). This will create the database and necessary tables and data.

#### Restore Dependencies (Visual Studio 2022 Only)

1. Right-click on the project in the Solution Explorer and select "Restore NuGet Packages."

### Running the Application

1. **Build and Run:**
   - **Visual Studio 2022:** Press F5 or click the green "Start" button.
   - **Command Line (If not using Visual Studio):** Navigate to the project directory in your terminal and run `dotnet run`.

## API Endpoints

1.The API will typically be accessible at `https://localhost:5001/api/tasks`.

The following endpoints are available:

| Endpoint                  | HTTP Method | Purpose                     |
| ------------------------- | ----------- | --------------------------- |
| `/api/tasks`              | GET         | Get all tasks               |
| `/api/tasks/create`       | POST        | Create a new task           |
| `/api/tasks/{id}`         | GET         | Get a specific task by ID   |
| `/api/tasks/{id}`         | PUT         | Update a task by ID         |
| `/api/tasks/delete/{id}`  | POST        | Delete a task by ID         | 

## Frontend Setup

The frontend source code for this application is located in the [FrontEnd](FrontEnd) folder. 
You can directly run the Index page at [index.html]`FrontEnd/pages/tasks/index.html` after the API is run.
