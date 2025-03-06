using EmployeeManagementSystem.BLL.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddSingleton<IDALBase, DALBase>();
builder.Services.AddSingleton<IDepartmentManagement, DepartmentManagement>();
builder.Services.AddSingleton<IEmployeeManagement, EmployeeManagement>();
builder.Services.AddSingleton<VersionService>();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger with custom title
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Employee360",  // Updated title
        Version = "1.0",
    });
});

var app = builder.Build();

app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee360 v1");
    c.DocumentTitle = "Employee360 API"; 
    
});

app.UseHttpsRedirection();
app.RegisterEmployeeRoutes();
app.RegisterDepartmentRoutes();
app.RegisterVersionRoutes();
app.Run();
