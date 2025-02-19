# Employee Management System API
This project was created using .NET 8.0.405.

This Employee Management System Web API Application handles all the CRUD operation regarding both the classes namely Employee and Department.

It opens the Swagger UI which has all the endpoints in it and we can test the endpoints through this Swagger tool.

To know more about the Swagger tool visit https://www.geeksforgeeks.org/testing-apis-with-swagger/.

## Development Server
To start the local development server, run:

dotnet run

Once the server is running, open your browser and navigate to http://localhost:5081/. 

This will open the Swagger UI where you can test the API end points

<img width="960" alt="image" src="https://github.com/user-attachments/assets/5e7bc2d9-9a84-4ee1-b498-81ae42d6867b" />

The above image shows the Swagger UI for the Employee Management System Which has the endpoinds for both employee and department classee.

## Project Structure
The project follows a three-tier architecture with the following layers:

Data Access Layer (DAL): Contains the data access logic.

Business Logic Layer (BLL): Contains the business logic.

Data Model: Contains the data models.

Routes: Contains the route definitions for the API endpoints.

Uploads: Contains the uploaded files in the current directory.

<img width="244" alt="image" src="https://github.com/user-attachments/assets/f92b5c3f-225a-407b-a593-b865de200d22" />


## Adding a New Endpoint
To add a new endpoint, follow these steps:

Define the Route: Add a new route in the appropriate route file (e.g., EmployeeRoutes.cs).

Implement the Logic: Implement the logic for the endpoint in the corresponding BLL and DAL classes.

## Building
To build the project, run:

dotnet build

This will compile your project and store the build artifacts in the bin/ directory.

dotnet run

This will start the application and make it available at http://localhost:5081/.

## Additional Resources
For more information on using .NET CLI, including detailed command references, visit the .NET CLI Overview and Command Reference:

https://learn.microsoft.com/en-us/dotnet/core/tools/
