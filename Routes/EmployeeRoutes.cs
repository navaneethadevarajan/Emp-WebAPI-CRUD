using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
[ApiExplorerSettings(GroupName = "Employee360")]
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


        // app.MapPost("/CreateEmployee", (Employee employee, IEmployeeManagement empManagement) =>
        // {
        //     try
        //     {
        //         empManagement.CreateEmployee(employee);
        //         return Results.Ok(new { message = "Employee created successfully" });
        //     }
        //     catch (Exception ex)
        //     {
        //         return Results.BadRequest(new { message = "Error creating employee", error = ex.Message });
        //     }
        // })
        // .WithName("CreateEmployee")
        // .WithOpenApi();
        app.MapPost("/CreateEmployee", (Employee employee, IDALBase dal) =>
        {
            try
            {
                var createdEmployee = dal.CreateEmployee(employee);
                return Results.Ok(createdEmployee); 
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = "Error creating employee", error = ex.Message });
            }
        })
        .WithName("CreateEmployee")
        .WithOpenApi();


        // app.MapPut("/UpdateEmployeeById/{employeeId}", (int employeeId,Employee updatedEmployee, IEmployeeManagement empManagement) =>
        // {
        //     try
        //     {
        //         empManagement.UpdateEmployeeById(employeeId,updatedEmployee);
        //         return Results.Ok(new { message = "Employee updated successfully" });
        //     }
        //     catch (Exception ex)
        //     {
        //         return Results.BadRequest(new { message = "Error updating employee", error = ex.Message });
        //     }
        // })
        // .WithName("UpdatedEmployeeById")
        // .WithOpenApi();
        app.MapPut("/UpdateEmployeeById", async (HttpContext context, IEmployeeManagement empManagement) =>
        {
            try
            {
                var form = await context.Request.ReadFormAsync(); // Read multipart form data
                int employeeId = int.Parse(form["employeeId"]);

                Employee updatedEmployee = new Employee
                {
                    EmployeeName = form["EmployeeName"],
                    EmployeeAge = int.Parse(form["EmployeeAge"]),
                    EmployeeLocation = form["EmployeeLocation"],
                    EmployeeDateOfBirth = DateTime.Parse(form["EmployeeDateOfBirth"]),
                    DepartmentId = int.Parse(form["DepartmentId"]),
                    FileName = form.Files.Count > 0 ? form.Files[0].FileName : null
                };

                // Handle File Upload
                if (form.Files.Count > 0)
                {
                    var file = form.Files[0];
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    var filePath = Path.Combine(uploadsFolder, file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                empManagement.UpdateEmployeeById(employeeId, updatedEmployee);
                return Results.Ok(new { message = "Employee updated successfully" });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { message = "Error updating employee", error = ex.Message });
            }
        });


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
        //This method uploads the file and stores the file in the local directory
        // app.MapPost("/UploadEmployeeFile/{employeeId}", async (int employeeId, IFormFile file, IEmployeeManagement empManagement) =>
        // {
        //     if (file == null || file.Length == 0)
        //     {
        //         return Results.BadRequest("No file uploaded.");
        //     }

        //     var Uploads = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        //     if (!Directory.Exists(Uploads))
        //     {
        //         Directory.CreateDirectory(Uploads);
        //     }

        //     var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        //     var filePath = Path.Combine(Uploads, fileName);

        //     using (var stream = new FileStream(filePath, FileMode.Create))
        //     {
        //         await file.CopyToAsync(stream);
        //     }

        //     empManagement.SaveEmployeeFile(employeeId, fileName);
        //     return Results.Ok(new { message = "File uploaded successfully", fileName });
        // }).DisableAntiforgery()
        // .WithName("UploadEmployeeFile")
        // .WithOpenApi();
        app.MapPost("/UploadEmployeeFile/{employeeId}", async (int employeeId, [FromForm] IFormFile file, IEmployeeManagement empManagement) =>
        {
            if (file == null || file.Length == 0)
            {
                return Results.BadRequest("No file uploaded.");
            }

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            empManagement.SaveEmployeeFile(employeeId, fileName);
            return Results.Ok(new { message = "File uploaded successfully", fileName });
        })
        .DisableAntiforgery()
        .WithName("UploadEmployeeFile")
        .WithOpenApi();

       app.MapGet("/GetEmployeeFile/{employeeId}", async (int employeeId, IEmployeeManagement empManagement, HttpContext context) =>
        {
            var fileName = empManagement.GetEmployeeFileName(employeeId);
            if (string.IsNullOrEmpty(fileName))
            {
                return Results.NotFound("⚠ No file found for this employee.");
            }
            var fileExtension = Path.GetExtension(fileName).ToLower();
            if (fileExtension != ".pdf")
            {
                return Results.BadRequest(" Only PDF files are allowed.");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return Results.NotFound("⚠ File does not exist on the server.");
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

            context.Response.Headers["Content-Disposition"] = "inline; filename=" + fileName;
            context.Response.ContentType = "application/pdf";
            
            await context.Response.Body.WriteAsync(fileBytes);
            return Results.Empty;
        });
            }
}