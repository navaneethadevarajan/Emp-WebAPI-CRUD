var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IDALBase, DALBase>(); 
builder.Services.AddSingleton<IDepartmentManagement, DepartmentManagement>(); 
builder.Services.AddSingleton<IEmployeeManagement, EmployeeManagement>();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.RegisterEmployeeRoutes();
app.RegisterDepartmentRoutes();
app.Run();

