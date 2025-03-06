using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using NpgsqlTypes;

public class DALBase : IDALBase
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=root;Database=EMSA";

        public List<Department> GetAllDepartments()
        {
            var departments = new List<Department>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM GetAllDepartments() ORDER BY DepartmentID ASC";  

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            departments.Add(new Department
                            {
                                DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                                DepartmentName = reader["DepartmentName"].ToString()
                            });
                        }
                    }
                }
            }

            return departments;
        }

        public List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT e.EmployeeId, 
                        e.EmployeeName, 
                        e.EmployeeAge, 
                        e.EmployeeLocation, 
                        e.EmployeeDateOfBirth,
                        d.DepartmentId, 
                        d.DepartmentName,
                        e.FileName
                    FROM Employee e
                    JOIN Department d ON e.DepartmentId = d.DepartmentId
                    ORDER BY e.EmployeeId ASC";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                                EmployeeName = reader["EmployeeName"].ToString(),
                                EmployeeAge = Convert.ToInt32(reader["EmployeeAge"]),
                                EmployeeLocation = reader["EmployeeLocation"].ToString(),
                                EmployeeDateOfBirth = Convert.ToDateTime(reader["EmployeeDateOfBirth"]),
                                DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                                DepartmentName = reader["DepartmentName"].ToString()
                            });
                        }
                    }
                }
            }
            return employees;
        }
    
    public List<Employee> GetEmployeesByDeptId(int departmentId)
    {
        var employees = new List<Employee>();

        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            string query = @"
                SELECT e.EmployeeId, 
                    e.EmployeeName, 
                    e.EmployeeAge, 
                    e.EmployeeLocation, 
                    e.EmployeeDateOfBirth,
                    d.DepartmentId, 
                    d.DepartmentName
                FROM Employee e
                JOIN Department d ON e.DepartmentId = d.DepartmentId
                WHERE e.DepartmentId = @DepartmentId
                ORDER BY e.EmployeeId ASC";

            using (var command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DepartmentId", departmentId);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                            EmployeeName = reader["EmployeeName"].ToString(),
                            EmployeeAge = Convert.ToInt32(reader["EmployeeAge"]),
                            EmployeeLocation = reader["EmployeeLocation"].ToString(),
                            EmployeeDateOfBirth = Convert.ToDateTime(reader["EmployeeDateOfBirth"]),
                            DepartmentId = Convert.ToInt32(reader["DepartmentId"]),
                            DepartmentName = reader["DepartmentName"].ToString(),
                            FileName = reader["FileName"].ToString()
                        });
                    }
                }
            }
        }
        return employees;
    }
     public void CreateDepartment(Department department)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = new NpgsqlCommand("CALL AddDepartment(@DepartmentName)", connection))
            {
                command.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                command.ExecuteNonQuery();
            }
        }
    }
    // 
    public Employee CreateEmployee(Employee employee)
{
    using (var connection = new NpgsqlConnection(connectionString))
    {
        connection.Open();
        using (var command = new NpgsqlCommand(@"
            INSERT INTO Employee (EmployeeName, EmployeeAge, EmployeeLocation, EmployeeDateOfBirth, DepartmentId, FileName)
            VALUES (@EmployeeName, @EmployeeAge, @EmployeeLocation, @EmployeeDateOfBirth, @DepartmentId, @FileName)
            RETURNING EmployeeId;", connection)) // ✅ RETURNING EmployeeId ensures we get the generated ID
        {
            command.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@EmployeeAge", employee.EmployeeAge);
            command.Parameters.AddWithValue("@EmployeeLocation", employee.EmployeeLocation ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@EmployeeDateOfBirth", employee.EmployeeDateOfBirth.Date);
            command.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);
            command.Parameters.AddWithValue("@FileName", employee.FileName ?? (object)DBNull.Value);

            int newEmployeeId = Convert.ToInt32(command.ExecuteScalar());

            employee.EmployeeId = newEmployeeId;
            return employee;
        }
    }
}


    public void UpdateEmployeeById(int employeeId,[FromBody] Employee updatedEmployee)
    {
            using (var connection = new NpgsqlConnection(connectionString))
            {
            connection.Open();
                using(var command=new NpgsqlCommand("CALL  UpdateEmployee(@EmployeeId::INTEGER, @EmployeeName::VARCHAR, @EmployeeAge::INTEGER, @EmployeeLocation::VARCHAR, @EmployeeDateOfBirth::DATE, @DepartmentId::INTEGER,@FileName::VARCHAR)", connection))
                {   
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    command.Parameters.AddWithValue("@EmployeeName", updatedEmployee.EmployeeName);
                    command.Parameters.AddWithValue("@EmployeeAge", updatedEmployee.EmployeeAge);
                    command.Parameters.AddWithValue("@EmployeeLocation", updatedEmployee.EmployeeLocation);
                    command.Parameters.AddWithValue("@EmployeeDateOfBirth", updatedEmployee.EmployeeDateOfBirth.Date);
                    command.Parameters.AddWithValue("@DepartmentId", updatedEmployee.DepartmentId);
                    command.Parameters.AddWithValue("@FileName", (object)updatedEmployee.FileName ?? DBNull.Value);
                    command.ExecuteNonQuery();

                }
            }
        }

        public void UpdateDepartmentById(int departmentId, Department updatedDepartment)
        {
           using (var connection = new NpgsqlConnection(connectionString))
            {
            connection.Open();
                using(var command=new NpgsqlCommand("CALL UpdateDepartment(@DepartmentID, @DepartmentName)",connection))
                {
                    command.Parameters.AddWithValue("@DepartmentID", departmentId);
                    command.Parameters.AddWithValue("@DepartmentName", updatedDepartment.DepartmentName);
                    command.ExecuteNonQuery();
                } 
            }
        }

        public void RemoveDepartmentById(int departmentId)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
            connection.Open();
                using(var command=new NpgsqlCommand("CALL DeleteDepartment(@DepartmentID)",connection))
                {
                    command.Parameters.AddWithValue("@DepartmentID", departmentId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void RemoveEmployeeById(int employeeId)
        {
            using(var connection=new NpgsqlConnection(connectionString))
            {
            connection.Open();
                using(var command=new NpgsqlCommand("CALL DeleteEmployee(@EmployeeId)",connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool CheckIfDepartmentHasEmployees(int departmentId)
        {
            using (var connection=new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command=new NpgsqlCommand("SELECT COUNT(*) FROM Employee WHERE DepartmentId = @DepartmentId", connection))
                {
                    command.Parameters.AddWithValue("@DepartmentId", departmentId);
                    int employeeCount = Convert.ToInt32(command.ExecuteScalar());
                    return employeeCount > 0;  
                }
            }
        }

        public void SaveEmployeeFile(int employeeId, string fileName)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("UPDATE Employee SET FileName = @FileName WHERE EmployeeId = @EmployeeId", connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    command.Parameters.AddWithValue("@FileName", fileName ?? (object)DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

    public string? GetEmployeeFileName(int employeeId)
{
    using (var connection = new NpgsqlConnection(connectionString))
    {
        connection.Open();
        string query = "SELECT FileName FROM Employee WHERE EmployeeId = @EmployeeId";

        using (var command = new NpgsqlCommand(query, connection))
        {
            command.Parameters.AddWithValue("@EmployeeId", employeeId);

            var result = command.ExecuteScalar();
            if (result != DBNull.Value && result != null)
            {
                return result.ToString();  // ✅ Returns the filename if found
            }
        }
    }
    return null;  // ✅ Ensures all code paths return a value
}

}
