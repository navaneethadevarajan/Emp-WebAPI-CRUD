public static class EmployeeRoutes
{
    public static void RegisterEmployeeRoutes(this WebApplication app)
    {
        app.MapGet("/GetAllEmp", (IEmployeeManagement empManagement) =>
        {
            var emp=empManagement.GetAllEmployees();
            return Results.Ok(emp);
        })
        .WithName("GetAllEmp")
        .WithOpenApi();
        app.MapGet("/GetEmployeesByDeptId/{departmentId}", (int departmentId,IEmployeeManagement empManagement) =>
        {
            var emp=empManagement.GetEmployeesByDeptId(departmentId);
            return Results.Ok(emp);
        })
        .WithName("GetEmployeesByDeptId")
        .WithOpenApi();
        app.MapPost("/CreateEmployee", (Employee employee, IEmployeeManagement empManagement) =>
        {
            try
            {
                empManagement.CreateEmployee(employee);
                return Results.Ok(new { message = "Employee created successfully" });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = "Error creating employee", error = ex.Message });
            }
        })
        .WithName("CreateEmployee")
        .WithOpenApi();
        app.MapPut("/UpdateEmployeeById", (int employeeId,Employee updatedEmployee, IEmployeeManagement empManagement) =>
        {
            try
            {
                empManagement.UpdateEmployeeById(employeeId,updatedEmployee);
                return Results.Ok(new { message = "Employee updated successfully" });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = "Error updating employee", error = ex.Message });
            }
        })
        .WithName("UpdatedEmployeeById")
        .WithOpenApi();
        app.MapDelete("/RemoveEmployeeById", (int employeeId, IEmployeeManagement empManagement) =>
        {
            try
            {
                empManagement.RemoveEmployeeById(employeeId);
                return Results.Ok(new { message = "Employee removed successfully" });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = "Error removing employee", error = ex.Message });
            }
        })
        .WithName("RemoveEmployeeById")
        .WithOpenApi();
    }
}