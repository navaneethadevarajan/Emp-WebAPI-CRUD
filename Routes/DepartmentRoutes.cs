public static class DepartmentRoutes
{
    public static void RegisterDepartmentRoutes(this WebApplication app)
    {
        app.MapGet("/GetAllDept", (IDepartmentManagement deptManagement) =>
        {
            var dept=deptManagement.GetDepartments();
            return Results.Ok(dept);
        })
        .WithName("GetAllDept")
        .WithOpenApi();
        app.MapPost("/CreateDepartment", (Department department, IDepartmentManagement deptManagement) =>
        {
            if (string.IsNullOrEmpty(department.DepartmentName))
            {
                return Results.BadRequest("Department name is required.");
            }

            deptManagement.CreateDepartment(department);
            return Results.Ok("Department created successfully.");
        })
        .WithName("CreateDepartment")
        .WithOpenApi();
        app.MapPut("/UpdateDepartmentById", (int departmentId,Department updatedDepartment, IDepartmentManagement deptManagement) =>
        {
            try
            {
                deptManagement.UpdateDepartmentById(departmentId,updatedDepartment);
                return Results.Ok(new { message = "Department updated successfully" });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = "Error updating department", error = ex.Message });
            }
        })
        .WithName("UpdatedDepartmentById")
        .WithOpenApi();
        app.MapDelete("/RemoveDepartmentById", (int departmentId, IDepartmentManagement deptManagement, IEmployeeManagement empManagement) =>
        {
            try
            {
                var employeesInDept = empManagement.GetEmployeesByDeptId(departmentId);

                if (employeesInDept.Any()) 
                {
                    return Results.BadRequest(new
                    {
                        message = "Department cannot be removed because it is associated with employees.",
                        //employees = employeesInDept, 
                        suggestion = "Please update the department for the employees before removing the department."
                    });
                }
                deptManagement.RemoveDepartmentById(departmentId);
                return Results.Ok(new { message = "Department removed successfully" });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = "Error removing department", error = ex.Message });
            }
        })
        .WithName("RemoveDepartmentById")
        .WithOpenApi();
    }
}