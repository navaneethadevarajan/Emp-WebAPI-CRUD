Overview:
The Employee Management System (EMS) is a web-based application designed to manage employees, departments, and their relationships.
The application allows for basic CRUD operations (Create, Read, Update, Delete) for both employee and department data. 
This system can handle employee assignments to departments, department management and employee details management.
This application is built using ASP.NET Core and interacts with a PostgreSQL database for data storage. 
It provides RESTful API endpoints for managing employee and department data.
Technologies Used:
ASP.NET Core: Used for building the web API.
PostgreSQL: Database system to store employee and department data.
Npgsql: .NET Data Provider for PostgreSQL, used to interact with the database.
C#: Language used to build the application logic.
Swagger: API documentation and testing interface.
Architecture
The architecture of the Employee Management System follows a layered approach, with the following key components:
Data Model (Entities):This layer contains the auto-implemented properties for the classes Employee and Department.
Business Logic Layer (BLL):Contains the business logic for managing departments and employees, using the methods provided by the DAL.
Data Access Layer (DAL):Contains methods for interacting with the PostgreSQL database. This layer performs CRUD operations on departments and employees.
